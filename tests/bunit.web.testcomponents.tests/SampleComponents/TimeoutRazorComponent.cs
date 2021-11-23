using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.SampleComponents;

public class TimeoutRazorComponent : TestComponentBase
{
	public static readonly TimeSpan TIMEOUT = TimeSpan.FromMilliseconds(1337);

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenComponent<Fixture>(10);
		builder.AddAttribute(11, nameof(Fixture.Description), "FIXTURE WITH TIMEOUT");
		builder.AddAttribute(11, nameof(Fixture.Timeout), TIMEOUT);
		builder.AddAttribute(12, nameof(Fixture.ChildContent), (RenderFragment)Content);
		builder.AddAttribute(13, nameof(Fixture.Test), (Action<Fixture>)TestMethod);
		builder.CloseComponent();
	}

	private void TestMethod(Fixture fixture) { }

	private void Content(RenderTreeBuilder builder) { }
}
