using AngleSharp.Dom;

namespace Bunit.RenderingPort;

internal class EventDelegator
{
	internal void SetListener(IElement toDomElement, string eventName, ulong eventHandlerId, int componentId) => throw new NotImplementedException();
	internal void SetPreventDefault(IElement element, string eventName, bool v) => throw new NotImplementedException();
	internal void SetStopPropagation(IElement element, string eventName, bool v) => throw new NotImplementedException();
}
