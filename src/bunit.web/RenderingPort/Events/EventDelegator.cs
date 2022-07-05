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

					// Apply side effects to DOM. AngleSharp does not
					// set of some input elements during events like keyboard events,
					// that would normally otherwise do so.
					// We apply the side effects before the event is dispatched
					// to its event handler, since that event handler might
					// override the value set by the side effect.
					ApplySideEffect(handlerInfo.Element, eventArgs);

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

	static void ApplySideEffect(object node, EventArgs e)
	{
		// This applies side effects to DOM elements.
		// TODO: Get all side effects implemented.
		switch (node)
		{
			case IHtmlInputElement input when e is KeyboardEventArgs kb:
			{
				// This will override current value, since we do not
				// track text cursor positions in the input field.
				input.SetAttribute("value", kb.Key);
				break;
			}
			default:
				break;
		}
	}
}
