using System;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit.Rendering;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	public class TriggerEventSpy<TEventArgs> where TEventArgs : EventArgs, new()
    {
        private readonly IRenderedComponent<TriggerTester<TEventArgs>> _renderedComponent;
        private readonly string _element;
        private TEventArgs? _receivedEvent;

        public TEventArgs RaisedEvent => _receivedEvent!;

        public TriggerEventSpy(Func<ComponentParameter[], IRenderedComponent<TriggerTester<TEventArgs>>> componentRenderer, string element, string eventName)
        {
            if (componentRenderer is null) throw new ArgumentNullException(nameof(componentRenderer));

            _renderedComponent = componentRenderer(new ComponentParameter[] {
                    (nameof(TriggerTester<TEventArgs>.Element), element),
                    (nameof(TriggerTester<TEventArgs>.EventName), eventName),
                    (nameof(TriggerTester<TEventArgs>.TriggeredEvent), EventCallback.Factory.Create<TEventArgs>(this, CallbackHandler))
                });
            _element = element;
        }

        public void Trigger(Action<IElement> trigger)
        {
            if (trigger is null) throw new ArgumentNullException(nameof(trigger));

            trigger(_renderedComponent.Find(_element));
        }

        public Task Trigger(Func<IElement, Task> trigger)
        {
            if (trigger is null) throw new ArgumentNullException(nameof(trigger));

            return trigger(_renderedComponent.Find(_element));
        }

        private void CallbackHandler(TEventArgs args) => _receivedEvent = args;
    }
}
