using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Mouse event dispatch helper extension methods.
/// </summary>
public static class MouseEventDispatchExtensions
{
	/// <summary>
	/// Raises the <c>@onmouseover</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void MouseOver(this IElement element)
		=> _ = MouseOverAsync(element, new MouseEventArgs());

	/// <summary>
	/// Raises the <c>@onmouseover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseOver(this IElement element, MouseEventArgs eventArgs)
		=> _ = MouseOverAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmouseover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseOverAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onmouseover", eventArgs);

	/// <summary>
	/// Raises the <c>@onmouseout</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void MouseOut(this IElement element)
		=> _ = MouseOutAsync(element, new MouseEventArgs());

	/// <summary>
	/// Raises the <c>@onmouseout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseOut(this IElement element, MouseEventArgs eventArgs)
		=> _ = MouseOutAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmouseout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseOutAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onmouseout", eventArgs);

	/// <summary>
	/// Raises the <c>@onmousemove</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void MouseMove(this IElement element)
		=> _ = MouseMoveAsync(element, new MouseEventArgs());

	/// <summary>
	/// Raises the <c>@onmousemove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseMove(this IElement element, MouseEventArgs eventArgs)
		=> _ = MouseMoveAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmousemove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseMoveAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onmousemove", eventArgs);

	/// <summary>
	/// Raises the <c>@onmousedown</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void MouseDown(this IElement element)
		=> _ = MouseDownAsync(element, new MouseEventArgs());

	/// <summary>
	/// Raises the <c>@onmousedown</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseDown(this IElement element, MouseEventArgs eventArgs)
		=> _ = MouseDownAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmousedown</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseDownAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onmousedown", eventArgs);

	/// <summary>
	/// Raises the <c>@onmouseup</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void MouseUp(this IElement element)
		=> _ = MouseUpAsync(element, new MouseEventArgs());

	/// <summary>
	/// Raises the <c>@onmouseup</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseUp(this IElement element, MouseEventArgs eventArgs)
		=> _ = MouseUpAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmouseup</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseUpAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onmouseup", eventArgs);

	/// <summary>
	/// Raises the <c>@onclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Click(this IElement element) => _ = ClickAsync(element, new MouseEventArgs { Detail = 1 });

	/// <summary>
	/// Raises the <c>@onclick</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void Click(this IElement element, MouseEventArgs eventArgs) => _ = ClickAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onclick</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task ClickAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onclick", eventArgs);

	/// <summary>
	/// Raises the <c>@ondblclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	public static void DoubleClick(this IElement element)
		=> _ = DoubleClickAsync(element, new MouseEventArgs { Detail = 2 });

	/// <summary>
	/// Raises the <c>@ondblclick</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void DoubleClick(this IElement element, MouseEventArgs eventArgs)
		=> _ = DoubleClickAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ondblclick</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task DoubleClickAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("ondblclick", eventArgs);

	/// <summary>
	/// Raises the <c>@onwheel</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Wheel(this IElement element)
		=> _ = WheelAsync(element, new WheelEventArgs());

	/// <summary>
	/// Raises the <c>@onwheel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void Wheel(this IElement element, WheelEventArgs eventArgs)
		=> _ = WheelAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onwheel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task WheelAsync(this IElement element, WheelEventArgs eventArgs) => element.TriggerEventAsync("onwheel", eventArgs);

	/// <summary>
	/// Raises the <c>@onmousewheel</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void MouseWheel(this IElement element)
		=> MouseWheelAsync(element, new WheelEventArgs());

	/// <summary>
	/// Raises the <c>@onmousewheel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseWheel(this IElement element, WheelEventArgs eventArgs)
		=> _ = MouseWheelAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmousewheel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseWheelAsync(this IElement element, WheelEventArgs eventArgs) => element.TriggerEventAsync("onmousewheel", eventArgs);

	/// <summary>
	/// Raises the <c>@oncontextmenu</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void ContextMenu(this IElement element)
		=> _ = ContextMenuAsync(element, new MouseEventArgs());

	/// <summary>
	/// Raises the <c>@oncontextmenu</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void ContextMenu(this IElement element, MouseEventArgs eventArgs)
		=> _ = ContextMenuAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@oncontextmenu</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task ContextMenuAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("oncontextmenu", eventArgs);
}