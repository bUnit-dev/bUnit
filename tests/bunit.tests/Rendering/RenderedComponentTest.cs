using Bunit.Rendering;

namespace Bunit;

public class RenderedComponentTest : TestContext
{
	[Fact(DisplayName = "Call to Render() results in a render of component")]
	public void Test001()
	{
		var cut = Render<Wrapper>(parameters => parameters.AddChildContent("<div>"));
		var initialRenderCount = cut.RenderCount;

		cut.Render();

		cut.RenderCount.ShouldBe(initialRenderCount + 1);
	}

	[Fact(DisplayName = "Call to Render(builder) provides the parameters to component")]
	public void Test004()
	{
		var cut = Render<Wrapper>(parameters => parameters.AddChildContent("<div>"));

		cut.Render(parameters => parameters.AddChildContent("<p>"));

		cut.Find("p").ShouldNotBeNull();
	}

	[Fact(DisplayName = "Call to Render(params) provides the parameters to component")]
	public void Test0041()
	{
		var cut = Render<Wrapper>(parameters => parameters.AddChildContent("<div>"));

		cut.Render(ps => ps.AddChildContent("<p>"));

		cut.Find("p").ShouldNotBeNull();
	}

	[Fact(DisplayName = "Trying to set CascadingValue during Render throws")]
	public void Test003()
	{
		// arrange
		var cut = Render<AllTypesOfParams<string>>();

		// assert
		Should.Throw<InvalidOperationException>(() => cut.Render(ps => ps.Add(p => p.UnnamedCascadingValue, 42)));
		Should.Throw<InvalidOperationException>(() => cut.Render(ps => ps.Add(p => p.NamedCascadingValue, 1337)));
	}

	[Fact(DisplayName = "Getting Instance from a RenderedComponent based on a disposed component throws")]
	public void Test020()
	{
		var cut = Render<ToggleChildComponent>(ps => ps.Add(p => p.ShowChild, true));
		var target = cut.FindComponent<Simple1>();

		// Disposes of <Simple1 />
		cut.Render(ps => ps.Add(p => p.ShowChild, false));

		Should.Throw<ComponentDisposedException>(() => target.Instance);
	}
}
