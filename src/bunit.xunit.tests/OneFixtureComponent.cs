using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.SampleComponents
{
	public class OneFixtureComponent : TestComponentBase
	{		
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent<Fixture>(0);
			builder.AddAttribute(1, nameof(Fixture.Description), "Fixture name");
			//builder.AddAttribute(2, nameof(Fixture.ChildContent), (RenderFragment)Content);
			builder.CloseComponent();
		}

		private void Content(RenderTreeBuilder builder) { }
	}
}
