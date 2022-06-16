using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Pointer event dispatch helper extension methods.
/// </summary>
public static class ProgressEventDispatchExtensions
{
	/// <summary>
	/// Raises the <c>@onloadstart</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void LoadStart(this IElement element)
		=> _ = LoadStartAsync(element, new ProgressEventArgs());

	/// <summary>
	/// Raises the <c>@onloadstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void LoadStart(this IElement element, ProgressEventArgs eventArgs) => _ = LoadStartAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onloadstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task LoadStartAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onloadstart", eventArgs);

	/// <summary>
	/// Raises the <c>@ontimeout</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Timeout(this IElement element)
		=> _ = TimeoutAsync(element, new ProgressEventArgs());

	/// <summary>
	/// Raises the <c>@ontimeout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void Timeout(this IElement element, ProgressEventArgs eventArgs) => _ = TimeoutAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ontimeout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task TimeoutAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("ontimeout", eventArgs);

	/// <summary>
	/// Raises the <c>@onabort</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Abort(this IElement element)
		=> _ = AbortAsync(element, new ProgressEventArgs());

	/// <summary>
	/// Raises the <c>@onabort</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void Abort(this IElement element, ProgressEventArgs eventArgs) => _ = AbortAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onabort</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task AbortAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onabort", eventArgs);

	/// <summary>
	/// Raises the <c>@onload</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Load(this IElement element)
		=> _ = LoadAsync(element, new ProgressEventArgs());

	/// <summary>
	/// Raises the <c>@onload</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void Load(this IElement element, ProgressEventArgs eventArgs) => _ = LoadAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onload</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task LoadAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onload", eventArgs);

	/// <summary>
	/// Raises the <c>@onloadend</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void LoadEnd(this IElement element)
		=> _ = LoadEndAsync(element, new ProgressEventArgs());

	/// <summary>
	/// Raises the <c>@onloadend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void LoadEnd(this IElement element, ProgressEventArgs eventArgs) => _ = LoadEndAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onloadend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task LoadEndAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onloadend", eventArgs);

	/// <summary>
	/// Raises the <c>@onprogress</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Progress(this IElement element)
		=> _ = ProgressAsync(element, new ProgressEventArgs());

	/// <summary>
	/// Raises the <c>@onprogress</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void Progress(this IElement element, ProgressEventArgs eventArgs) => _ = ProgressAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onprogress</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task ProgressAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onprogress", eventArgs);

	/// <summary>
	/// Raises the <c>@onerror</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Error(this IElement element)
		=> _ = ErrorAsync(element, new ProgressEventArgs());

	/// <summary>
	/// Raises the <c>@onerror</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void Error(this IElement element, ProgressEventArgs eventArgs) => _ = ErrorAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onerror</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task ErrorAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onerror", eventArgs);
}