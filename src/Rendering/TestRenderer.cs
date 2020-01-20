﻿using Egil.RazorComponents.Testing.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// A custom Blazor renderer used when testing Blazor components.
    /// </summary>
    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    public class TestRenderer : Renderer
    {
        private Exception? _unhandledException;

        private TaskCompletionSource<object?> _nextRenderTcs = new TaskCompletionSource<object?>();

        /// <summary>
        /// Gets or sets an action that will be triggered whenever the renderer
        /// detects changes in rendered markup in components or fragments 
        /// after a render.
        /// </summary>
        public StructAction<RenderBatch>? OnRenderingHasComponentUpdates { get; set; }

        /// <inheritdoc/>
        public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

        /// <summary>
        /// Gets a task that completes after the next render.
        /// </summary>
        public Task NextRender => _nextRenderTcs.Task;

        /// <inheritdoc/>
        public TestRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
            : base(serviceProvider, loggerFactory)
        {
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
            // TODO: Capture batches (and the state of component output) for individual inspection
            var prevTcs = _nextRenderTcs;
            _nextRenderTcs = new TaskCompletionSource<object?>();

            NotifyOfComponentsWithChangedMarkup(renderBatch);

            prevTcs.SetResult(null);
            return Task.CompletedTask;
        }

        private void NotifyOfComponentsWithChangedMarkup(in RenderBatch renderBatch)
        {
            if (renderBatch.UpdatedComponents.Count > 0)
            {
                for (int i = 0; i < renderBatch.UpdatedComponents.Count; i++)
                {
                    if (renderBatch.UpdatedComponents.Array[i].Edits.Count > 0)
                    {
                        OnRenderingHasComponentUpdates?.Invoke(renderBatch);
                        return;
                    }
                }
            }
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
            OnRenderingHasComponentUpdates = null;
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
