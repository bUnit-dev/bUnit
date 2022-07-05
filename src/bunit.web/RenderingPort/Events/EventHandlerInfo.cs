// This file is a port of the EventDelegator.ts in https://github.dev/dotnet/aspnetcore
// Version ported: https://github.dev/dotnet/aspnetcore/blob/8a95ee9b8b1bbd41357cd756375d78cec6936116/src/Components/Web.JS/src/Rendering/Events/EventDelegator.ts

using AngleSharp.Dom;

namespace Bunit.RenderingPort.Events;

internal sealed class EventHandlerInfo
{
	public IElement Element { get; }
	public string EventName { get; }
	public ulong EventHandlerId { get; set; }
	// The component whose tree includes the event handler attribute frame, *not* necessarily the
	// same component that will be re-rendered after the event is handled (since we re-render the
	// component that supplied the delegate, not the one that rendered the event handler frame)
	public int RenderingComponentId { get; }

	public EventHandlerInfo(IElement element, string eventName, ulong eventHandlerId, int renderingComponentId)
	{
		Element = element;
		EventName = eventName;
		EventHandlerId = eventHandlerId;
		RenderingComponentId = renderingComponentId;
	}
}
