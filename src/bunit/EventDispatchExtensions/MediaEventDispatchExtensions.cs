using AngleSharp.Dom;

namespace Bunit;

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
	public static void CanPlay(this IElement element) => _ = CanPlayAsync(element);

	/// <summary>
	/// Raises the <c>@oncanplay</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task CanPlayAsync(this IElement element) => element.TriggerEventAsync("oncanplay", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@oncanplay</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task CanPlayAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await CanPlayAsync(element);
	}

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
	public static Task CanPlayThroughAsync(this IElement element) => element.TriggerEventAsync("oncanplaythrough", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@oncanplaythrough</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task CanPlayThroughAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await CanPlayThroughAsync(element);
	}

	/// <summary>
	/// Raises the <c>@oncuechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void CueChange(this IElement element) => _ = CueChangeAsync(element);

	/// <summary>
	/// Raises the <c>@oncuechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task CueChangeAsync(this IElement element) => element.TriggerEventAsync("oncuechange", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@oncuechange</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task CueChangeAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await CueChangeAsync(element);
	}

	/// <summary>
	/// Raises the <c>@ondurationchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void DurationChange(this IElement element) => _ = DurationChangeAsync(element);

	/// <summary>
	/// Raises the <c>@ondurationchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task DurationChangeAsync(this IElement element) => element.TriggerEventAsync("ondurationchange", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@ondurationchange</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task DurationChangeAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await DurationChangeAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onemptied</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Emptied(this IElement element) => _ = EmptiedAsync(element);

	/// <summary>
	/// Raises the <c>@onemptied</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task EmptiedAsync(this IElement element) => element.TriggerEventAsync("onemptied", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onemptied</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task EmptiedAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await EmptiedAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onpause</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Pause(this IElement element) => _ = PauseAsync(element);

	/// <summary>
	/// Raises the <c>@onpause</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task PauseAsync(this IElement element) => element.TriggerEventAsync("onpause", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onpause</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task PauseAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await PauseAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onplay</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Play(this IElement element) => _ = PlayAsync(element);

	/// <summary>
	/// Raises the <c>@onplay</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task PlayAsync(this IElement element) => element.TriggerEventAsync("onplay", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onplay</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task PlayAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await PlayAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onplaying</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Playing(this IElement element) => _ = PlayingAsync(element);

	/// <summary>
	/// Raises the <c>@onplaying</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task PlayingAsync(this IElement element) => element.TriggerEventAsync("onplaying", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onplaying</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task PlayingAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await PlayingAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onratechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void RateChange(this IElement element) => _ = RateChangeAsync(element);

	/// <summary>
	/// Raises the <c>@onratechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task RateChangeAsync(this IElement element) => element.TriggerEventAsync("onratechange", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onratechange</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task RateChangeAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await RateChangeAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onseeked</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Seeked(this IElement element) => _ = SeekedAsync(element);

	/// <summary>
	/// Raises the <c>@onseeked</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task SeekedAsync(this IElement element) => element.TriggerEventAsync("onseeked", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onseeked</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task SeekedAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await SeekedAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onseeking</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Seeking(this IElement element) => _ = SeekingAsync(element);

	/// <summary>
	/// Raises the <c>@onseeking</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task SeekingAsync(this IElement element) => element.TriggerEventAsync("onseeking", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onseeking</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task SeekingAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await SeekingAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onstalled</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Stalled(this IElement element) => _ = StalledAsync(element);

	/// <summary>
	/// Raises the <c>@onstalled</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task StalledAsync(this IElement element) => element.TriggerEventAsync("onstalled", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onstalled</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task StalledAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await StalledAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onstop</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Stop(this IElement element) => _ = StopAsync(element);

	/// <summary>
	/// Raises the <c>@onstop</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task StopAsync(this IElement element) => element.TriggerEventAsync("onstop", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onstop</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task StopAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await StopAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onsuspend</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Suspend(this IElement element) => _ = SuspendAsync(element);

	/// <summary>
	/// Raises the <c>@onsuspend</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task SuspendAsync(this IElement element) => element.TriggerEventAsync("onsuspend", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onsuspend</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task SuspendAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await SuspendAsync(element);
	}

	/// <summary>
	/// Raises the <c>@ontimeupdate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void TimeUpdate(this IElement element) => _ = TimeUpdateAsync(element);

	/// <summary>
	/// Raises the <c>@ontimeupdate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task TimeUpdateAsync(this IElement element) => element.TriggerEventAsync("ontimeupdate", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@ontimeupdate</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task TimeUpdateAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await TimeUpdateAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onvolumechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void VolumeChange(this IElement element) => _ = VolumeChangeAsync(element);

	/// <summary>
	/// Raises the <c>@onvolumechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task VolumeChangeAsync(this IElement element) => element.TriggerEventAsync("onvolumechange", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onvolumechange</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task VolumeChangeAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await VolumeChangeAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onwaiting</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Waiting(this IElement element) => _ = WaitingAsync(element);

	/// <summary>
	/// Raises the <c>@onwaiting</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task WaitingAsync(this IElement element) => element.TriggerEventAsync("onwaiting", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onwaiting</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the element on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task WaitingAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await WaitingAsync(element);
	}
}
