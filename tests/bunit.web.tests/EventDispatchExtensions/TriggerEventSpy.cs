using AngleSharp.Dom;

namespace Bunit;

public class TriggerEventSpy<TEventArgs>
	where TEventArgs : EventArgs, new()
{
	private readonly Task<IRenderedComponent<TriggerTester<TEventArgs>>> renderedComponent;
	private readonly string element;
	private TEventArgs? receivedEvent;

	public TEventArgs RaisedEvent => receivedEvent!;

	public TriggerEventSpy(Func<ComponentParameter[], Task<IRenderedComponent<TriggerTester<TEventArgs>>>> componentRenderer, string element, string eventName)
	{
		if (componentRenderer is null)
			throw new ArgumentNullException(nameof(componentRenderer));

		renderedComponent = componentRenderer(new ComponentParameter[]
			{
					(nameof(TriggerTester<TEventArgs>.Element), element),
					(nameof(TriggerTester<TEventArgs>.EventName), eventName),
					(nameof(TriggerTester<TEventArgs>.TriggeredEvent), EventCallback.Factory.Create<TEventArgs>(this, CallbackHandler)),
			});
		this.element = element;
	}

	public async Task Trigger(Action<IElement> trigger)
	{
		if (trigger is null)
			throw new ArgumentNullException(nameof(trigger));

		trigger((await renderedComponent).Find(element));
	}

	public async Task Trigger(Func<IElement, Task> trigger)
	{
		if (trigger is null)
			throw new ArgumentNullException(nameof(trigger));

		await trigger((await renderedComponent).Find(element));
	}

	private void CallbackHandler(TEventArgs args) => receivedEvent = args;
}
