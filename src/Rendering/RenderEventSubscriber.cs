using System;

namespace Bunit
{
    /// <summary>
    /// Represents a subscriber to <see cref="RenderEvent"/>s, published by
    /// the <see cref="TestRenderer"/>.
    /// </summary>
    public class RenderEventSubscriber : IObserver<RenderEvent>
    {
        private readonly IDisposable _unsubscriber;
        private readonly Action<RenderEvent>? _onRender;
        private readonly Action? _onCompleted;

        /// <summary>
        /// Gets the number of renders that have occurred since subscribing.
        /// </summary>
        public int RenderCount { get; private set; }

        /// <summary>
        /// Gets whether the <see cref="TestRenderer"/> is disposed an no more
        /// renders will happen.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Gets the latests <see cref="RenderEvent"/> received by the <see cref="TestRenderer"/>.
        /// </summary>
        public RenderEvent? LatestRenderEvent { get; private set; }

        /// <summary>
        /// Creates an instance of the <see cref="RenderEventSubscriber"/>, and
        /// subscribes to the provided <paramref name="observable"/>.
        /// </summary>
        /// <param name="observable">The observable to observe.</param>
        /// <param name="onRender">A callback to invoke when a <see cref="RenderEvent"/> is received.</param>
        /// <param name="onCompleted">A callback to invoke when no more renders will happen.</param>
        public RenderEventSubscriber(IObservable<RenderEvent> observable, Action<RenderEvent>? onRender = null, Action? onCompleted = null)
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
            RenderCount += 1;
            LatestRenderEvent = value;
            _onRender?.Invoke(value);
        }

        /// <inheritdoc/>
        public virtual void OnCompleted()
        {
            IsCompleted = true;
            _onCompleted?.Invoke();
        }

        /// <inheritdoc/>
        public virtual void OnError(Exception exception)
            => throw new AggregateException("The renderer throw an error", exception);
    }
}
