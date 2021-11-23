using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.SampleComponents;

public class OneFixtureComponent : TestComponentBase
{
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenComponent<Fixture>(0);
		builder.AddAttribute(1, nameof(Fixture.Description), "FIXTURE 1");
		builder.AddAttribute(2, nameof(Fixture.ChildContent), (RenderFragment)Content);
		builder.AddAttribute(3, nameof(Fixture.Test), (Action<Fixture>)TestMethod);
		builder.CloseComponent();
	}

	private void TestMethod(Fixture fixture) { }

	private void Content(RenderTreeBuilder builder) { }
}
