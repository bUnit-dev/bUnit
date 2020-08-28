#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
	public interface IRenderedComponent
	{
		void OnRender(IReadOnlyDictionary<int, ArrayRange<RenderTreeFrame>> currentRenderTree);
	}

	public interface IRenderedComponentActivator
	{
		IRenderedComponent CreateRenderedComponent(int componentId);
		IRenderedComponent CreateRenderedComponent<T>(int componentId) where T : IComponent;
	}

	public sealed class NewTestRenderer : Renderer
	{
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

		public T RenderFragment<T>(RenderFragment renderFragment) where T : IRenderedComponent
		{
			T renderedComponent = default;
			var task = Dispatcher.InvokeAsync(() =>
			{
				var root = new WrapperComponent(renderFragment);
				var rootComponentId = AssignRootComponentId(root);
				renderedComponent = (T)_activator.CreateRenderedComponent(rootComponentId);
				_renderedComponents.Add(rootComponentId, renderedComponent);
				root.Render();
			});

			Debug.Assert(task.IsCompleted, "The render task did not complete as expected");
			AssertNoUnhandledExceptions();

			return renderedComponent!;
		}

		public T RenderComponent<T, TComponent>(ComponentParameter[] componentParameters)
			where T : IRenderedComponent
			where TComponent : IComponent
		{
			T renderedComponent = default;
			var task = Dispatcher.InvokeAsync(() =>
			{
				var root = new WrapperComponent(componentParameters.ToComponentRenderFragment<TComponent>());
				var rootComponentId = AssignRootComponentId(root);
				renderedComponent = (T)_activator.CreateRenderedComponent<TComponent>(rootComponentId);
				_renderedComponents.Add(rootComponentId, renderedComponent);
				root.Render();
			});

			Debug.Assert(task.IsCompleted, "The render task did not complete as expected");
			AssertNoUnhandledExceptions();

			return renderedComponent!;
		}

		protected override void HandleException(Exception exception)
			=> _unhandledException = exception;

		protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
		{
			for (var i = 0; i < renderBatch.UpdatedComponents.Count; i++)
			{
				ref var update = ref renderBatch.UpdatedComponents.Array[i];
				var id = update.ComponentId;
				if (_renderedComponents.TryGetValue(id, out var rc))
				{
					var crtf = GetRenderTreeFromRoot(id);

					rc.OnRender(crtf);
				}
			}

			return Task.CompletedTask;
		}

		private Dictionary<int, ArrayRange<RenderTreeFrame>> GetRenderTreeFromRoot(int rootComponentId)
		{
			var result = new Dictionary<int, ArrayRange<RenderTreeFrame>>();
			GetRenderTreeFramesInternal(rootComponentId);
			return result;

			void GetRenderTreeFramesInternal(int componentId)
			{
				var frames = GetCurrentRenderTreeFrames(componentId);
				result.Add(componentId, frames);

				for (var i = 0; i < frames.Count; i++)
				{
					ref var frame = ref frames.Array[i];
					if (frame.FrameType == RenderTreeFrameType.Component)
					{
						GetRenderTreeFramesInternal(frame.ComponentId);
					}
				}
			}
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
