// This file is a port of the EventDelegator.ts in https://github.dev/dotnet/aspnetcore
// Version ported: https://github.dev/dotnet/aspnetcore/blob/8a95ee9b8b1bbd41357cd756375d78cec6936116/src/Components/Web.JS/src/Rendering/Events/EventDelegator.ts

using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Dom.Events;
using static Bunit.RenderingPort.Events.EventTypes;

namespace Bunit.RenderingPort.Events;

// Responsible for adding/removing the eventInfo on an expando property on DOM elements, and
// calling an EventInfoStore that deals with registering/unregistering the underlying delegated
// event listeners as required (and also maps actual events back to the given callback).
internal class EventDelegator
{
	private static int nextEventDelegatorId = 0;
	private readonly BunitRenderer renderer;
	private readonly string eventsCollectionKey;

	private readonly List<Action<MouseEvent>> afterClickCallbacks = new();

	private readonly EventInfoStore eventInfoStore;

	public EventDelegator(IDocument document, BunitRenderer renderer)
	{
		this.renderer = renderer;
		var eventDelegatorId = ++nextEventDelegatorId;
		eventsCollectionKey = $"_blazorEvents_{eventDelegatorId}";
		eventInfoStore = new EventInfoStore(OnGlobalEvent, document);
	}

	internal void SetListener(IElement element, string eventName, ulong eventHandlerId, int renderingComponentId)
	{
		var infoForElement = GetEventHandlerInfosForElement(element, true)!;
		var existingHandler = infoForElement.GetHandler(eventName);

		if (existingHandler is not null)
		{
			// We can cheaply update the info on the existing object and don't need any other housekeeping
			// Note that this also takes care of updating the eventHandlerId on the existing handler object
			eventInfoStore.Update(existingHandler.EventHandlerId, eventHandlerId);
		}
		else
		{
			// Go through the whole flow which might involve registering a new global handler
			var newInfo = new EventHandlerInfo(element, eventName, eventHandlerId, renderingComponentId);
			eventInfoStore.Add(newInfo);
			infoForElement.SetHandler(eventName, newInfo);
		}
	}

	public EventHandlerInfo GetHandler(ulong eventHandlerId)
		=> eventInfoStore.Get(eventHandlerId);

	internal void RemoveListener(ulong eventHandlerId)
	{
		// This method gets called whenever the .NET-side code reports that a certain event handler
		// has been disposed. However we will already have disposed the info about that handler if
		// the eventHandlerId for the (element,eventName) pair was replaced during diff application.
		var info = eventInfoStore.Remove(eventHandlerId);
		if (info is not null)
		{
			// Looks like this event handler wasn't already disposed
			// Remove the associated data from the DOM element
			var element = info.Element;
			var elementEventInfos = GetEventHandlerInfosForElement(element, false);
			if (elementEventInfos is not null)
			{
				elementEventInfos.RemoveHandler(info.EventName);
			}
		}
	}

	private EventHandlerInfosForElement? GetEventHandlerInfosForElement(IElement element, bool createIfNeeded)
	{
		var logicalElement = element.ToLogicalElement();
		if (logicalElement.EventsCollections.TryGetValue(eventsCollectionKey, out var result))
		{
			return result;
		}
		else if (createIfNeeded)
		{
			logicalElement.EventsCollections.Add(eventsCollectionKey, new EventHandlerInfosForElement());
			return logicalElement.EventsCollections[eventsCollectionKey];
		}
		else
		{
			return null;
		}
	}

	internal void SetStopPropagation(IElement element, string eventName, bool value)
	{
		var infoForElement = GetEventHandlerInfosForElement(element, true)!;
		infoForElement.StopPropagation(eventName, value);
	}

	internal void SetPreventDefault(IElement element, string eventName, bool value)
	{
		var infoForElement = GetEventHandlerInfosForElement(element, true)!;
		infoForElement.PreventDefault(eventName, value);
	}

	private void OnGlobalEvent(object _, Event evt)
	{
		if (evt.OriginalTarget is not IElement)
		{
			return;
		}

		// Always dispatch to any listeners for the original underlying browser event name
		DispatchGlobalEventToAllElements(evt.Type, evt);

		// If this event name has aliases, dispatch for those listeners too
		var eventNameAliases = GetEventNameAliases(evt.Type);

		foreach (var alias in eventNameAliases)
		{
			DispatchGlobalEventToAllElements(alias, evt);
		}

		// Special case for navigation interception
		if (evt.Type == "click" && afterClickCallbacks.Count > 0)
		{
			MouseEvent? mouseEvent = evt as MouseEvent;

			if (mouseEvent is null
				&& evt is BunitEvent bunitEvent
				&& bunitEvent.BlazorEventArgs is MouseEventArgs args)
			{
				mouseEvent = new MouseEvent(
					args.Type,
					bubbles: evt.IsBubbling,
					cancelable: evt.IsCancelable,
					detail: (int)args.Detail,
					screenX: (int)args.ScreenX,
					screenY: (int)args.ScreenY,
					clientX: (int)args.ClientX,
					clientY: (int)args.ClientY,
					ctrlKey: args.CtrlKey,
					altKey: args.AltKey,
					shiftKey: args.ShiftKey,
					metaKey: args.MetaKey,
					button: (MouseButton)args.Button,
					relatedTarget: evt.OriginalTarget);
			}

			if (mouseEvent is null)
			{
				throw new InvalidOperationException("An event with type 'click' was received, but it was a MouseEvent");
			}

			foreach (var callback in afterClickCallbacks)
			{
				callback(mouseEvent);
			}
		}
	}

	private void DispatchGlobalEventToAllElements(string eventName, Event browserEvent)
	{
		// Note that 'eventName' can be an alias. For example, eventName may be 'click.special'
		// while browserEvent.type may be 'click'.

		// Use the event's 'path' rather than the chain of parent nodes, since the path gives
		// visibility into shadow roots.
		//const path = browserEvent.composedPath();
		var path = EmulateComposedPath(browserEvent);

		BunitEvent? bunitEvent = browserEvent as BunitEvent;
		EventArgs? eventArgs = bunitEvent?.BlazorEventArgs;
		//var eventArgsIsPopulated = false;
		var eventIsNonBubbling = NonBubblingEvents.Contains(eventName);
		var stopPropagationWasRequested = false;
		foreach (var candidateEventTarget in path)
		{
			var candidateElement = candidateEventTarget;
			var handlerInfos = GetEventHandlerInfosForElement(candidateElement, false);
			if (handlerInfos is not null)
			{
				var handlerInfo = handlerInfos.GetHandler(eventName);

				if (handlerInfo is not null && !EventIsDisabledOnElement(candidateElement, browserEvent.Type))
				{
					// We are going to raise an event for this element, so prepare info needed by the .NET code
					if (eventArgs is null /*!eventArgsIsPopulated*/)
					{
						var eventOptionsIfRegistered = GetEventTypeOptions(eventName);
						// For back-compat, if there's no registered createEventArgs, we supply empty event args (not null).
						// But if there is a registered createEventArgs, it can supply anything (including null).
						eventArgs = eventOptionsIfRegistered?.CreateEventArgs is not null
							? eventOptionsIfRegistered.CreateEventArgs(browserEvent)
							: EventArgs.Empty;
						//eventArgsIsPopulated = true;
					}

					// For certain built-in events, having any .NET handler implicitly means we will prevent
					// the browser's default behavior. This has to be based on the original browser event type name,
					// not any alias (e.g., if you create a custom 'submit' variant, it should still preventDefault).
					if (AlwaysPreventDefaultEvents.TryGetValue(browserEvent.Type, out var alwaysPreventDefaultEvents) && alwaysPreventDefaultEvents)
					{
						browserEvent.Cancel(); // preventDefault
					}

					// Dispatch directly to the native C# renderer and store
					// the dispatch task in the browser event, if it is a
					// BunitEvent. This allows the user to await async event handlers.
					var dispatchTask = renderer.DispatchEventAsync(
						handlerInfo.EventHandlerId,
						FromEvent(handlerInfo.RenderingComponentId, browserEvent),
						eventArgs);

					bunitEvent?.AddEventHandlerTask(dispatchTask);
				}

				if (handlerInfos.StopPropagation(eventName))
				{
					stopPropagationWasRequested = true;
				}

				if (handlerInfos.PreventDefault(eventName))
				{
					browserEvent.Cancel(); // preventDefault
				}
			}

			if (eventIsNonBubbling || stopPropagationWasRequested)
			{
				break;
			}
		}
	}

	// Originally placed in EventFieldInfo.ts. Type already part of C# Blazor SDK. Helpers moved here.
	private static EventFieldInfo? FromEvent(int componentId, Event @event)
	{
		var node = @event.OriginalTarget;
		if (node is IElement element)
		{
			var fieldData = GetFormFieldData(element);
			if (fieldData is not null)
			{
				return new EventFieldInfo { ComponentId = componentId, FieldValue = fieldData };
			}
		}

		// This event isn't happening on a form field that we can reverse-map back to some incoming attribute
		return null;

		static object? GetFormFieldData(IElement elem)
		{
			// The logic in here should be the inverse of the logic in BrowserRenderer's TryApplySpecialProperty.
			// That is, we're doing the reverse mapping, starting from an HTML property and reconstructing which
			// "special" attribute would have been mapped to that property.
			if (elem is IHtmlInputElement inputElement)
			{
				return inputElement.Type.ToLowerInvariant() == "checkbox"
					? inputElement.IsChecked
					: inputElement.Value;
			}

			if (elem is IHtmlSelectElement selectElm)
			{
				return selectElm.Value;
			}

			if (elem is IHtmlTextAreaElement textArea)
			{
				return textArea.Value;
			}

			return null;

		}
	}

	// It seems AngleSharp does not support Event.composedPath().
	// For now, just walk up the DOM tree, even if the comment in
	// DispatchGlobalEventToAllElements seems to indicate that is not
	// what we want. 
	private static IEnumerable<IElement> EmulateComposedPath(Event browserEvent)
	{
		var target = browserEvent.OriginalTarget as IElement;

		while (target is not null)
		{
			yield return target;
			target = target.ParentElement;
		}
	}

	private bool EventIsDisabledOnElement(IElement element, string rawBrowserEventName)
	{
		// We want to replicate the normal DOM event behavior that, for 'interactive' elements
		// with a 'disabled' attribute, certain mouse events are suppressed
		return (element is IHtmlButtonElement || element is IHtmlInputElement || element is IHtmlTextAreaElement || element is IHtmlSelectElement)
			&& DisableableEventNames.Contains(rawBrowserEventName)
			&& element.IsDisabled();
	}
}


internal sealed class EventHandlerInfosForElement
{
	// Although we *could* track multiple event handlers per (element, eventName) pair
	// (since they have distinct eventHandlerId values), there's no point doing so because
	// our programming model is that you declare event handlers as attributes. An element
	// can only have one attribute with a given name, hence only one event handler with
	// that name at any one time.
	// So to keep things simple, only track one EventHandlerInfo per (element, eventName)
	private readonly Dictionary<string, EventHandlerInfo> handlers = new();
	private readonly Dictionary<string, bool> preventDefaultFlags = new();
	private readonly Dictionary<string, bool> stopPropagationFlags = new();

	public EventHandlerInfosForElement()
	{
	}

	internal EventHandlerInfo? GetHandler(string eventName)
	{
		return handlers.TryGetValue(eventName, out var result)
			? result
			: default;
	}

	internal void SetHandler(string eventName, EventHandlerInfo handler)
	{
		handlers.Add(eventName, handler);
	}

	internal void RemoveHandler(string eventName)
	{
		handlers.Remove(eventName);
	}

	internal bool PreventDefault(string eventName, bool? setValue = null)
	{
		if (setValue is not null)
		{
			preventDefaultFlags[eventName] = setValue.Value;
		}

		return preventDefaultFlags.ContainsKey(eventName)
			? preventDefaultFlags[eventName]
			: false;
	}

	internal bool StopPropagation(string eventName, bool? setValue = null)
	{
		if (setValue is not null)
		{
			stopPropagationFlags[eventName] = setValue.Value;
		}

		return stopPropagationFlags.ContainsKey(eventName)
			? stopPropagationFlags[eventName]
			: false;
	}
}

internal sealed record class EventHandlerInfo(
	IElement Element,
	string EventName,
	ulong EventHandlerId,
	// The component whose tree includes the event handler attribute frame, *not* necessarily the
	// same component that will be re-rendered after the event is handled (since we re-render the
	// component that supplied the delegate, not the one that rendered the event handler frame)
	int RenderingComponentId);

internal class EventInfoStore
{
	private readonly Dictionary<ulong, EventHandlerInfo> infosByEventHandlerId = new();
	private readonly Dictionary<string, int> countByEventName = new();
	private readonly DomEventHandler globalListener;
	private readonly IDocument document;

	public EventInfoStore(DomEventHandler globalListener, IDocument document)
	{
		this.globalListener = globalListener;
		this.document = document;
		EventNameAliasRegisteredCallbacks.Add(HandleEventNameAliasAdded);
	}

	internal void Add(EventHandlerInfo info)
	{
		if (infosByEventHandlerId.ContainsKey(info.EventHandlerId))
		{
			// Should never happen, but we want to know if it does
			throw new ArgumentException($"Event {info.EventHandlerId} is already tracked");
		}

		infosByEventHandlerId.Add(info.EventHandlerId, info);

		AddGlobalListener(info.EventName);
	}

	internal EventHandlerInfo Get(ulong eventHandlerId)
		=> infosByEventHandlerId[eventHandlerId];

	internal void AddGlobalListener(string eventName)
	{
		// If this event name is an alias, update the global listener for the corresponding browser event
		eventName = GetBrowserEventName(eventName);

		if (countByEventName.ContainsKey(eventName))
		{
			countByEventName[eventName] += 1;
		}
		else
		{
			countByEventName[eventName] = 1;

			// To make delegation work with non-bubbling events, register a 'capture' listener.
			// We preserve the non-bubbling behavior by only dispatching such events to the targeted element.
			var useCapture = NonBubblingEvents.Contains(eventName);
			document.AddEventListener(eventName, globalListener, useCapture);
		}
	}

	internal void Update(ulong oldEventHandlerId, ulong newEventHandlerId)
	{
		if (infosByEventHandlerId.ContainsKey(newEventHandlerId))
		{
			// Should never happen, but we want to know if it does
			throw new ArgumentException($"Event {newEventHandlerId} is already tracked");
		}

		// Since we're just updating the event handler ID, there's no need to update the global counts
		var info = infosByEventHandlerId[oldEventHandlerId];
		infosByEventHandlerId.Remove(oldEventHandlerId);
		info = info with { EventHandlerId = newEventHandlerId };
		infosByEventHandlerId[newEventHandlerId] = info;
	}

	internal EventHandlerInfo? Remove(ulong eventHandlerId)
	{
		if (infosByEventHandlerId.TryGetValue(eventHandlerId, out var info))
		{
			infosByEventHandlerId.Remove(eventHandlerId);

			// If this event name is an alias, update the global listener for the corresponding browser event
			var eventName = GetBrowserEventName(info.EventName);

			if (countByEventName.TryGetValue(eventName, out var count))
			{
				if (--count == 0)
				{
					countByEventName.Remove(eventName);
					document.RemoveEventListener(eventName, globalListener);
				}
				else
				{
					countByEventName[eventName] = count;
				}
			}
		}

		return info;
	}

	private void HandleEventNameAliasAdded(string aliasEventName, string browserEventName)
	{
		// If an event name alias gets registered later, we need to update the global listener
		// registrations to match. This makes it equivalent to the alias having been registered
		// before the elements with event handlers got rendered.
		if (countByEventName.TryGetValue(aliasEventName, out var countByAliasEventName))
		{
			// Delete old
			countByEventName.Remove(aliasEventName);
			document.RemoveEventListener(aliasEventName, globalListener);

			// Ensure corresponding count is added to new
			AddGlobalListener(browserEventName);
			countByEventName[browserEventName] += countByAliasEventName - 1;
		}
	}

}
