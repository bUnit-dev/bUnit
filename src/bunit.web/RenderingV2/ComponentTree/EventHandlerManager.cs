namespace Bunit.RenderingV2.ComponentTree;

// TODO: Should EventHandlerManager implement IDisposable and
//       unsubscribe from events when it goes out of scope?
internal sealed class EventHandlerManager
{
	private readonly Dictionary<ulong, Action> eventHandlers = new();

	public void RegisterHandler(ulong eventId, Action unsubscribeAction)
		=> eventHandlers[eventId] = unsubscribeAction;

	public void DisposeHandler(ulong eventId)
	{
		if (eventHandlers.Remove(eventId, out var action))
			action.Invoke();
	}
}
