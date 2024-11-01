#if NET9_0_OR_GREATER

namespace Bunit.TestAssets.RenderModes;

public class RendererInfoComponent : ComponentBase
{
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "p");
		builder.AddContent(1, "Is interactive: ");
		builder.AddContent(2, RendererInfo.IsInteractive);
		builder.CloseElement();

		builder.OpenElement(3, "p");
		builder.AddContent(4, "Rendermode: ");
		builder.AddContent(5, RendererInfo.Name);
		builder.CloseElement();
	}
}
#endif