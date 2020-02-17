using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Threading;

namespace Bunit
{
    /// <summary>
    /// A custom Blazor renderer used when testing Blazor components.
    /// </summary>
    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    public class TestRenderer : Renderer
    {
        private readonly RenderEventPublisher _renderEventPublisher;
        private Exception? _unhandledException;

        /// <inheritdoc/>
        public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

        /// <summary>
        /// Gets an <see cref="IObservable{RenderEvent}"/> which will provide subscribers with <see cref="RenderEvent"/>s from the
        /// <see cref="TestRenderer"/> during its life time.
        /// </summary>
        public IObservable<RenderEvent> RenderEvents { get; }

        /// <inheritdoc/>
        public TestRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
            _renderEventPublisher = new RenderEventPublisher();
            RenderEvents = _renderEventPublisher;
        }

        /// <inheritdoc/>
        public new ArrayRange<RenderTreeFrame> GetCurrentRenderTreeFrames(int componentId)
            => base.GetCurrentRenderTreeFrames(componentId);

        /// <inheritdoc/>
        public int AttachTestRootComponent(IComponent testRootComponent)
            => AssignRootComponentId(testRootComponent);

        /// <inheritdoc/>
        public new Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo fieldInfo, EventArgs eventArgs)
        {
            var task = Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs);
                }
                catch (Exception e)
                {
                    _unhandledException = e;
                    throw;
                }
            });
            AssertNoSynchronousErrors();
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
