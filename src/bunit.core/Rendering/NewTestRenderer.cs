using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bunit.Rendering
{
	public interface IRenderedComponent : IDisposable
	{
		bool IsDisposed { get; }

		int ComponentId { get; }

		void OnRender(NewRenderEvent renderEvent);
	}

	public interface IRenderedComponentActivator
	{
		IRenderedComponent CreateRenderedComponent(int componentId);
		IRenderedComponent CreateRenderedComponent<T>(int componentId) where T : IComponent;
		IRenderedComponent CreateRenderedComponent<T>(int componentId, T component, RenderTreeFrameCollection componentFrames) where T : IComponent;
	}

	public sealed class NewTestRenderer : Renderer
	{
		private readonly object _renderTreeAccessLock = new object();
		private readonly ILogger _logger;
		private readonly IRenderedComponentActivator _activator;
		private Exception? _unhandledException;
		private readonly Dictionary<int, IRenderedComponent> _renderedComponents = new Dictionary<int, IRenderedComponent>();

		public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

		public NewTestRenderer(IRenderedComponentActivator activator, IServiceProvider services, ILoggerFactory loggerFactory) : base(services, loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<TestRenderer>();
			_activator = activator;
		}

		public IRenderedComponent RenderFragment(RenderFragment renderFragment)
		{
			IRenderedComponent renderedComponent = default!;
			var task = Dispatcher.InvokeAsync(() =>
			{
				var root = new WrapperComponent(renderFragment);
				var rootComponentId = AssignRootComponentId(root);
				renderedComponent = _activator.CreateRenderedComponent(rootComponentId);
				_renderedComponents.Add(rootComponentId, renderedComponent);
				root.Render();
			});

			Debug.Assert(task.IsCompleted, "The render task did not complete as expected");
			AssertNoUnhandledExceptions();

			return renderedComponent!;
		}

		public IRenderedComponent RenderComponent<TComponent>(IEnumerable<ComponentParameter> componentParameters)
			where TComponent : IComponent
		{
			IRenderedComponent renderedComponent = default!;
			var task = Dispatcher.InvokeAsync(() =>
			{
				var root = new WrapperComponent(componentParameters.ToComponentRenderFragment<TComponent>());
				var rootComponentId = AssignRootComponentId(root);
				renderedComponent = _activator.CreateRenderedComponent<TComponent>(rootComponentId);
				_renderedComponents.Add(rootComponentId, renderedComponent);
				root.Render();
			});

			Debug.Assert(task.IsCompleted, "The render task did not complete as expected");
			AssertNoUnhandledExceptions();

			return renderedComponent!;
		}


		public IRenderedComponent FindComponent<TComponent>(IRenderedComponent parentComponent)
			where TComponent : IComponent
		{
			if (parentComponent is null)
				throw new ArgumentNullException(nameof(parentComponent));

			var framesCollection = new RenderTreeFrameCollection();

			lock (_renderTreeAccessLock)
			{
				if (TryFindComponent(parentComponent.ComponentId, out var id, out var component))
				{
					LoadRenderTreeFrames(id, framesCollection);
					var rc = _activator.CreateRenderedComponent(id, component, framesCollection);
					_renderedComponents.Add(rc.ComponentId, rc);
					return rc;
				}
				else
				{
					throw new ComponentNotFoundException(typeof(TComponent));
				}
			}

			bool TryFindComponent(int parentComponentId, out int componentId, out TComponent component)
			{
				var result = false;
				componentId = -1;
				component = default!;

				var frames = LoadAndGetRenderTreeFrame(framesCollection, parentComponentId);

				for (var i = 0; i < frames.Count; i++)
				{
					ref var frame = ref frames.Array[i];
					if (frame.FrameType == RenderTreeFrameType.Component)
					{
						if (frame.Component is TComponent c)
						{
							componentId = frame.ComponentId;
							component = c;
							result = true;
							break;
						}

						if (TryFindComponent(frame.ComponentId, out componentId, out component))
						{
							result = true;
							break;
						}
					}
				}

				return result;
			}
		}

		public IReadOnlyList<IRenderedComponent> FindComponents<TComponent>(IRenderedComponent parentComponent)
			where TComponent : IComponent
		{
			if (parentComponent is null)
				throw new ArgumentNullException(nameof(parentComponent));

			var result = new List<IRenderedComponent>();
			var framesCollection = new RenderTreeFrameCollection();

			lock (_renderTreeAccessLock)
			{
				FindComponentsInternal(parentComponent.ComponentId);
				foreach (var rc in result)
				{
					_renderedComponents.Add(rc.ComponentId, rc);
				}
			}

			return result;

			void FindComponentsInternal(int componentId)
			{
				var frames = LoadAndGetRenderTreeFrame(framesCollection, componentId);

				for (var i = 0; i < frames.Count; i++)
				{
					ref var frame = ref frames.Array[i];
					if (frame.FrameType == RenderTreeFrameType.Component)
					{
						if (frame.Component is TComponent component)
						{
							var id = frame.ComponentId;
							LoadRenderTreeFrames(id, framesCollection);
							var rc = _activator.CreateRenderedComponent(id, component, framesCollection);
							result.Add(rc);
						}

						FindComponentsInternal(frame.ComponentId);
					}
				}
			}
		}

		/// <inheritdoc/>
		protected override void ProcessPendingRender()
		{
			lock (_renderTreeAccessLock)
			{
				base.ProcessPendingRender();
			}
		}

		/// <inheritdoc/>
		protected override void HandleException(Exception exception)
			=> _unhandledException = exception;

		/// <inheritdoc/>
		protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
		{
			var renderEvent = new NewRenderEvent(renderBatch, new RenderTreeFrameCollection());

			// removes disposed components
			for (var i = 0; i < renderBatch.DisposedComponentIDs.Count; i++)
			{
				var id = renderBatch.DisposedComponentIDs.Array[i];
				if (_renderedComponents.TryGetValue(id, out var rc))
				{
					_renderedComponents.Remove(id);
					rc.OnRender(renderEvent);
				}
			}

			foreach (var (key, rc) in _renderedComponents.ToArray())
			{
				LoadRenderTreeFrames(rc.ComponentId, renderEvent.Frames);
				rc.OnRender(renderEvent);

				// RC can replace the instance of the component is bound
				// to while processing the update event. 
				if (key != rc.ComponentId)
				{
					_renderedComponents.Remove(key);
					_renderedComponents.Add(rc.ComponentId, rc);
				}
			}

			return Task.CompletedTask;
		}

		protected override void Dispose(bool disposing)
		{
			foreach (var rc in _renderedComponents.Values)
			{
				rc.Dispose();
			}
			base.Dispose(disposing);
		}

		private void LoadRenderTreeFrames(int componentId, RenderTreeFrameCollection framesCollection)
		{
			var frames = LoadAndGetRenderTreeFrame(framesCollection, componentId);

			for (var i = 0; i < frames.Count; i++)
			{
				ref var frame = ref frames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					LoadRenderTreeFrames(frame.ComponentId, framesCollection);
				}
			}
		}

		private ArrayRange<RenderTreeFrame> LoadAndGetRenderTreeFrame(RenderTreeFrameCollection framesCollection, int componentId)
		{
			if (!framesCollection.Contains(componentId))
			{
				framesCollection.Add(componentId, GetCurrentRenderTreeFrames(componentId));
			}

			return framesCollection[componentId];
		}

		private void AssertNoUnhandledExceptions()
		{
			if (_unhandledException is { } unhandled)
			{
				_unhandledException = null;
				var evt = new EventId(3, nameof(AssertNoUnhandledExceptions));
				_logger.LogError(evt, unhandled, $"An unhandled exception happened during rendering: {unhandled.Message}{Environment.NewLine}{unhandled.StackTrace}");
				ExceptionDispatchInfo.Capture(unhandled).Throw();
			}
		}
	}
}
