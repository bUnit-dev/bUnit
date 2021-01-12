using System;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	public class TriggerEventSpy<TEventArgs>
	    where TEventArgs : EventArgs, new()
	{
		private readonly IRenderedComponent<TriggerTester<TEventArgs>> renderedComponent;
		private readonly string element;
		private TEventArgs? receivedEvent;

		public TEventArgs RaisedEvent => receivedEvent!;

		public TriggerEventSpy(Func<ComponentParameter[], IRenderedComponent<TriggerTester<TEventArgs>>> componentRenderer, string element, string eventName)
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

		public void Trigger(Action<IElement> trigger)
		{
			if (trigger is null)
				throw new ArgumentNullException(nameof(trigger));

			trigger(renderedComponent.Find(element));
		}

		public Task Trigger(Func<IElement, Task> trigger)
		{
			if (trigger is null)
				throw new ArgumentNullException(nameof(trigger));

			return trigger(renderedComponent.Find(element));
		}

		private void CallbackHandler(TEventArgs args) => receivedEvent = args;
	}
}
