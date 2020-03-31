using AngleSharp.Dom;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;

namespace Bunit
{
    /// <summary>
    /// General event dispatch helper extension methods.
    /// </summary>
    public static class GeneralEventDispatchExtensions
    {
        /// <summary>
        /// Raises the event <paramref name="eventName"/> on the element <paramref name="element"/>
        /// passing the <paramref name="eventArgs"/> to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <param name="eventName">The name of the event to raise (using on-form, e.g. <c>onclick</c>).</param>
        /// <param name="eventArgs">The event arguments to pass to the event handler</param>
        /// <returns></returns>
        [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
        public static Task TriggerEventAsync(this IElement element, string eventName, EventArgs eventArgs)
        {
            if (element is null) throw new ArgumentNullException(nameof(element));

            var eventHandlerIdString = element.GetAttribute(Htmlizer.ToBlazorAttribute(eventName));

            if (string.IsNullOrEmpty(eventHandlerIdString))
                throw new MissingEventHandlerException(element, eventName);

            var eventHandlerId = ulong.Parse(eventHandlerIdString, CultureInfo.InvariantCulture);

            var renderer = element.Owner.Context.GetService<TestRenderer>();
            if (renderer is null)
                throw new InvalidOperationException($"Blazor events can only be raised on elements rendered with the Blazor test renderer '{nameof(TestRenderer)}'.");

            return renderer.DispatchEventAsync(eventHandlerId, new EventFieldInfo() { FieldValue = eventName }, eventArgs);
        }

        /// <summary>
        /// Raises the <c>@onactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void Activate(this IElement element) => ActivateAsync(element);

        /// <summary>
        /// Raises the <c>@onactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task ActivateAsync(this IElement element) => TriggerEventAsync(element, "onactivate", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@onbeforeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void BeforeActivate(this IElement element) => BeforeActivateAsync(element);

        /// <summary>
        /// Raises the <c>@onbeforeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task BeforeActivateAsync(this IElement element) => TriggerEventAsync(element, "onbeforeactivate", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@onbeforedeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void BeforeDeactivate(this IElement element) => BeforeDeactivateAsync(element);

        /// <summary>
        /// Raises the <c>@onbeforedeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task BeforeDeactivateAsync(this IElement element) => TriggerEventAsync(element, "onbeforedeactivate", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@ondeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void Deactivate(this IElement element) => DeactivateAsync(element);

        /// <summary>
        /// Raises the <c>@ondeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task DeactivateAsync(this IElement element) => TriggerEventAsync(element, "ondeactivate", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@onended</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void Ended(this IElement element) => EndedAsync(element);

        /// <summary>
        /// Raises the <c>@onended</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task EndedAsync(this IElement element) => TriggerEventAsync(element, "onended", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@onfullscreenchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void FullscreenChange(this IElement element) => FullscreenChangeAsync(element);

        /// <summary>
        /// Raises the <c>@onfullscreenchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task FullscreenChangeAsync(this IElement element) => TriggerEventAsync(element, "onfullscreenchange", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@onfullscreenerror</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void FullscreenError(this IElement element) => FullscreenErrorAsync(element);

        /// <summary>
        /// Raises the <c>@onfullscreenerror</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task FullscreenErrorAsync(this IElement element) => TriggerEventAsync(element, "onfullscreenerror", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@onloadeddata</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void LoadedData(this IElement element) => LoadedDataAsync(element);

        /// <summary>
        /// Raises the <c>@onloadeddata</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task LoadedDataAsync(this IElement element) => TriggerEventAsync(element, "onloadeddata", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@onloadedmetadata</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void LoadedMetadata(this IElement element) => LoadedMetadataAsync(element);

        /// <summary>
        /// Raises the <c>@onloadedmetadata</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task LoadedMetadataAsync(this IElement element) => TriggerEventAsync(element, "onloadedmetadata", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@onpointerlockchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void PointerlockChange(this IElement element) => PointerlockChangeAsync(element);

        /// <summary>
        /// Raises the <c>@onpointerlockchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task PointerlockChangeAsync(this IElement element) => TriggerEventAsync(element, "onpointerlockchange", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@onpointerlockerror</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void PointerlockError(this IElement element) => PointerlockErrorAsync(element);

        /// <summary>
        /// Raises the <c>@onpointerlockerror</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task PointerlockErrorAsync(this IElement element) => TriggerEventAsync(element, "onpointerlockerror", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@onreadystatechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void ReadystateChange(this IElement element) => ReadystateChangeAsync(element);

        /// <summary>
        /// Raises the <c>@onreadystatechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task ReadystateChangeAsync(this IElement element) => TriggerEventAsync(element, "onreadystatechange", EventArgs.Empty);

        /// <summary>
        /// Raises the <c>@onscroll</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        public static void Scroll(this IElement element) => ScrollAsync(element);

        /// <summary>
        /// Raises the <c>@onscroll</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>) 
        /// to the event handler.
        /// </summary>
        /// <param name="element">The element to raise the event on.</param>
        /// <returns>A task that completes when the event handler is done.</returns>
        public static Task ScrollAsync(this IElement element) => TriggerEventAsync(element, "onscroll", EventArgs.Empty);
    }
}
