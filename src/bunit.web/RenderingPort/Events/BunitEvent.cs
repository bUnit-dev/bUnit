using AngleSharp.Dom.Events;

namespace Bunit.RenderingPort;

public class BunitEvent : Event, IEventDispatchResult
{
	private List<Task>? eventDispatchResults;
	private Task? eventDispatchResult;

	internal Task EventDispatchResult
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

	internal EventArgs BlazorEventArgs { get; }

	internal BunitEvent(EventArgs blazorEvent, string type)
		: base(type, bubbles: true, cancelable: true)
	{
		BlazorEventArgs = blazorEvent;
	}

	internal BunitEvent(EventArgs blazorEvent, string type, bool bubbles, bool cancelable)
		: base(type, bubbles, cancelable)
	{
		BlazorEventArgs = blazorEvent;
	}

	internal void AddEventHandlerTask(Task task)
	{
		eventDispatchResults ??= new();
		eventDispatchResults.Add(task);
	}

	bool IEventDispatchResult.DefaultPrevented => IsDefaultPrevented;

	Task IEventDispatchResult.DispatchCompleted => EventDispatchResult;
}
