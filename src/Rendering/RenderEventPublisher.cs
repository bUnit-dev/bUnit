using System;
using System.Collections.Generic;

namespace Bunit
{
    /// <summary>
    /// Represents a <see cref="RenderEvent"/> publisher.
    /// </summary>
    internal class RenderEventPublisher : IObservable<RenderEvent>
    {
        private readonly HashSet<IObserver<RenderEvent>> _observers = new HashSet<IObserver<RenderEvent>>();

        public IDisposable Subscribe(IObserver<RenderEvent> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        public void OnRender(in RenderEvent renderEvent)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(renderEvent);
            }
        }

        public void OnCompleted()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        private sealed class Unsubscriber : IDisposable
        {
            private HashSet<IObserver<RenderEvent>> _observers;
            private IObserver<RenderEvent> _observer;

            public Unsubscriber(HashSet<IObserver<RenderEvent>> observers, IObserver<RenderEvent> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                _observers.Remove(_observer);
            }
        }
    }
}
