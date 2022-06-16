using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Pointer event dispatch helper extension methods.
/// </summary>
public static class TouchEventDispatchExtensions
{
	/// <summary>
	/// Raises the <c>@ontouchcancel</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void TouchCancel(this IElement element)
		=> _ = TouchCancelAsync(element, new TouchEventArgs());

	/// <summary>
	/// Raises the <c>@ontouchcancel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void TouchCancel(this IElement element, TouchEventArgs eventArgs)
		=> _ = TouchCancelAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ontouchcancel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task TouchCancelAsync(this IElement element, TouchEventArgs eventArgs)
		=> element.TriggerEventAsync("ontouchcancel", eventArgs);

	/// <summary>
	/// Raises the <c>@ontouchend</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void TouchEnd(this IElement element)
		=> _ = TouchEndAsync(element, new TouchEventArgs());

	/// <summary>
	/// Raises the <c>@ontouchend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void TouchEnd(this IElement element, TouchEventArgs eventArgs)
		=> _ = TouchEndAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ontouchend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task TouchEndAsync(this IElement element, TouchEventArgs eventArgs)
		=> element.TriggerEventAsync("ontouchend", eventArgs);

	/// <summary>
	/// Raises the <c>@ontouchmove</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void TouchMove(this IElement element)
		=> _ = TouchMoveAsync(element, new TouchEventArgs());

	/// <summary>
	/// Raises the <c>@ontouchmove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void TouchMove(this IElement element, TouchEventArgs eventArgs)
		=> _ = TouchMoveAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ontouchmove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task TouchMoveAsync(this IElement element, TouchEventArgs eventArgs)
		=> element.TriggerEventAsync("ontouchmove", eventArgs);

	/// <summary>
	/// Raises the <c>@ontouchstart</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void TouchStart(this IElement element)
		=> _ = TouchStartAsync(element, new TouchEventArgs());

	/// <summary>
	/// Raises the <c>@ontouchstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void TouchStart(this IElement element, TouchEventArgs eventArgs)
		=> _ = TouchStartAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ontouchstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task TouchStartAsync(this IElement element, TouchEventArgs eventArgs)
		=> element.TriggerEventAsync("ontouchstart", eventArgs);

	/// <summary>
	/// Raises the <c>@ontouchenter</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void TouchEnter(this IElement element)
		=> _ = TouchEnterAsync(element, new TouchEventArgs());

	/// <summary>
	/// Raises the <c>@ontouchenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void TouchEnter(this IElement element, TouchEventArgs eventArgs)
		=> _ = TouchEnterAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ontouchenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task TouchEnterAsync(this IElement element, TouchEventArgs eventArgs)
		=> element.TriggerEventAsync("ontouchenter", eventArgs);

	/// <summary>
	/// Raises the <c>@ontouchleave</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void TouchLeave(this IElement element)
		=> _ = TouchLeaveAsync(element, new TouchEventArgs());

	/// <summary>
	/// Raises the <c>@ontouchleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void TouchLeave(this IElement element, TouchEventArgs eventArgs)
		=> _ = TouchLeaveAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ontouchleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task TouchLeaveAsync(this IElement element, TouchEventArgs eventArgs)
		=> element.TriggerEventAsync("ontouchleave", eventArgs);
}