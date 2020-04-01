using System;

namespace Bunit.Rendering.RenderEvents
{
	/// <summary>
	/// Represents a simple filter for <see cref="RenderEventObservable"/>.
	/// </summary>
	public sealed class RenderEventFilter : RenderEventObservable, IObservable<RenderEvent>, IObserver<RenderEvent>
	{
		private readonly IObservable<RenderEvent> _source;
		private readonly Func<RenderEvent, bool> _forwardEvent;
		private IDisposable? _subscription;

		/// <summary>
		/// Creates an instance of the <see cref="RenderEventFilter"/>,
		/// that filters events from <paramref name="source"/> based on the
		/// <paramref name="forwardEvent"/> predicate.
		/// </summary>
		/// <param name="source">Source to filter.</param>
		/// <param name="forwardEvent">Filter to apply to <see cref="RenderEvent"/>.</param>
		public RenderEventFilter(IObservable<RenderEvent> source, Func<RenderEvent, bool> forwardEvent)
		{
			_source = source;
			_forwardEvent = forwardEvent;
		}

		/// <inheritdoc/>
		public override IDisposable Subscribe(IObserver<RenderEvent> observer)
		{
			if (_subscription is null)
				_subscription = _source.Subscribe(this);
			return base.Subscribe(observer);
		}

		/// <inheritdoc/>
		protected override void RemoveSubscription(IObserver<RenderEvent> observer)
		{
			base.RemoveSubscription(observer);
			if (Observers.Count == 0 && _subscription is { })
			{
				_subscription.Dispose();
				_subscription = null;
			}
		}

		/// <inheritdoc/>
		void IObserver<RenderEvent>.OnCompleted()
		{
			foreach (var observer in Observers)
				observer.OnCompleted();
		}

		/// <inheritdoc/>
		void IObserver<RenderEvent>.OnError(Exception error)
		{
			foreach (var observer in Observers)
				observer.OnError(error);
		}

		/// <inheritdoc/>
		void IObserver<RenderEvent>.OnNext(RenderEvent renderEvent)
		{
			if (!_forwardEvent(renderEvent))
				return;
			foreach (var observer in Observers)
				observer.OnNext(renderEvent);
		}
	}
}
