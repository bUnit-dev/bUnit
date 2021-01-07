using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit.TestAssets.SampleComponents
{
	public class EventBubbles : ComponentBase
	{
		[Parameter] public string ChildElementType { get; set; } = "div";
		[Parameter] public bool ChildElementDisabled { get; set; }
		[Parameter] public string? EventName { get; set; }
		[Parameter] public bool GrandParentStopPropergation { get; set; }
		[Parameter] public bool ParentStopPropergation { get; set; }
		[Parameter] public bool ChildStopPropergation { get; set; }
		public int GrandParentTriggerCount { get; private set; }
		public int ParentTriggerCount { get; private set; }
		public int ChildTriggerCount { get; private set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (EventName is null) return;

			builder.OpenElement(0, "div");
			builder.AddAttribute(1, EventName, EventCallback.Factory.Create<EventArgs>(this, (evt) => GrandParentTriggerCount++));
			builder.AddAttribute(2, "id", "grand-parent");
			builder.AddEventStopPropagationAttribute(3, EventName, GrandParentStopPropergation);

			builder.OpenElement(10, "div");
			builder.AddAttribute(11, EventName, EventCallback.Factory.Create<EventArgs>(this, (evt) => ParentTriggerCount++));
			builder.AddAttribute(12, "id", "parent");
			builder.AddEventStopPropagationAttribute(13, EventName, ParentStopPropergation);

			builder.OpenElement(20, ChildElementType);

			builder.AddAttribute(21, EventName, EventCallback.Factory.Create<EventArgs>(this, (evt) => ChildTriggerCount++));
			builder.AddAttribute(22, "id", "child");
			builder.AddEventStopPropagationAttribute(23, EventName, ChildStopPropergation);
			if (ChildElementDisabled)
			{
				builder.AddAttribute(24, "disabled", "disabled");
			}

			builder.CloseElement();

			builder.CloseElement();

			builder.CloseElement();
		}
	}
}
