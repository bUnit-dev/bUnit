using System;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.SampleComponents
{
	public class SkipRazorComponent : TestComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent<Fixture>(10);
			builder.AddAttribute(11, nameof(Fixture.Description), "FIXTURE SKIPPED");
			builder.AddAttribute(11, nameof(Fixture.Skip), "SKIP ME");
			builder.AddAttribute(12, nameof(Fixture.ChildContent), (RenderFragment)Content);
			builder.AddAttribute(13, nameof(Fixture.Test), (Action<Fixture>)TestMethod);
			builder.CloseComponent();
		}

		private void TestMethod(Fixture fixture) { }

		private void Content(RenderTreeBuilder builder) { }
	}
}
