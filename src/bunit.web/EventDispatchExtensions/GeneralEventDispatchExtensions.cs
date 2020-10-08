using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharpWrappers;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit
{
	/// <summary>
	/// General event dispatch helper extension methods.
	/// </summary>
	public static class GeneralEventDispatchExtensions
	{
		private static HashSet<string> NonBubblingEvents = new HashSet<string> { "onabort", "onblur", "onchange", "onerror", "onfocus", "onload", "onloadend", "onloadstart", "onmouseenter", "onmouseleave", "onprogress", "onreset", "onscroll", "onsubmit", "onunload", "ontoggle", "ondomnodeinsertedintodocument", "ondomnoderemovedfromdocument" };
		private static HashSet<string> DisabledEventNames = new HashSet<string> { "onclick", "ondblclick", "onmousedown", "onmousemove", "onmouseup" };

		/// <summary>
		/// Raises the event <paramref name="eventName"/> on the element <paramref name="element"/>
		/// passing the <paramref name="eventArgs"/> to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventName">The name of the event to raise (using on-form, e.g. <c>onclick</c>).</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler</param>
		/// <returns></returns>
		public static Task TriggerEventAsync(this IElement element, string eventName, EventArgs eventArgs)
		{
			if (element is null)
				throw new ArgumentNullException(nameof(element));
			if (eventName is null)
				throw new ArgumentNullException(nameof(eventName));
			var renderer = element.Owner.Context.GetService<ITestRenderer>();
			if (renderer is null)
				throw new InvalidOperationException($"Blazor events can only be raised on elements rendered with the Blazor test renderer '{nameof(ITestRenderer)}'.");

			var eventAttrName = Htmlizer.ToBlazorAttribute(eventName);
			var eventStopPropergationAttrName = $"{eventAttrName}:stoppropagation";
			var result = new List<Task>();
			var isNonBubblingEvent = NonBubblingEvents.Contains(eventName.ToLowerInvariant());

			foreach (var candidate in element.GetParentsAndSelf())
			{
				if (candidate.TryGetEventId(eventAttrName, out var id))
					result.Add(renderer.DispatchEventAsync(id, new EventFieldInfo() { FieldValue = eventName }, eventArgs));

				if (isNonBubblingEvent || candidate.HasAttribute(eventStopPropergationAttrName) || candidate.EventIsDisabled(eventName))
				{
					break;
				}
			}

			if (result.Count == 0)
				throw new MissingEventHandlerException(element, eventName);

			return Task.WhenAll(result);
		}

		private static bool EventIsDisabled(this IElement element, string eventName)
		{
			// We want to replicate the normal DOM event behavior that, for 'interactive' elements
			// with a 'disabled' attribute, certain mouse events are suppressed

			var elm = (element as ElementWrapper)?.WrappedElement ?? element;
			switch (elm)
			{
				case IHtmlButtonElement:
				case IHtmlInputElement:
				case IHtmlTextAreaElement:
				case IHtmlSelectElement:
					return DisabledEventNames.Contains(eventName) && elm.IsDisabled();
				default:
					return false;
			}
		}

		private static bool TryGetEventId(this IElement element, string blazorEventName, out ulong id)
		{
			var eventId = element.GetAttribute(blazorEventName);
			return ulong.TryParse(eventId, NumberStyles.Integer, CultureInfo.InvariantCulture, out id);
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
		private static Task ActivateAsync(this IElement element) => TriggerEventAsync(element, "onactivate", EventArgs.Empty);

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
		private static Task BeforeActivateAsync(this IElement element) => TriggerEventAsync(element, "onbeforeactivate", EventArgs.Empty);

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
		private static Task BeforeDeactivateAsync(this IElement element) => TriggerEventAsync(element, "onbeforedeactivate", EventArgs.Empty);

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
		private static Task DeactivateAsync(this IElement element) => TriggerEventAsync(element, "ondeactivate", EventArgs.Empty);

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
		private static Task EndedAsync(this IElement element) => TriggerEventAsync(element, "onended", EventArgs.Empty);

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
		private static Task FullscreenChangeAsync(this IElement element) => TriggerEventAsync(element, "onfullscreenchange", EventArgs.Empty);

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
		private static Task FullscreenErrorAsync(this IElement element) => TriggerEventAsync(element, "onfullscreenerror", EventArgs.Empty);

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
		private static Task LoadedDataAsync(this IElement element) => TriggerEventAsync(element, "onloadeddata", EventArgs.Empty);

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
		private static Task LoadedMetadataAsync(this IElement element) => TriggerEventAsync(element, "onloadedmetadata", EventArgs.Empty);

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
		private static Task PointerlockChangeAsync(this IElement element) => TriggerEventAsync(element, "onpointerlockchange", EventArgs.Empty);

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
		private static Task PointerlockErrorAsync(this IElement element) => TriggerEventAsync(element, "onpointerlockerror", EventArgs.Empty);

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
		private static Task ReadystateChangeAsync(this IElement element) => TriggerEventAsync(element, "onreadystatechange", EventArgs.Empty);

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
		private static Task ScrollAsync(this IElement element) => TriggerEventAsync(element, "onscroll", EventArgs.Empty);
	}
}
