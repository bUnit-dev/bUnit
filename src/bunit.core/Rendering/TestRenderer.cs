using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

using Bunit.Extensions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;

namespace Bunit.Rendering
{
	/// <summary>
	/// Generalized Blazor renderer for testing purposes.
	/// </summary>
	public class TestRenderer : Renderer, ITestRenderer, IRenderEventProducer
	{
		private readonly ILogger _logger;
		private readonly List<IRenderEventHandler> _renderEventHandlers = new List<IRenderEventHandler>();
		private Exception? _unhandledException;

		/// <inheritdoc/>
		public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

		/// <summary>
		/// Creates an instance of the <see cref="TestRenderer"/> class.
		/// </summary>
		public TestRenderer(IServiceProvider services, ILoggerFactory loggerFactory) : base(services, loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<TestRenderer>();
		}

		/// <summary>
		/// Adds a <see cref="IRenderEventHandler"/> to this renderer,
		/// which will be triggered when the renderer has finished rendering
		/// a render cycle.
		/// </summary>
		/// <param name="handler">The handler to add.</param>
		public void AddRenderEventHandler(IRenderEventHandler handler) => _renderEventHandlers.Add(handler);

		/// <summary>
		/// Removes a <see cref="IRenderEventHandler"/> from this renderer.
		/// </summary>
		/// <param name="handler">The handler to remove.</param>
		public void RemoveRenderEventHandler(IRenderEventHandler handler) => _renderEventHandlers.Remove(handler);

		/// <inheritdoc/>
		public (int ComponentId, TComponent Component) RenderComponent<TComponent>(IEnumerable<ComponentParameter> parameters) where TComponent : IComponent
		{
			var componentType = typeof(TComponent);
			var renderFragment = parameters.ToComponentRenderFragment<TComponent>();
			var wrapperId = RenderFragmentInsideWrapper(renderFragment);
			return FindComponent<TComponent>(wrapperId);
		}

		/// <inheritdoc/>
		public int RenderFragment(RenderFragment renderFragment)
		{
			return RenderFragmentInsideWrapper(renderFragment);
		}

		/// <inheritdoc/>
		public (int ComponentId, TComponent Component) FindComponent<TComponent>(int parentComponentId)
		{
			var result = GetComponent<TComponent>(parentComponentId);
			if (result.HasValue)
				return result.Value;
			else
				throw new ComponentNotFoundException(typeof(TComponent));
		}

		/// <inheritdoc/>
		public IReadOnlyList<(int ComponentId, TComponent Component)> FindComponents<TComponent>(int parentComponentId)
		{
			return GetComponents<TComponent>(parentComponentId);
		}

		/// <inheritdoc/>
		public new ArrayRange<RenderTreeFrame> GetCurrentRenderTreeFrames(int componentId)
		{
			return base.GetCurrentRenderTreeFrames(componentId);
		}

		/// <inheritdoc/>
		public new Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo fieldInfo, EventArgs eventArgs)
		{
			if (fieldInfo is null)
				throw new ArgumentNullException(nameof(fieldInfo));

			_logger.LogDebug(new EventId(10, nameof(DispatchEventAsync)), $"Starting trigger of '{fieldInfo.FieldValue}'");

			var result = Dispatcher.InvokeAsync(() => base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs));

			AssertNoUnhandledExceptions();

			if (result.IsCompletedSuccessfully)
			{
				_logger.LogDebug(new EventId(11, nameof(DispatchEventAsync)), $"Finished trigger synchronously for '{fieldInfo.FieldValue}'");
			}
			else
			{
				_logger.LogDebug(new EventId(13, nameof(DispatchEventAsync)), $"Event handler for '{fieldInfo.FieldValue}' returned an incomplete task with status {result.Status}");
				result = result.ContinueWith(x =>
				{
					if (x.IsCompletedSuccessfully)
					{
						_logger.LogDebug(new EventId(12, nameof(DispatchEventAsync)), $"Finished trigger asynchronously for '{fieldInfo.FieldValue}'");
					}
				}, TaskScheduler.Default);
			}

			return result;
		}

		private int RenderFragmentInsideWrapper(RenderFragment renderFragment)
		{
			var wrapper = new WrapperComponent(renderFragment);

			var wrapperId = AssignRootComponentId(wrapper);
			AssertNoUnhandledExceptions();

			Dispatcher.InvokeAsync(wrapper.Render).Wait();
			AssertNoUnhandledExceptions();

			return wrapperId;
		}

		/// <inheritdoc/>
		protected override void HandleException(Exception exception) => _unhandledException = exception;

		/// <inheritdoc/>
		protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
		{
			_logger.LogDebug(new EventId(0, nameof(UpdateDisplayAsync)), $"New render batch with ReferenceFrames = {renderBatch.ReferenceFrames.Count}, UpdatedComponents = {renderBatch.UpdatedComponents.Count}, DisposedComponentIDs = {renderBatch.DisposedComponentIDs.Count}, DisposedEventHandlerIDs = {renderBatch.DisposedEventHandlerIDs.Count}");

			return _renderEventHandlers.Count == 0
				? Task.CompletedTask
				: PublishRenderEvent(in renderBatch);
		}

		private Task PublishRenderEvent(in RenderBatch renderBatch)
		{
			var renderEvent = new RenderEvent(in renderBatch, this);

			return _renderEventHandlers.Count switch
			{
				0 => Task.CompletedTask,
				1 => _renderEventHandlers[0].Handle(renderEvent),
				_ => NotifyEventHandlers(renderEvent)
			};
		}

		private Task NotifyEventHandlers(RenderEvent renderEvent)
		{
			// copy to new array since _renderEventHandlers might be modified by the
			// Handle method on event handlers if component is disposed.
			var handleTasks = _renderEventHandlers
				.ToArray()
				.Select(x => x.Handle(renderEvent));

			return Task.WhenAll(handleTasks);
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

		private (int ComponentId, TComponent Component)? GetComponent<TComponent>(int rootComponentId)
		{
			var ownFrames = GetCurrentRenderTreeFrames(rootComponentId);

			for (var i = 0; i < ownFrames.Count; i++)
			{
				ref var frame = ref ownFrames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					if (frame.Component is TComponent component)
						return (frame.ComponentId, component);

					var result = GetComponent<TComponent>(frame.ComponentId);
					if (result is { })
						return result;
				}
			}

			return null;
		}

		private IReadOnlyList<(int ComponentId, TComponent Component)> GetComponents<TComponent>(int rootComponentId)
		{
			var result = new List<(int ComponentId, TComponent Component)>();

			GetComponentsInternal(rootComponentId, result);

			return result;

			void GetComponentsInternal(int rootComponentId, List<(int ComponentId, TComponent Component)> result)
			{
				var ownFrames = GetCurrentRenderTreeFrames(rootComponentId);
				for (var i = 0; i < ownFrames.Count; i++)
				{
					ref var frame = ref ownFrames.Array[i];
					if (frame.FrameType == RenderTreeFrameType.Component)
					{
						if (frame.Component is TComponent component)
							result.Add((frame.ComponentId, component));

						GetComponentsInternal(frame.ComponentId, result);
					}
				}
			}
		}
	}
}
