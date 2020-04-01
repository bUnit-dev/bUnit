using System;

namespace Bunit.Rendering.RenderEvents
{
	/// <summary>
	/// Represents a publisher of <see cref="RenderEvent"/>
	/// </summary>
	public sealed class RenderEventPublisher : RenderEventObservable, IObservable<RenderEvent>
	{
		private bool _isCompleted;

		/// <summary>
		/// Sends the <paramref name="renderEvent"/> to all observers/subscribers.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown when <see cref="OnCompleted"/> has been called.</exception>
		/// <param name="renderEvent">The <see cref="RenderEvent"/> to publish.</param>
		public void OnRender(in RenderEvent renderEvent)
		{
			if (_isCompleted)
				throw new InvalidOperationException($"Calling {nameof(OnRender)} is not allowed after {nameof(OnCompleted)} has been called.");
			foreach (var observer in Observers)
				observer.OnNext(renderEvent);
		}

		/// <summary>
		/// Marks the publisher as completed, and signals that to observers/subscribers.
		/// </summary>
		public void OnCompleted()
		{
			_isCompleted = true;
			foreach (var observer in Observers)
				observer.OnCompleted();
		}
	}
}
