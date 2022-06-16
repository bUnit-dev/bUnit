using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Drag event dispatch helper extension methods.
/// </summary>
public static class DragEventDispatchExtensions
{
	/// <summary>
	/// Raises the <c>@ondrag</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="DragEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Drag(this IElement element)
		=> _ = DragAsync(element, new DragEventArgs());

	/// <summary>
	/// Raises the <c>@ondrag</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void Drag(this IElement element, DragEventArgs eventArgs) => _ = DragAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ondrag</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task DragAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondrag", eventArgs);

	/// <summary>
	/// Raises the <c>@ondragend</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="DragEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void DragEnd(this IElement element)
		=> _ = DragEndAsync(element, new DragEventArgs());

	/// <summary>
	/// Raises the <c>@ondragend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void DragEnd(this IElement element, DragEventArgs eventArgs) => _ = DragEndAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ondragend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task DragEndAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondragend", eventArgs);

	/// <summary>
	/// Raises the <c>@ondragenter</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="DragEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void DragEnter(this IElement element)
		=> _ = DragEnterAsync(element, new DragEventArgs());

	/// <summary>
	/// Raises the <c>@ondragenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void DragEnter(this IElement element, DragEventArgs eventArgs) => _ = DragEnterAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ondragenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task DragEnterAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondragenter", eventArgs);

	/// <summary>
	/// Raises the <c>@ondragleave</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="DragEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void DragLeave(this IElement element)
		=> _ = DragLeaveAsync(element, new DragEventArgs());

	/// <summary>
	/// Raises the <c>@ondragleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void DragLeave(this IElement element, DragEventArgs eventArgs) => _ = DragLeaveAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ondragleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task DragLeaveAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondragleave", eventArgs);

	/// <summary>
	/// Raises the <c>@ondragover</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="DragEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void DragOver(this IElement element)
		=> _ = DragOverAsync(element, new DragEventArgs());

	/// <summary>
	/// Raises the <c>@ondragover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void DragOver(this IElement element, DragEventArgs eventArgs) => _ = DragOverAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ondragover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task DragOverAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondragover", eventArgs);

	/// <summary>
	/// Raises the <c>@ondragstart</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="DragEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void DragStart(this IElement element)
		=> _ = DragStartAsync(element, new DragEventArgs());

	/// <summary>
	/// Raises the <c>@ondragstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void DragStart(this IElement element, DragEventArgs eventArgs) => _ = DragStartAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ondragstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task DragStartAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondragstart", eventArgs);

	/// <summary>
	/// Raises the <c>@ondrop</c> event on <paramref name="element"/>, passing the provided
	/// properties to the event handler via a <see cref="DragEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Drop(this IElement element)
		=> _ = DropAsync(element, new DragEventArgs());

	/// <summary>
	/// Raises the <c>@ondrop</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void Drop(this IElement element, DragEventArgs eventArgs) => _ = DropAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ondrop</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task DropAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondrop", eventArgs);
}