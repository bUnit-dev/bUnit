using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.TestAssets.SampleComponents
{
	public class TriggerTester<TEventArgs> : ComponentBase
	    where TEventArgs : EventArgs, new()
	{
		[Parameter] public string Element { get; set; } = "p";
		[Parameter] public string EventName { get; set; } = "p";
		[Parameter] public EventCallback<TEventArgs>? TriggeredEvent { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (builder is null)
				throw new ArgumentNullException(nameof(builder));
			if (TriggeredEvent is null)
				throw new InvalidOperationException($"{nameof(TriggeredEvent)} is null");

			builder.OpenElement(0, Element);
			builder.AddAttribute(1, EventName, EventCallback.Factory.Create(this, TriggeredEvent.Value));
			builder.CloseElement();
		}
	}
}
