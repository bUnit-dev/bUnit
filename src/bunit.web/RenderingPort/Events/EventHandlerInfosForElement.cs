// This file is a port of the EventDelegator.ts in https://github.dev/dotnet/aspnetcore
// Version ported: https://github.dev/dotnet/aspnetcore/blob/8a95ee9b8b1bbd41357cd756375d78cec6936116/src/Components/Web.JS/src/Rendering/Events/EventDelegator.ts

namespace Bunit.RenderingPort.Events;

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

#pragma warning disable S1125 // Boolean literals should not be redundant
		return preventDefaultFlags.ContainsKey(eventName)
			? preventDefaultFlags[eventName]
			: false;
#pragma warning restore S1125 // Boolean literals should not be redundant
	}

	internal bool StopPropagation(string eventName, bool? setValue = null)
	{
		if (setValue is not null)
		{
			stopPropagationFlags[eventName] = setValue.Value;
		}

#pragma warning disable S1125 // Boolean literals should not be redundant
		return stopPropagationFlags.ContainsKey(eventName)
			? stopPropagationFlags[eventName]
			: false;
#pragma warning restore S1125 // Boolean literals should not be redundant
	}
}
