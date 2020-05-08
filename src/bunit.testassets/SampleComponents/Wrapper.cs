using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.TestAssets.SampleComponents
{
	public class Wrapper : ComponentBase
	{
		[Parameter] public RenderFragment? ChildContent { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(0, ChildContent);
	}
}
