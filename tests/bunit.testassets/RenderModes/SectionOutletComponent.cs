using Microsoft.AspNetCore.Components.Web;

#if NET8_0_OR_GREATER
using Microsoft.AspNetCore.Components.Sections;
#endif

namespace Bunit.TestAssets.RenderModes;

public class SectionOutletComponent : ComponentBase
{
#if NET8_0_OR_GREATER
	private static readonly Guid SectionId = Guid.NewGuid();
#endif

	[Parameter] public RenderFragment ChildContent { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
#if NET8_0_OR_GREATER
		builder.OpenComponent<SectionOutlet>(0);
		builder.AddComponentParameter(1, nameof(SectionOutlet.SectionId), SectionId);
		builder.AddComponentRenderMode(RenderMode.InteractiveWebAssembly);
		builder.CloseComponent();
		builder.OpenComponent<SectionContent>(10);
		builder.AddComponentParameter(11, nameof(SectionContent.SectionId), SectionId);
		builder.AddAttribute(12, nameof(SectionContent.ChildContent), ChildContent);
		builder.CloseComponent();
#endif
	}
}
