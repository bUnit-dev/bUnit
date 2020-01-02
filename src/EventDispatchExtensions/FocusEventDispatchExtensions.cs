using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing.EventDispatchExtensions
{
    /// <summary>
    /// Focus event dispatch helper extension methods.
    /// </summary>
    public static class FocusEventDispatchExtensions
    {
        /// <summary>
        /// Raises the <c>@onfocus</c> event on <paramref name="element"/>, passing the provided
        /// properties to the event handler via a <see cref="FocusEventArgs"/> object
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="type">The type of the event.</param>
        public static void Focus(this IElement element, string? type = default) => _ = FocusAsync(element, new FocusEventArgs { Type = type });

        /// <summary>
        /// Raises the <c>@onfocus</c> event on <paramref name="element"/>, passing the provided
        /// event arguments to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="eventArgs">The event arguments to passs the event handler.</param>
        public static void Focus(this IElement element, FocusEventArgs eventArgs) => _ = FocusAsync(element, eventArgs);

        /// <summary>
        /// Raises the <c>@onfocus</c> event on <paramref name="element"/>, passing the provided
        /// event arguments to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="eventArgs">The event arguments to passs the event handler.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task FocusAsync(this IElement element, FocusEventArgs eventArgs) => element.TriggerEventAsync("onfocus", eventArgs);

        /// <summary>
        /// Raises the <c>@onblur</c> event on <paramref name="element"/>, passing the provided
        /// properties to the event handler via a <see cref="FocusEventArgs"/> object
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="type">The type of the event.</param>
        public static void Blur(this IElement element, string? type = default) => _ = BlurAsync(element, new FocusEventArgs { Type = type });

        /// <summary>
        /// Raises the <c>@onblur</c> event on <paramref name="element"/>, passing the provided
        /// event arguments to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="eventArgs">The event arguments to passs the event handler.</param>
        public static void Blur(this IElement element, FocusEventArgs eventArgs) => _ = BlurAsync(element, eventArgs);

        /// <summary>
        /// Raises the <c>@onblur</c> event on <paramref name="element"/>, passing the provided
        /// event arguments to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="eventArgs">The event arguments to passs the event handler.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task BlurAsync(this IElement element, FocusEventArgs eventArgs) => element.TriggerEventAsync("onblur", eventArgs);

        /// <summary>
        /// Raises the <c>@onfocusin</c> event on <paramref name="element"/>, passing the provided
        /// properties to the event handler via a <see cref="FocusEventArgs"/> object
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="type">The type of the event.</param>
        public static void FocusIn(this IElement element, string? type = default) => _ = FocusInAsync(element, new FocusEventArgs { Type = type });

        /// <summary>
        /// Raises the <c>@onfocusin</c> event on <paramref name="element"/>, passing the provided
        /// event arguments to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="eventArgs">The event arguments to passs the event handler.</param>
        public static void FocusIn(this IElement element, FocusEventArgs eventArgs) => _ = FocusInAsync(element, eventArgs);

        /// <summary>
        /// Raises the <c>@onfocusin</c> event on <paramref name="element"/>, passing the provided
        /// event arguments to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="eventArgs">The event arguments to passs the event handler.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task FocusInAsync(this IElement element, FocusEventArgs eventArgs) => element.TriggerEventAsync("onfocusin", eventArgs);

        /// <summary>
        /// Raises the <c>@onfocusout</c> event on <paramref name="element"/>, passing the provided
        /// properties to the event handler via a <see cref="FocusEventArgs"/> object
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="type">The type of the event.</param>
        public static void FocusOut(this IElement element, string? type = default) => _ = FocusOutAsync(element, new FocusEventArgs { Type = type });

        /// <summary>
        /// Raises the <c>@onfocusout</c> event on <paramref name="element"/>, passing the provided
        /// event arguments to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="eventArgs">The event arguments to passs the event handler.</param>
        public static void FocusOut(this IElement element, FocusEventArgs eventArgs) => _ = FocusOutAsync(element, eventArgs);

        /// <summary>
        /// Raises the <c>@onfocusout</c> event on <paramref name="element"/>, passing the provided
        /// event arguments to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="eventArgs">The event arguments to passs the event handler.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task FocusOutAsync(this IElement element, FocusEventArgs eventArgs) => element.TriggerEventAsync("onfocusout", eventArgs);
    }
}
