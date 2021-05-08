using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.TestAssets.SampleComponents
{
	public class Simple1 : ComponentBase
	{
		[Parameter] public string Header { get; set; } = string.Empty;

		[Parameter] public string AttrValue { get; set; } = string.Empty;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "h1");
			builder.AddAttribute(1, "id", "header");
			builder.AddAttribute(2, "attr", AttrValue);
			builder.AddContent(3, Header);
			builder.CloseElement();
		}
	}
}
