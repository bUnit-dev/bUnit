using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.TestAssets.SampleComponents;

public class WrapperDiv : ComponentBase
{
	[Parameter] public RenderFragment? ChildContent { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", "wrapper");
		builder.AddContent(10, ChildContent);
		builder.CloseElement();
	}
}
