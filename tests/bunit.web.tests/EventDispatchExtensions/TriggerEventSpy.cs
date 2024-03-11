using AngleSharp.Dom;

namespace Bunit;

public class TriggerEventSpy<TEventArgs>
	where TEventArgs : EventArgs, new()
{
	private readonly IRenderedComponent<TriggerTester<TEventArgs>> renderedComponent;
	private readonly string element;
	private TEventArgs? receivedEvent;

	public TEventArgs RaisedEvent => receivedEvent!;

	public TriggerEventSpy(Func<ComponentParameter[], IRenderedComponent<TriggerTester<TEventArgs>>> componentRenderer, string element, string eventName)
	{
		ArgumentNullException.ThrowIfNull(componentRenderer);

		renderedComponent = componentRenderer(new ComponentParameter[]
			{
					(nameof(TriggerTester<TEventArgs>.Element), element),
					(nameof(TriggerTester<TEventArgs>.EventName), eventName),
					(nameof(TriggerTester<TEventArgs>.TriggeredEvent), EventCallback.Factory.Create<TEventArgs>(this, CallbackHandler)),
			});
		this.element = element;
	}

	public void Trigger(Action<IElement> trigger)
	{
		ArgumentNullException.ThrowIfNull(trigger);

		trigger(renderedComponent.Find(element));
	}

	public Task Trigger(Func<IElement, Task> trigger)
	{
		ArgumentNullException.ThrowIfNull(trigger);

		return trigger(renderedComponent.Find(element));
	}

	private void CallbackHandler(TEventArgs args) => receivedEvent = args;
}
