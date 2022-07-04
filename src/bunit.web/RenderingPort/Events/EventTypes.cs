// This file is a port of the EventTypes.ts in https://github.dev/dotnet/aspnetcore
// Version ported: https://github.dev/dotnet/aspnetcore/blob/8a05a97cbe08ee72486713f999856662f96f115c/src/Components/Web.JS/src/Rendering/Events/EventTypes.ts

using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom.Events;

namespace Bunit.RenderingPort.Events;

internal static class EventTypes
{
	private static readonly Dictionary<string, EventTypeOptions> EventTypeRegistry = new();
	private static readonly Dictionary<string, string[]> BrowserEventNamesToAliases = new();

	// Originally placed in EventDelegator.ts
	public static readonly ISet<string> NonBubblingEvents = new HashSet<string>
	{
		"abort",
		"blur",
		"canplay",
		"canplaythrough",
		"change",
		"cuechange",
		"durationchange",
		"emptied",
		"ended",
		"error",
		"focus",
		"load",
		"loadeddata",
		"loadedmetadata",
		"loadend",
		"loadstart",
		"mouseenter",
		"mouseleave",
		"pause",
		"play",
		"playing",
		"progress",
		"ratechange",
		"reset",
		"scroll",
		"seeked",
		"seeking",
		"stalled",
		"submit",
		"suspend",
		"timeupdate",
		"toggle",
		"unload",
		"volumechange",
		"waiting",
		"DOMNodeInsertedIntoDocument",
		"DOMNodeRemovedFromDocument",
	};

	// Originally placed in EventDelegator.ts
	public static readonly IReadOnlyList<string> DisableableEventNames = new string[]
	{
		"click",
		"dblclick",
		"mousedown",
		"mousemove",
		"mouseup",
	};

	// Originally placed in EventDelegator.ts
	public static readonly Dictionary<string, bool> AlwaysPreventDefaultEvents = new()
	{
		{ "submit", true }
	};

	public delegate void EventNameAliasRegisteredCallback(string aliasEventName, string browserEventName);
	public static readonly List<EventNameAliasRegisteredCallback> EventNameAliasRegisteredCallbacks = new();

	static EventTypes()
	{
		RegisterBuiltInEventType(
			new[]
			{
				"contextmenu",
				"click",
				"mouseover",
				"mouseout",
				"mousemove",
				"mousedown",
				"mouseup",
				"mouseleave",
				"mouseenter",
				"dblclick",
			}, new EventTypeOptions(default, e => ParseMouseEvent((MouseEvent)e)));
	}

	public static string GetBrowserEventName(string possibleAliasEventName)
	{
		return EventTypeRegistry.TryGetValue(possibleAliasEventName, out var eventOptions)
			&& eventOptions.BrowserEventName is not null
			? eventOptions.BrowserEventName
			: possibleAliasEventName;
	}

	public static string[] GetEventNameAliases(string eventName)
	{
		return BrowserEventNamesToAliases.TryGetValue(eventName, out var result)
			? result
			: Array.Empty<string>();
	}

	public static EventTypeOptions? GetEventTypeOptions(string eventName)
	{
		return EventTypeRegistry.TryGetValue(eventName, out var result)
			? result
			: null;
	}

	private static void RegisterBuiltInEventType(string[] eventNames, EventTypeOptions options)
	{
		foreach (var eventName in eventNames)
		{
			EventTypeRegistry[eventName] = options;
		}
	}

	public record class EventTypeOptions(string? BrowserEventName, Func<Event, EventArgs> CreateEventArgs);

	private static MouseEventArgs ParseMouseEvent(MouseEvent @event)
	{
		return new MouseEventArgs
		{
			Detail = @event.Detail,
			ScreenX = @event.ScreenX,
			ScreenY = @event.ScreenY,
			ClientX = @event.ClientX,
			ClientY = @event.ClientY,
			// Offset and Page does not exist in AngleSharp's default MouseEvent event.
			//OffsetX = @event.OffsetX,
			//OffsetY = @event.OffsetY,
			//PageX = @event.PageX,
			//PageY = @event.PageY,
			// Movement properties does not exist in C# event
			//MovementX = @event.MovementX,
			//MovementY = @event.MovementY,
			Button = (int)@event.Button,
			Buttons = (int)@event.Buttons,
			CtrlKey = @event.IsCtrlPressed,
			ShiftKey = @event.IsShiftPressed,
			AltKey = @event.IsAltPressed,
			MetaKey = @event.IsMetaPressed,
			Type = @event.Type,
		};
	}
}
