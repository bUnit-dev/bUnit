using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Sections;

namespace Bunit.TestAssets.RenderModes;

public class SectionOutletComponent : ComponentBase
{
	private static readonly Guid SectionId = Guid.NewGuid();

	[Parameter] public RenderFragment ChildContent { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenComponent<SectionOutlet>(0);
		builder.AddComponentParameter(1, nameof(SectionOutlet.SectionId), SectionId);
		builder.AddComponentRenderMode(RenderMode.InteractiveWebAssembly);
		builder.CloseComponent();
		builder.OpenComponent<SectionContent>(10);
		builder.AddComponentParameter(11, nameof(SectionContent.SectionId), SectionId);
		builder.AddAttribute(12, nameof(SectionContent.ChildContent), ChildContent);
		builder.CloseComponent();
	}
}
