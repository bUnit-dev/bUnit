namespace Bunit.Web.Stub.Components;

public class ContainerComponent : ComponentBase
{
	[Parameter]
	public RenderFragment ChildContent { get; set; }
	
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "div");
		builder.AddContent(1, ChildContent);
		builder.CloseElement();
	}
}
