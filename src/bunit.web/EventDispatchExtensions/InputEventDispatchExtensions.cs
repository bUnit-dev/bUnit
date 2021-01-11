using System;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// Input event dispatch helper extension methods.
	/// </summary>
	public static class InputEventDispatchExtensions
	{
		/// <summary>
		/// Raises the <c>@onchange</c> event on <paramref name="element"/>,  passing the provided
		/// properties to the event handler via a <see cref="ChangeEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="value">The new value.</param>
		public static void Change(this IElement element, object value)
			=> _ = ChangeAsync(element, new ChangeEventArgs { Value = value });

		/// <summary>
		/// Raises the <c>@onchange</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Change(this IElement element, ChangeEventArgs eventArgs)
			=> _ = ChangeAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onchange</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task ChangeAsync(this IElement element, ChangeEventArgs eventArgs) => element.TriggerEventAsync("onchange", eventArgs);

		/// <summary>
		/// Raises the <c>@oninput</c> event on <paramref name="element"/>,  passing the provided
		/// properties to the event handler via a <see cref="ChangeEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="value">The new value.</param>
		public static void Input(this IElement element, string value)
			=> _ = InputAsync(element, new ChangeEventArgs { Value = value });

		/// <summary>
		/// Raises the <c>@oninput</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Input(this IElement element, ChangeEventArgs eventArgs)
			=> _ = InputAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@oninput</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Input(this IElement element) => _ = InputAsync(element);

		/// <summary>
		/// Raises the <c>@oninput</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task InputAsync(this IElement element) => element.TriggerEventAsync("oninput", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@oninput</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task InputAsync(this IElement element, ChangeEventArgs eventArgs) => element.TriggerEventAsync("oninput", eventArgs);

		/// <summary>
		/// Raises the <c>@oninvalid</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Invalid(this IElement element) => _ = InvalidAsync(element);

		/// <summary>
		/// Raises the <c>@oninvalid</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task InvalidAsync(this IElement element) => element.TriggerEventAsync("oninvalid", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onreset</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Reset(this IElement element) => _ = ResetAsync(element);

		/// <summary>
		/// Raises the <c>@onreset</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task ResetAsync(this IElement element) => element.TriggerEventAsync("onreset", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onselect</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Select(this IElement element) => _ = SelectAsync(element);

		/// <summary>
		/// Raises the <c>@onselect</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task SelectAsync(this IElement element) => element.TriggerEventAsync("onselect", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onselectstart</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void SelectStart(this IElement element) => _ = SelectStartAsync(element);

		/// <summary>
		/// Raises the <c>@onselectstart</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task SelectStartAsync(this IElement element) => element.TriggerEventAsync("onselectstart", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onselectionchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void SelectionChange(this IElement element) => _ = SelectionChangeAsync(element);

		/// <summary>
		/// Raises the <c>@onselectionchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task SelectionChangeAsync(this IElement element) => element.TriggerEventAsync("onselectionchange", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onsubmit</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Submit(this IElement element) => _ = SubmitAsync(element);

		/// <summary>
		/// Raises the <c>@onsubmit</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task SubmitAsync(this IElement element) => element.TriggerEventAsync("onsubmit", EventArgs.Empty);
	}
}
