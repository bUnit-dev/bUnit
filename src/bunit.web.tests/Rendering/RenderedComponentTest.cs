using System;
using Bunit.TestAssets.SampleComponents;
using Bunit.TestDoubles.JSInterop;
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

			cut.SetParametersAndRenderAsync(parameters => parameters.AddChildContent("<p>"));

			cut.Find("p").ShouldNotBeNull();
		}


		[Fact(DisplayName = "Call to SetParametersAndRender(params) provides the parameters to component")]
		public void Test0041()
		{
			var cut = RenderComponent<Wrapper>(parameters => parameters.AddChildContent("<div>"));

			cut.SetParametersAndRenderAsync(parameters => parameters.AddChildContent("<p>"));

			cut.Find("p").ShouldNotBeNull();
		}


		[Fact(DisplayName = "Trying to set CascadingValue during SetParametersAndRender throws")]
		public void Test003()
		{
			// arrange
			Services.AddMockJSRuntime();
			var cut = RenderComponent<AllTypesOfParams<string>>();

			// assert
			Should.Throw<InvalidOperationException>(() => cut.SetParametersAndRenderAsync(ps => ps.Add(p => p.UnnamedCascadingValue, 42)));
			Should.Throw<InvalidOperationException>(() => cut.SetParametersAndRenderAsync(ps => ps.Add(p => p.NamedCascadingValue, 1337)));
		}
	}
}
