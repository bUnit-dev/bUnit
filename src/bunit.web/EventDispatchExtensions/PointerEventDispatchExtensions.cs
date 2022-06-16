using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Pointer event dispatch helper extension methods.
/// </summary>
public static class PointerEventDispatchExtensions
{
	/// <summary>
	/// Raises the <c>@ongotpointercapture</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void GotPointerCapture(this IElement element)
		=> _ = GotPointerCaptureAsync(element, new PointerEventArgs());

	/// <summary>
	/// Raises the <c>@ongotpointercapture</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void GotPointerCapture(this IElement element, PointerEventArgs eventArgs) => _ = GotPointerCaptureAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ongotpointercapture</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task GotPointerCaptureAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("ongotpointercapture", eventArgs);

	/// <summary>
	/// Raises the <c>@onlostpointercapture</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void LostPointerCapture(this IElement element)
		=> _ = LostPointerCaptureAsync(element, new PointerEventArgs());

	/// <summary>
	/// Raises the <c>@onlostpointercapture</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void LostPointerCapture(this IElement element, PointerEventArgs eventArgs) => _ = LostPointerCaptureAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onlostpointercapture</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task LostPointerCaptureAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onlostpointercapture", eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerdown</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void PointerDown(this IElement element)
		=> _ = PointerDownAsync(element, new PointerEventArgs());

	/// <summary>
	/// Raises the <c>@onpointerdown</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void PointerDown(this IElement element, PointerEventArgs eventArgs) => _ = PointerDownAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerdown</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task PointerDownAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerdown", eventArgs);

	/// <summary>
	/// Raises the <c>@onpointercancel</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void PointerCancel(this IElement element)
		=> _ = PointerCancelAsync(element, new PointerEventArgs());

	/// <summary>
	/// Raises the <c>@onpointercancel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void PointerCancel(this IElement element, PointerEventArgs eventArgs) => _ = PointerCancelAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onpointercancel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task PointerCancelAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointercancel", eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerenter</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void PointerEnter(this IElement element)
		=> _ = PointerEnterAsync(element, new PointerEventArgs());

	/// <summary>
	/// Raises the <c>@onpointerenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void PointerEnter(this IElement element, PointerEventArgs eventArgs) => _ = PointerEnterAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task PointerEnterAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerenter", eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerleave</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void PointerLeave(this IElement element)
		=> _ = PointerLeaveAsync(element, new PointerEventArgs());

	/// <summary>
	/// Raises the <c>@onpointerleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void PointerLeave(this IElement element, PointerEventArgs eventArgs) => _ = PointerLeaveAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task PointerLeaveAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerleave", eventArgs);

	/// <summary>
	/// Raises the <c>@onpointermove</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void PointerMove(this IElement element)
		=> _ = PointerMoveAsync(element, new PointerEventArgs());

	/// <summary>
	/// Raises the <c>@onpointermove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void PointerMove(this IElement element, PointerEventArgs eventArgs) => _ = PointerMoveAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onpointermove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task PointerMoveAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointermove", eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerout</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void PointerOut(this IElement element)
		=> _ = PointerOutAsync(element, new PointerEventArgs());

	/// <summary>
	/// Raises the <c>@onpointerout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void PointerOut(this IElement element, PointerEventArgs eventArgs) => _ = PointerOutAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task PointerOutAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerout", eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerover</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void PointerOver(this IElement element)
		=> _ = PointerOverAsync(element, new PointerEventArgs());

	/// <summary>
	/// Raises the <c>@onpointerover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void PointerOver(this IElement element, PointerEventArgs eventArgs) => _ = PointerOverAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task PointerOverAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerover", eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerup</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void PointerUp(this IElement element)
		=> _ = PointerUpAsync(element, new PointerEventArgs());

	/// <summary>
	/// Raises the <c>@onpointerup</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void PointerUp(this IElement element, PointerEventArgs eventArgs) => _ = PointerUpAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onpointerup</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task PointerUpAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerup", eventArgs);
}