using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a bUnit <see cref="ITestRenderer"/> used to render Blazor components and fragments during bUnit tests.
	/// </summary>
	public partial class TestRenderer : Renderer, ITestRenderer
	{
		private readonly object _renderTreeAccessLock = new object();
		private readonly ILogger _logger;
		private readonly IRenderedComponentActivator _activator;
		private Exception? _unhandledException;
		private readonly Dictionary<int, IRenderedFragmentBase> _renderedComponents = new Dictionary<int, IRenderedFragmentBase>();

		/// <inheritdoc/>
		public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

		/// <summary>
		/// Creates an instance of the <see cref="TestRenderer"/>.
		/// </summary>
		public TestRenderer(IRenderedComponentActivator activator, IServiceProvider services, ILoggerFactory loggerFactory) : base(services, loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<TestRenderer>();
			_activator = activator;
		}

		/// <inheritdoc/>
		public IRenderedFragmentBase RenderFragment(RenderFragment renderFragment)
		{
			return Render(renderFragment, id => _activator.CreateRenderedFragment(id));
		}

		/// <inheritdoc/>
		public IRenderedComponentBase<TComponent> RenderComponent<TComponent>(ComponentParameterCollection parameters)
			where TComponent : IComponent
		{
			if (parameters is null) throw new ArgumentNullException(nameof(parameters));

			var renderFragment = parameters.ToComponentRenderFragment<TComponent>();
			return Render(renderFragment, id => _activator.CreateRenderedComponent<TComponent>(id));
		}

		/// <inheritdoc/>
		public new Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo fieldInfo, EventArgs eventArgs)
		{
			if (fieldInfo is null) throw new ArgumentNullException(nameof(fieldInfo));

			var result = Dispatcher.InvokeAsync(() => base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs));

			AssertNoUnhandledExceptions();

			return result;
		}

		/// <inheritdoc/>
		public IRenderedComponentBase<TComponent> FindComponent<TComponent>(IRenderedFragmentBase parentComponent) where TComponent : IComponent
		{
			var foundComponents = FindComponents<TComponent>(parentComponent, 1);
			if (foundComponents.Count == 1)
			{
				return foundComponents[0];
			}
			else
			{
				throw new ComponentNotFoundException(typeof(TComponent));
			}
		}

		/// <inheritdoc/>
		public IReadOnlyList<IRenderedComponentBase<TComponent>> FindComponents<TComponent>(IRenderedFragmentBase parentComponent) where TComponent : IComponent
			=> FindComponents<TComponent>(parentComponent, int.MaxValue);

		/// <inheritdoc/>
		protected override void ProcessPendingRender()
		{
			// the lock is in place to avoid a race condition between
			// the dispatchers thread and the test frameworks thread,
			// where one will read the current render tree (find components)
			// while the other thread (the renderer) updates the
			// render tree.
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
			var renderEvent = new RenderEvent(renderBatch, new RenderTreeFrameCollection());

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

			// notify each rendered component about the render
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

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (var rc in _renderedComponents.Values)
				{
					rc.Dispose();
				}
				_renderedComponents.Clear();
			}
			base.Dispose(disposing);
		}

		private TResult Render<TResult>(RenderFragment renderFragment, Func<int, TResult> activator)
			where TResult : IRenderedFragmentBase
		{
			TResult renderedComponent = default!;

			var task = Dispatcher.InvokeAsync(() =>
			{
				var root = new WrapperComponent(renderFragment);
				var rootComponentId = AssignRootComponentId(root);
				renderedComponent = activator(rootComponentId);
				_renderedComponents.Add(rootComponentId, renderedComponent);
				root.Render();
			});

			task.Wait();

			AssertNoUnhandledExceptions();

			return renderedComponent!;
		}

		private IReadOnlyList<IRenderedComponentBase<TComponent>> FindComponents<TComponent>(IRenderedFragmentBase parentComponent, int resultLimit)
			where TComponent : IComponent
		{
			if (parentComponent is null)
				throw new ArgumentNullException(nameof(parentComponent));

			var result = new List<IRenderedComponentBase<TComponent>>();
			var framesCollection = new RenderTreeFrameCollection();

			// the lock is in place to avoid a race condition between
			// the dispatchers thread and the test frameworks thread,
			// where one will read the current render tree (this method)
			// while the other thread (the renderer) updates the
			// render tree.
			lock (_renderTreeAccessLock)
			{
				FindComponentsInternal(parentComponent.ComponentId);
			}

			return result;

			void FindComponentsInternal(int componentId)
			{
				var frames = GetOrLoadRenderTreeFrame(framesCollection, componentId);

				for (var i = 0; i < frames.Count; i++)
				{
					ref var frame = ref frames.Array[i];
					if (frame.FrameType == RenderTreeFrameType.Component)
					{
						if (frame.Component is TComponent component)
						{
							GetOrCreateRenderedComponent(frame.ComponentId, component);

							if (result.Count == resultLimit)
								return;
						}

						FindComponentsInternal(frame.ComponentId);

						if (result.Count == resultLimit)
							return;
					}
				}
			}

			void GetOrCreateRenderedComponent(int componentId, TComponent component)
			{
				IRenderedComponentBase<TComponent> rc;

				if (_renderedComponents.TryGetValue(componentId, out var rf))
				{
					rc = (IRenderedComponentBase<TComponent>)rf;
				}
				else
				{
					LoadRenderTreeFrames(componentId, framesCollection);
					rc = _activator.CreateRenderedComponent(componentId, component, framesCollection);
					_renderedComponents.Add(rc.ComponentId, rc);
				}

				result.Add(rc);
			}
		}

		/// <summary>
		/// Populates the <paramref name="framesCollection"/> with <see cref="ArrayRange{RenderTreeFrame}"/>
		/// starting with the one that belongs to the component with ID <paramref name="componentId"/>.
		/// </summary>
		private void LoadRenderTreeFrames(int componentId, RenderTreeFrameCollection framesCollection)
		{
			var frames = GetOrLoadRenderTreeFrame(framesCollection, componentId);

			for (var i = 0; i < frames.Count; i++)
			{
				ref var frame = ref frames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					LoadRenderTreeFrames(frame.ComponentId, framesCollection);
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="ArrayRange{RenderTreeFrame}"/> from the <paramref name="framesCollection"/>.
		/// If the <paramref name="framesCollection"/> does not contain the frames, they are loaded into it first.
		/// </summary>
		private ArrayRange<RenderTreeFrame> GetOrLoadRenderTreeFrame(RenderTreeFrameCollection framesCollection, int componentId)
		{
			if (!framesCollection.Contains(componentId))
			{
				var frames = GetCurrentRenderTreeFrames(componentId);
				framesCollection.Add(componentId, frames);
			}

			return framesCollection[componentId];
		}

		private void AssertNoUnhandledExceptions()
		{
			if (_unhandledException is Exception unhandled)
			{
				_unhandledException = null;
				LogUnhandledException(unhandled);
				ExceptionDispatchInfo.Capture(unhandled).Throw();
			}
		}

		private void LogUnhandledException(Exception unhandled)
		{
			var evt = new EventId(3, nameof(AssertNoUnhandledExceptions));
			_logger.LogError(evt, unhandled, $"An unhandled exception happened during rendering: {unhandled.Message}{Environment.NewLine}{unhandled.StackTrace}");
		}
	}
}
