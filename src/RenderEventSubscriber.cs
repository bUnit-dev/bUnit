using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunit
{
    /// <summary>
    /// Represents a subscriber to <see cref="RenderEvent"/>s, published by
    /// the <see cref="TestRenderer"/>.
    /// </summary>
    public sealed class RenderEventSubscriber : IObserver<RenderEvent>, IDisposable
    {
        private IDisposable _unsubscriber;

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
        /// Gets or sets a callback to invoke when a <see cref="RenderEvent"/> is received.
        /// </summary>
        public Action<RenderEvent>? OnRender { get; set; }

        /// <summary>
        /// Gets or sets a callback to invoke when the <see cref="TestRenderer"/> is 
        /// disposed and no more renders will happen.
        /// </summary>
        public Action? OnCompleted { get; set; }

        /// <summary>
        /// Creates an instance of the <see cref="RenderEventSubscriber"/>, and
        /// subscribes to the provided <paramref name="observable"/>.
        /// </summary>
        public RenderEventSubscriber(IObservable<RenderEvent> observable)
        {
            if (observable is null) throw new ArgumentNullException(nameof(observable));
            _unsubscriber = observable.Subscribe(this);
        }

        /// <summary>
        /// Unsubscribes from the observable.
        /// </summary>
        public void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }

        /// <summary>
        /// Unsubscribes from the observable.
        /// </summary>
        public void Dispose() => Unsubscribe();

        /// <inheritdoc/>
        void IObserver<RenderEvent>.OnNext(RenderEvent value)
        {
            RenderCount += 1;
            LatestRenderEvent = value;
            OnRender?.Invoke(value);
        }

        /// <inheritdoc/>
        void IObserver<RenderEvent>.OnCompleted()
        {
            IsCompleted = true;
            OnCompleted?.Invoke();
        }

        /// <inheritdoc/>
        void IObserver<RenderEvent>.OnError(Exception error)
            => throw new AggregateException("The renderer throw an error", error);
    }
}
