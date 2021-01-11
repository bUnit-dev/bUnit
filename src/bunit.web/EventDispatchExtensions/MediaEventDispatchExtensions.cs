using System;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Bunit
{
	/// <summary>
	/// Media event dispatch helper extension methods.
	/// </summary>
	public static class MediaEventDispatchExtensions
	{
		/// <summary>
		/// Raises the <c>@oncanplay</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void CanPlay(this IElement element) => CanPlayAsync(element);

		/// <summary>
		/// Raises the <c>@oncanplay</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task CanPlayAsync(this IElement element) => element.TriggerEventAsync("oncanplay", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@oncanplaythrough</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void CanPlayThrough(this IElement element) => _ = CanPlayThroughAsync(element);

		/// <summary>
		/// Raises the <c>@oncanplaythrough</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task CanPlayThroughAsync(this IElement element) => element.TriggerEventAsync("oncanplaythrough", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@oncuechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void CueChange(this IElement element) => CueChangeAsync(element);

		/// <summary>
		/// Raises the <c>@oncuechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task CueChangeAsync(this IElement element) => element.TriggerEventAsync("oncuechange", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@ondurationchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void DurationChange(this IElement element) => DurationChangeAsync(element);

		/// <summary>
		/// Raises the <c>@ondurationchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task DurationChangeAsync(this IElement element) => element.TriggerEventAsync("ondurationchange", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onemptied</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Emptied(this IElement element) => EmptiedAsync(element);

		/// <summary>
		/// Raises the <c>@onemptied</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task EmptiedAsync(this IElement element) => element.TriggerEventAsync("onemptied", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onpause</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Pause(this IElement element) => PauseAsync(element);

		/// <summary>
		/// Raises the <c>@onpause</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PauseAsync(this IElement element) => element.TriggerEventAsync("onpause", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onplay</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Play(this IElement element) => PlayAsync(element);

		/// <summary>
		/// Raises the <c>@onplay</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PlayAsync(this IElement element) => element.TriggerEventAsync("onplay", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onplaying</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Playing(this IElement element) => PlayingAsync(element);

		/// <summary>
		/// Raises the <c>@onplaying</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PlayingAsync(this IElement element) => element.TriggerEventAsync("onplaying", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onratechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void RateChange(this IElement element) => RateChangeAsync(element);

		/// <summary>
		/// Raises the <c>@onratechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task RateChangeAsync(this IElement element) => element.TriggerEventAsync("onratechange", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onseeked</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Seeked(this IElement element) => SeekedAsync(element);

		/// <summary>
		/// Raises the <c>@onseeked</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task SeekedAsync(this IElement element) => element.TriggerEventAsync("onseeked", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onseeking</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Seeking(this IElement element) => SeekingAsync(element);

		/// <summary>
		/// Raises the <c>@onseeking</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task SeekingAsync(this IElement element) => element.TriggerEventAsync("onseeking", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onstalled</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Stalled(this IElement element) => StalledAsync(element);

		/// <summary>
		/// Raises the <c>@onstalled</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task StalledAsync(this IElement element) => element.TriggerEventAsync("onstalled", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onstop</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Stop(this IElement element) => StopAsync(element);

		/// <summary>
		/// Raises the <c>@onstop</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task StopAsync(this IElement element) => element.TriggerEventAsync("onstop", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onsuspend</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Suspend(this IElement element) => SuspendAsync(element);

		/// <summary>
		/// Raises the <c>@onsuspend</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task SuspendAsync(this IElement element) => element.TriggerEventAsync("onsuspend", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@ontimeupdate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void TimeUpdate(this IElement element) => TimeUpdateAsync(element);

		/// <summary>
		/// Raises the <c>@ontimeupdate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task TimeUpdateAsync(this IElement element) => element.TriggerEventAsync("ontimeupdate", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onvolumechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void VolumeChange(this IElement element) => VolumeChangeAsync(element);

		/// <summary>
		/// Raises the <c>@onvolumechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task VolumeChangeAsync(this IElement element) => element.TriggerEventAsync("onvolumechange", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onwaiting</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Waiting(this IElement element) => WaitingAsync(element);

		/// <summary>
		/// Raises the <c>@onwaiting</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task WaitingAsync(this IElement element) => element.TriggerEventAsync("onwaiting", EventArgs.Empty);
	}
}
