using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bunit
{
	/// <summary>
	/// A custom Blazor renderer used when testing Blazor components.
	/// </summary>
	[SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
	public class TestRenderer : Renderer
	{
		private readonly RenderEventPublisher _renderEventPublisher;
		private readonly ILogger _logger;
		private Exception? _unhandledException;

		/// <inheritdoc/>
		public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

		/// <summary>
		/// Gets an <see cref="IObservable{RenderEvent}"/> which will provide subscribers with <see cref="RenderEvent"/>s from the
		/// <see cref="TestRenderer"/> during its life time.
		/// </summary>
		public IObservable<RenderEvent> RenderEvents { get; }

		/// <inheritdoc/>
		public TestRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
		{
			_renderEventPublisher = new RenderEventPublisher();
			_logger = loggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance;
			RenderEvents = _renderEventPublisher;
		}

		/// <inheritdoc/>
		public new ArrayRange<RenderTreeFrame> GetCurrentRenderTreeFrames(int componentId)
		{
			try
			{
				return base.GetCurrentRenderTreeFrames(componentId);
			}
			catch (ArgumentException ex) when (ex.Message.Equals($"The renderer does not have a component with ID {componentId}.", StringComparison.Ordinal))
			{ }
			return new ArrayRange<RenderTreeFrame>(Array.Empty<RenderTreeFrame>(), 0);
		}

		/// <inheritdoc/>
		public int AttachTestRootComponent(IComponent testRootComponent)
			=> AssignRootComponentId(testRootComponent);

		/// <inheritdoc/>
		public new Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo fieldInfo, EventArgs eventArgs)
		{
			if (fieldInfo is null)
				throw new ArgumentNullException(nameof(fieldInfo));
			_logger.LogDebug(new EventId(1, nameof(DispatchEventAsync)), $"Starting trigger of '{fieldInfo.FieldValue}'");

			var task = Dispatcher.InvokeAsync(() =>
			{
				try
				{
					return base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs);
				}
				catch (Exception e)
				{
					_unhandledException = e;
					throw;
				}
			});

			AssertNoSynchronousErrors();

			_logger.LogDebug(new EventId(1, nameof(DispatchEventAsync)), $"Finished trigger of '{fieldInfo.FieldValue}'");
			return task;
		}

		/// <inheritdoc/>
		protected override void HandleException(Exception exception)
		{
			_unhandledException = exception;
		}

		/// <inheritdoc/>
		protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
		{
			_logger.LogDebug(new EventId(0, nameof(UpdateDisplayAsync)), $"New render batch with ReferenceFrames = {renderBatch.ReferenceFrames.Count}, UpdatedComponents = {renderBatch.UpdatedComponents.Count}, DisposedComponentIDs = {renderBatch.DisposedComponentIDs.Count}, DisposedEventHandlerIDs = {renderBatch.DisposedEventHandlerIDs.Count}");
			var renderEvent = new RenderEvent(in renderBatch, this);
			_renderEventPublisher.OnRender(renderEvent);
			return Task.CompletedTask;
		}

		/// <summary>
		/// Dispatches an callback in the context of the renderer synchronously and 
		/// asserts no errors happened during dispatch
		/// </summary>
		/// <param name="callback"></param>
		public void DispatchAndAssertNoSynchronousErrors(Action callback)
		{
			Dispatcher.InvokeAsync(callback).Wait();
			AssertNoSynchronousErrors();
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			_renderEventPublisher.OnCompleted();
			base.Dispose(disposing);
		}

		private void AssertNoSynchronousErrors()
		{
			if (_unhandledException is { })
			{
				ExceptionDispatchInfo.Capture(_unhandledException).Throw();
			}
		}
	}
}
