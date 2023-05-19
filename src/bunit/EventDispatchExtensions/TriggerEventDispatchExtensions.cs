using System.Globalization;
using System.Runtime.ExceptionServices;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharpWrappers;
using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// General event dispatch helper extension methods.
/// </summary>
public static class TriggerEventDispatchExtensions
{
	private static readonly HashSet<string> NonBubblingEvents = new(StringComparer.OrdinalIgnoreCase)
	{
		"onabort",
		"onblur",
		"onchange",
		"onerror",
		"onfocus",
		"onload",
		"onloadend",
		"onloadstart",
		"onmouseenter",
		"onmouseleave",
		"onprogress",
		"onreset",
		"onscroll",
		"onsubmit",
		"onunload",
		"ontoggle",
		"ondomnodeinsertedintodocument",
		"ondomnoderemovedfromdocument",
		"oninvalid",
		"onpointerleave",
		"onpointerenter",
		"onselectionchange",
	};
	private static readonly HashSet<string> DisabledEventNames = new(StringComparer.OrdinalIgnoreCase) { "onclick", "ondblclick", "onmousedown", "onmousemove", "onmouseup" };

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
	public static Task TriggerEventAsync(this IElement element, string eventName, EventArgs eventArgs)
	{
		ArgumentNullException.ThrowIfNull(element);
		ArgumentNullException.ThrowIfNull(eventName);

		var renderer = element.GetTestContext()?.Renderer
			?? throw new InvalidOperationException($"Blazor events can only be raised on elements rendered with the Blazor test renderer '{nameof(BunitRenderer)}'.");

		// TriggerEventsAsync will traverse the DOM tree to find
		// all event handlers that needs to be triggered. This is done
		// in the renderes synchronization context to avoid a race condition
		// between the DOM tree being updated and traversed.
		var result = renderer.Dispatcher.InvokeAsync(
			() => TriggerEventsAsync(renderer, element, eventName, eventArgs));

		ThrowIfFailedSynchronously(result);

		return result;
	}

	private static Task TriggerEventsAsync(BunitRenderer renderer, IElement element, string eventName, EventArgs eventArgs)
	{
		var isNonBubblingEvent = NonBubblingEvents.Contains(eventName);
		var unwrappedElement = element.Unwrap();

		return isNonBubblingEvent
			? TriggerNonBubblingEventAsync(renderer, unwrappedElement, eventName, eventArgs)
			: TriggerBubblingEventAsync(renderer, unwrappedElement, eventName, eventArgs);
	}

	private static Task TriggerNonBubblingEventAsync(BunitRenderer renderer, IElement element, string eventName, EventArgs eventArgs)
	{
		var eventAttrName = Htmlizer.ToBlazorAttribute(eventName);

		if (string.Equals(eventName, "onsubmit", StringComparison.Ordinal) && element is not IHtmlFormElement)
		{
			throw new InvalidOperationException("Only forms can have a onsubmit event");
		}

		if (element.TryGetEventId(eventAttrName, out var id))
		{
			return renderer.DispatchEventAsync(id, new EventFieldInfo { FieldValue = eventName }, eventArgs);
		}

		throw new MissingEventHandlerException(element, eventName);
	}

	private static Task TriggerBubblingEventAsync(BunitRenderer renderer, IElement element, string eventName, EventArgs eventArgs)
	{
		var eventTasks = GetDispatchEventTasks(renderer, element, eventName, eventArgs);

		if (eventTasks.Count == 0)
		{
			throw new MissingEventHandlerException(element, eventName);
		}

		return Task.WhenAll(eventTasks);
	}

	private static List<Task> GetDispatchEventTasks(
		BunitRenderer renderer,
		IElement element,
		string eventName,
		EventArgs eventArgs)
	{
		var eventAttrName = Htmlizer.ToBlazorAttribute(eventName);
		var eventStopPropagationAttrName = $"{eventAttrName}:stoppropagation";
		var eventTasks = new List<Task>();

		foreach (var candidate in element.GetParentsAndSelf())
		{
			if (candidate.TryGetEventId(eventAttrName, out var eventId))
			{
				var info = new EventFieldInfo { FieldValue = eventName };
				eventTasks.Add(renderer.DispatchEventAsync(eventId, info, eventArgs, ignoreUnknownEventHandlers: eventTasks.Count > 0));
			}

			// Special case for elements inside form elements
			if (TryGetParentFormElementSpecialCase(candidate, eventName, out var formEventId))
			{
				var info = new EventFieldInfo { FieldValue = "onsubmit" };
				eventTasks.Add(renderer.DispatchEventAsync(formEventId, info, eventArgs, ignoreUnknownEventHandlers: true));
			}

			if (candidate.HasAttribute(eventStopPropagationAttrName) || candidate.EventIsDisabled(eventName))
			{
				break;
			}
		}

		return eventTasks;
	}

	private static bool TryGetParentFormElementSpecialCase(
		IElement element,
		string eventName,
		out ulong eventId)
	{
		eventId = default;

		if (!eventName.Equals("onclick", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}

		var eventAttrName = Htmlizer.ToBlazorAttribute(eventName);
		var preventDefaultAttrName = $"{eventAttrName}:preventdefault";
		if (element.HasAttribute(preventDefaultAttrName))
		{
			return false;
		}

		var form = element switch
		{
			IHtmlInputElement { Type: "submit", Form: not null } input => input.Form,
			IHtmlButtonElement { Type: "submit", Form: not null } button => button.Form,
			_ => null
		};

		return form is not null
			&& form.TryGetEventId(Htmlizer.ToBlazorAttribute("onsubmit"), out eventId);
	}

	private static bool EventIsDisabled(this IElement element, string eventName)
	{
		// We want to replicate the normal DOM event behavior that, for 'interactive' elements
		// with a 'disabled' attribute, certain mouse events are suppressed

		return element switch
		{
			IHtmlButtonElement or IHtmlInputElement or IHtmlTextAreaElement or IHtmlSelectElement => DisabledEventNames.Contains(eventName) && element.IsDisabled(),
			_ => false,
		};
	}

	private static bool TryGetEventId(this IElement element, string blazorEventName, out ulong id)
	{
		var eventId = element.GetAttribute(blazorEventName);
		return ulong.TryParse(eventId, NumberStyles.Integer, CultureInfo.InvariantCulture, out id);
	}

	private static void ThrowIfFailedSynchronously(Task result)
	{
		if (result.IsFaulted && result.Exception is not null)
		{
			if (result.Exception.InnerExceptions.Count == 1)
			{
				ExceptionDispatchInfo.Capture(result.Exception.InnerExceptions[0]).Throw();
			}
			else
			{
				ExceptionDispatchInfo.Capture(result.Exception).Throw();
			}
		}
	}
}
