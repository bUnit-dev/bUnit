using Bunit.Rendering;

namespace Bunit;

public class RenderedComponentTest : TestContext
{
	[Fact(DisplayName = "Call to Render() results in a render of component")]
	public async Task Test001()
	{
		var cut = await RenderComponent<Wrapper>(parameters => parameters.AddChildContent("<div>"));
		var initialRenderCount = cut.RenderCount;

		await cut.Render();

		cut.RenderCount.ShouldBe(initialRenderCount + 1);
	}

	[Fact(DisplayName = "Call to SetParametersAndRender(builder) provides the parameters to component")]
	public async Task Test004()
	{
		var cut = await RenderComponent<Wrapper>(parameters => parameters.AddChildContent("<div>"));

		await cut.SetParametersAndRender(parameters => parameters.AddChildContent("<p>"));

		cut.Find("p").ShouldNotBeNull();
	}

	[Fact(DisplayName = "Call to SetParametersAndRender(params) provides the parameters to component")]
	public async Task Test0041()
	{
		var cut = await RenderComponent<Wrapper>(parameters => parameters.AddChildContent("<div>"));

		await cut.SetParametersAndRender(ComponentParameterFactory.ChildContent("<p>"));

		cut.Find("p").ShouldNotBeNull();
	}

	[Fact(DisplayName = "Trying to set CascadingValue during SetParametersAndRender throws")]
	public async Task Test003()
	{
		// arrange
		var cut = await RenderComponent<AllTypesOfParams<string>>();

		// assert
		await Should.ThrowAsync<InvalidOperationException>(() => cut.SetParametersAndRender(ps => ps.Add(p => p.UnnamedCascadingValue, 42)));
		await Should.ThrowAsync<InvalidOperationException>(() => cut.SetParametersAndRender(ps => ps.Add(p => p.NamedCascadingValue, 1337)));
	}

	[Fact(DisplayName = "Getting Instance from a RenderedComponent based on a disposed component throws")]
	public async Task Test020()
	{
		var cut = await RenderComponent<ToggleChildComponent>(ps => ps.Add(p => p.ShowChild, true));
		var target = cut.FindComponent<Simple1>();

		// Disposes of <Simple1 />
		await cut.SetParametersAndRender(ps => ps.Add(p => p.ShowChild, false));

		Should.Throw<ComponentDisposedException>(() => target.Instance);
	}
}
