using System;
using System.Threading;
using Bunit.Rendering;

namespace Bunit
{
	/// <summary>
	/// Represents a subscriber to <see cref="RenderEvent"/>s, published by
	/// the <see cref="TestRendererOld"/>.
	/// </summary>
	public class ConcurrentRenderEventSubscriber : IObserver<RenderEvent>
    {
        private readonly IDisposable _unsubscriber;
        private readonly Action<RenderEvent>? _onRender;
        private readonly Action? _onCompleted;
        private int _renderCount;
        private bool _isCompleted;
        private RenderEvent? _latestRenderEvent;

        /// <summary>
        /// Gets the number of renders that have occurred since subscribing.
        /// </summary>
        public int RenderCount => Volatile.Read(ref _renderCount);

        /// <summary>
        /// Gets whether the <see cref="TestRendererOld"/> is disposed an no more
        /// renders will happen.
        /// </summary>
        public bool IsCompleted => Volatile.Read(ref _isCompleted);

        /// <summary>
        /// Gets the latests <see cref="RenderEvent"/> received by the <see cref="TestRendererOld"/>.
        /// </summary>
        public RenderEvent? LatestRenderEvent => Volatile.Read(ref _latestRenderEvent);

        /// <summary>
        /// Creates an instance of the <see cref="ConcurrentRenderEventSubscriber"/>, and
        /// subscribes to the provided <paramref name="observable"/>.
        /// </summary>
        /// <param name="observable">The observable to observe.</param>
        /// <param name="onRender">A callback to invoke when a <see cref="RenderEvent"/> is received.</param>
        /// <param name="onCompleted">A callback to invoke when no more renders will happen.</param>
        public ConcurrentRenderEventSubscriber(IObservable<RenderEvent> observable, Action<RenderEvent>? onRender = null, Action? onCompleted = null)
        {
            if (observable is null) throw new ArgumentNullException(nameof(observable));
            _onRender = onRender;
            _onCompleted = onCompleted;
            _unsubscriber = observable.Subscribe(this);
        }

        /// <summary>
        /// Unsubscribes from the observable.
        /// </summary>
        public void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }

        /// <inheritdoc/>
        public virtual void OnNext(RenderEvent value)
        {
            Interlocked.Increment(ref _renderCount);
            Volatile.Write(ref _latestRenderEvent, value);
            _onRender?.Invoke(value);
        }

        /// <inheritdoc/>
        public virtual void OnCompleted()
        {
            Volatile.Write(ref _isCompleted, true);
            _onCompleted?.Invoke();
        }

        /// <inheritdoc/>
        public virtual void OnError(Exception exception)
            => throw new AggregateException("The renderer throw an error", exception);
    }
}
