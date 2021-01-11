using System;
using Bunit.Rendering;
using Bunit.TestAssets.SampleComponents;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class RenderedComponentTest : TestContext
	{
		[Fact(DisplayName = "Call to Render() results in a render of component")]
		public void Test001()
		{
			var cut = RenderComponent<Wrapper>(parameters => parameters.AddChildContent("<div>"));
			var initialRenderCount = cut.RenderCount;

			cut.Render();

			cut.RenderCount.ShouldBe(initialRenderCount + 1);
		}

		[Fact(DisplayName = "Call to SetParametersAndRender(builder) provides the parameters to component")]
		public void Test004()
		{
			var cut = RenderComponent<Wrapper>(parameters => parameters.AddChildContent("<div>"));

			cut.SetParametersAndRender(parameters => parameters.AddChildContent("<p>"));

			cut.Find("p").ShouldNotBeNull();
		}

		[Fact(DisplayName = "Call to SetParametersAndRender(params) provides the parameters to component")]
		public void Test0041()
		{
			var cut = RenderComponent<Wrapper>(parameters => parameters.AddChildContent("<div>"));

			cut.SetParametersAndRender(ComponentParameterFactory.ChildContent("<p>"));

			cut.Find("p").ShouldNotBeNull();
		}

		[Fact(DisplayName = "Trying to set CascadingValue during SetParametersAndRender throws")]
		public void Test003()
		{
			// arrange
			var cut = RenderComponent<AllTypesOfParams<string>>();

			// assert
			Should.Throw<InvalidOperationException>(() => cut.SetParametersAndRender(ps => ps.Add(p => p.UnnamedCascadingValue, 42)));
			Should.Throw<InvalidOperationException>(() => cut.SetParametersAndRender(ps => ps.Add(p => p.NamedCascadingValue, 1337)));
		}

		[Fact(DisplayName = "Getting Instance from a RenderedComponent based on a disposed component throws")]
		public void Test020()
		{
			var cut = RenderComponent<ToggleChildComponent>(ps => ps.Add(p => p.ShowChild, true));
			var target = cut.FindComponent<Simple1>();

			// Disposes of <Simple1 />
			cut.SetParametersAndRender(ps => ps.Add(p => p.ShowChild, false));

			Should.Throw<ComponentDisposedException>(() => target.Instance);
		}
	}
}
