using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
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
	public static class TriggerEventDispatchExtensions
	{
		private static readonly HashSet<string> NonBubblingEvents = new(StringComparer.Ordinal) { "onabort", "onblur", "onchange", "onerror", "onfocus", "onload", "onloadend", "onloadstart", "onmouseenter", "onmouseleave", "onprogress", "onreset", "onscroll", "onsubmit", "onunload", "ontoggle", "ondomnodeinsertedintodocument", "ondomnoderemovedfromdocument" };
		private static readonly HashSet<string> DisabledEventNames = new(StringComparer.Ordinal) { "onclick", "ondblclick", "onmousedown", "onmousemove", "onmouseup" };

		/// <summary>
		/// Raises the event <paramref name="eventName"/> on the element <paramref name="element"/>
		/// passing the <paramref name="eventArgs"/> to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventName">The name of the event to raise (using on-form, e.g. <c>onclick</c>).</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler. Use <see cref="EventArgs.Empty"/> to pass an empty <see cref="EventArgs"/>.</param>
		public static void TriggerEvent(this IElement element, string eventName, EventArgs eventArgs)
			=> _ = TriggerEventAsync(element, eventName, eventArgs);

		/// <summary>
		/// Raises the event <paramref name="eventName"/> on the element <paramref name="element"/>
		/// passing the <paramref name="eventArgs"/> to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventName">The name of the event to raise (using on-form, e.g. <c>onclick</c>).</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A <see cref="Task"/> that completes when the render caused by the triggering of the event finishes.</returns>
		[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "HTML events are standardize to lower case and safe in this context.")]
		public static Task TriggerEventAsync(this IElement element, string eventName, EventArgs eventArgs)
		{
			if (element is null)
				throw new ArgumentNullException(nameof(element));
			if (eventName is null)
				throw new ArgumentNullException(nameof(eventName));

			var renderer = element.GetTestContext()?.Renderer
				?? throw new InvalidOperationException($"Blazor events can only be raised on elements rendered with the Blazor test renderer '{nameof(ITestRenderer)}'.");

			var isNonBubblingEvent = NonBubblingEvents.Contains(eventName.ToLowerInvariant());

			if (isNonBubblingEvent)
				return TriggerNonBubblingEventAsync(renderer, element.Unwrap(), eventName, eventArgs);

			return TriggerBubblingEventAsync(renderer, element.Unwrap(), eventName, eventArgs);
		}

		private static Task TriggerBubblingEventAsync(ITestRenderer renderer, IElement element, string eventName, EventArgs eventArgs)
		{
			var eventAttrName = Htmlizer.ToBlazorAttribute(eventName);
			var eventStopPropergationAttrName = $"{eventAttrName}:stoppropagation";
			var eventTasks = new List<Task>();

			foreach (var candidate in element.GetParentsAndSelf())
			{
				if (candidate.TryGetEventId(eventAttrName, out var id))
				{
					try
					{
						var info = new EventFieldInfo() { FieldValue = eventName };
						eventTasks.Add(renderer.DispatchEventAsync(id, info, eventArgs));
					}
					catch (UnknownEventHandlerIdException) when (eventTasks.Count > 0)
					{
						// Capture and ignore NoEventHandlerException for bubbling events
						// if at least one event handler has been triggered without throwing.
					}
				}

				if (candidate.HasAttribute(eventStopPropergationAttrName) || candidate.EventIsDisabled(eventName))
				{
					break;
				}
			}

			if (eventTasks.Count == 0)
				throw new MissingEventHandlerException(element, eventName);

			return Task.WhenAll(eventTasks);
		}

		private static Task TriggerNonBubblingEventAsync(ITestRenderer renderer, IElement element, string eventName, EventArgs eventArgs)
		{
			var eventAttrName = Htmlizer.ToBlazorAttribute(eventName);

			if (element.TryGetEventId(eventAttrName, out var id))
				return renderer.DispatchEventAsync(id, new EventFieldInfo() { FieldValue = eventName }, eventArgs);

			throw new MissingEventHandlerException(element, eventName);
		}

		private static bool EventIsDisabled(this IElement element, string eventName)
		{
			// We want to replicate the normal DOM event behavior that, for 'interactive' elements
			// with a 'disabled' attribute, certain mouse events are suppressed

			switch (element)
			{
				case IHtmlButtonElement:
				case IHtmlInputElement:
				case IHtmlTextAreaElement:
				case IHtmlSelectElement:
					return DisabledEventNames.Contains(eventName) && element.IsDisabled();
				default:
					return false;
			}
		}

		private static bool TryGetEventId(this IElement element, string blazorEventName, out ulong id)
		{
			var eventId = element.GetAttribute(blazorEventName);
			return ulong.TryParse(eventId, NumberStyles.Integer, CultureInfo.InvariantCulture, out id);
		}
	}
}
