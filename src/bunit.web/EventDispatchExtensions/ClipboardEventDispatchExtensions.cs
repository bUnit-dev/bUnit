using System;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit
{
	/// <summary>
	/// Clipboard event dispatch helper extension methods.
	/// </summary>
	public static class ClipboardEventDispatchExtensions
	{
		/// <summary>
		/// Raises the <c>@onbeforecopy</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void BeforeCopy(this IElement element) => BeforeCopyAsync(element);

		/// <summary>
		/// Raises the <c>@onbeforecopy</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task BeforeCopyAsync(this IElement element) => element.TriggerEventAsync("onbeforecopy", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onbeforecut</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void BeforeCut(this IElement element) => BeforeCutAsync(element);

		/// <summary>
		/// Raises the <c>@onbeforecut</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task BeforeCutAsync(this IElement element) => element.TriggerEventAsync("onbeforecut", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onbeforepaste</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void BeforePaste(this IElement element) => BeforePasteAsync(element);

		/// <summary>
		/// Raises the <c>@onbeforepaste</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task BeforePasteAsync(this IElement element) => element.TriggerEventAsync("onbeforepaste", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@oncopy</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="ClipboardEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="type">The type of the event.</param>
		public static void Copy(this IElement element, string? type = default) => _ = CopyAsync(element, new ClipboardEventArgs { Type = type! });

		/// <summary>
		/// Raises the <c>@oncopy</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Copy(this IElement element, ClipboardEventArgs eventArgs) => _ = CopyAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@oncopy</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task CopyAsync(this IElement element, ClipboardEventArgs eventArgs) => element.TriggerEventAsync("oncopy", eventArgs);

		/// <summary>
		/// Raises the <c>@oncut</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="ClipboardEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="type">The type of the event.</param>
		public static void Cut(this IElement element, string type = default!) => _ = CutAsync(element, new ClipboardEventArgs { Type = type });

		/// <summary>
		/// Raises the <c>@oncut</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Cut(this IElement element, ClipboardEventArgs eventArgs) => _ = CutAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@oncut</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task CutAsync(this IElement element, ClipboardEventArgs eventArgs) => element.TriggerEventAsync("oncut", eventArgs);

		/// <summary>
		/// Raises the <c>@onpaste</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="ClipboardEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="type">The type of the event.</param>
		public static void Paste(this IElement element, string type = default!) => _ = PasteAsync(element, new ClipboardEventArgs { Type = type });

		/// <summary>
		/// Raises the <c>@onpaste</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Paste(this IElement element, ClipboardEventArgs eventArgs) => _ = PasteAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onpaste</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PasteAsync(this IElement element, ClipboardEventArgs eventArgs) => element.TriggerEventAsync("onpaste", eventArgs);
	}
}
