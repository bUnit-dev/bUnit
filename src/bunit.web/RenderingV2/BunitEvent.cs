using AngleSharp.Dom.Events;

namespace Bunit.RenderingV2;

public class BunitEvent : Event
{
	private List<Task>? eventDispatchResults;
	private Task? eventDispatchResult;

	public Task EventDispatchResult
	{
		get
		{
			if (eventDispatchResult is null)
			{
				eventDispatchResult = eventDispatchResults is null
					? Task.CompletedTask
					: Task.WhenAll(eventDispatchResults);
			}

			return eventDispatchResult;
		}
	}

	public EventArgs BlazorEventArgs { get; }

	public BunitEvent(EventArgs blazorEvent)
	{
		BlazorEventArgs = blazorEvent;
	}

	public BunitEvent(EventArgs blazorEvent, string type, bool bubbles = false, bool cancelable = false)
		: base(type, bubbles, cancelable)
	{
		BlazorEventArgs = blazorEvent;
	}

	internal void AddEventHandlerTask(Task task)
	{
		eventDispatchResults ??= new();
		eventDispatchResults.Add(task);
	}
}
