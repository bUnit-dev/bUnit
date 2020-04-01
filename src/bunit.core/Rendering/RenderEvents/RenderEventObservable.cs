using System;
using System.Collections.Generic;

namespace Bunit.Rendering.RenderEvents
{
	/// <summary>
	/// Represents a <see cref="RenderEvent"/> <see cref="IObservable{RenderEvent}"/>.
	/// </summary>
	public class RenderEventObservable : IObservable<RenderEvent>
	{
		/// <summary>
		/// Gets the observers currently subscribed to the observable.
		/// </summary>
		protected HashSet<IObserver<RenderEvent>> Observers { get; } = new HashSet<IObserver<RenderEvent>>();

		/// <inheritdoc/>
		public virtual IDisposable Subscribe(IObserver<RenderEvent> observer)
		{
			if (!Observers.Contains(observer))
				Observers.Add(observer);
			return new Unsubscriber(this, observer);
		}

		/// <summary>
		/// Unsubscribes the <paramref name="observer"/> from this observable.
		/// </summary>
		/// <param name="observer">Observer to remove.</param>
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
}
