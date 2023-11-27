using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.Web.Stub.Components;

public class ParentComponent : ComponentBase
{
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenComponent<CounterComponent>(1);
		builder.AddAttribute(2, "Count", 2);
		builder.CloseComponent();
	}
}
