using AngleSharp.Dom;
using Bunit.Rendering;

namespace Bunit;

public class TriggerEventSpy<TEventArgs>
	where TEventArgs : EventArgs, new()
{
	private readonly RenderedComponent<TriggerTester<TEventArgs>> renderedComponent;
	private readonly string element;
	private TEventArgs? receivedEvent;

	public TEventArgs RaisedEvent => receivedEvent!;

	public TriggerEventSpy(Func<Action<ComponentParameterCollectionBuilder<TriggerTester<TEventArgs>>>, RenderedComponent<TriggerTester<TEventArgs>>> componentRenderer, string element, string eventName)
	{
		ArgumentNullException.ThrowIfNull(componentRenderer);

		renderedComponent = componentRenderer(ps => ps
			.Add(p => p.Element, element)
			.Add(p => p.EventName, eventName)
			.Add(p => p.TriggeredEvent, EventCallback.Factory.Create<TEventArgs>(this, CallbackHandler)));

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
