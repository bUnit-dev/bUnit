// This file is a port of the EventDelegator.ts in https://github.dev/dotnet/aspnetcore
// Version ported: https://github.dev/dotnet/aspnetcore/blob/8a95ee9b8b1bbd41357cd756375d78cec6936116/src/Components/Web.JS/src/Rendering/Events/EventDelegator.ts

using AngleSharp.Dom;
using static Bunit.RenderingPort.Events.EventTypes;

namespace Bunit.RenderingPort.Events;

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
		info.EventHandlerId = newEventHandlerId;
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
