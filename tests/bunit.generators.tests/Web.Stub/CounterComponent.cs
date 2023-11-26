using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.Web.Stub;

public class ParentComponent : ComponentBase
{
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenComponent<CounterComponent>(1);
		builder.AddAttribute(2, "Count", 2);
		builder.CloseComponent();
	}
}

public class CounterComponent : ComponentBase
{
	[Parameter] public int Count { get; set; }
	[CascadingParameter] public int CascadingCount { get; set; }
	[Parameter] public EventCallback IncrementCount { get; set; }
}
