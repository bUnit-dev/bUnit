using System;
using System.Collections.Generic;

namespace Bunit
{
    internal class RenderEventObservable : IObservable<RenderEvent>
    {
        protected HashSet<IObserver<RenderEvent>> Observers { get; } = new HashSet<IObserver<RenderEvent>>();

        public virtual IDisposable Subscribe(IObserver<RenderEvent> observer)
        {
            if (!Observers.Contains(observer))
                Observers.Add(observer);
            return new Unsubscriber(this, observer);
        }

        protected virtual void RemoveSubscription(IObserver<RenderEvent> observer)
        {
            Observers.Remove(observer);
        }

        private sealed class Unsubscriber : IDisposable
        {
            private RenderEventObservable _observable;
            private IObserver<RenderEvent> _observer;

            public Unsubscriber(RenderEventObservable observable, IObserver<RenderEvent> observer)
            {
                _observable = observable;
                _observer = observer;
            }

            public void Dispose()
            {
                _observable.RemoveSubscription(_observer);
            }
        }
    }

    internal sealed class RenderEventFilter : RenderEventObservable, IObservable<RenderEvent>, IObserver<RenderEvent>
    {
        private readonly IObservable<RenderEvent> _source;
        private readonly Func<RenderEvent, bool> _forwardEvent;
        private IDisposable? _subscription;

        public RenderEventFilter(IObservable<RenderEvent> source, Func<RenderEvent, bool> forwardEvent)
        {
            _source = source;
            _forwardEvent = forwardEvent;
        }

        public override IDisposable Subscribe(IObserver<RenderEvent> observer)
        {
            if (_subscription is null)
            {
                _subscription = _source.Subscribe(this);
            }
            return base.Subscribe(observer);
        }

        protected override void RemoveSubscription(IObserver<RenderEvent> observer)
        {
            base.RemoveSubscription(observer);
            if (Observers.Count == 0 && _subscription is { })
            {
                _subscription.Dispose();
                _subscription = null;
            }
        }

        void IObserver<RenderEvent>.OnCompleted()
        {
            foreach (var observer in Observers)
                observer.OnCompleted();
        }
        void IObserver<RenderEvent>.OnError(Exception error)
        {
            foreach (var observer in Observers)
                observer.OnError(error);
        }

        void IObserver<RenderEvent>.OnNext(RenderEvent renderEvent)
        {
            if (!_forwardEvent(renderEvent)) return;
            foreach (var observer in Observers)
                observer.OnNext(renderEvent);
        }
    }

    internal sealed class RenderEventPublisher : RenderEventObservable, IObservable<RenderEvent>
    {
        private bool _isCompleted;

        public void OnRender(in RenderEvent renderEvent)
        {
            if (_isCompleted) throw new InvalidOperationException($"Calling {nameof(OnRender)} is not allowed after {nameof(OnCompleted)} has been called.");
            foreach (var observer in Observers)
                observer.OnNext(renderEvent);
        }

        public void OnCompleted()
        {
            _isCompleted = true;
            foreach (var observer in Observers)
                observer.OnCompleted();
        }
    }
}
