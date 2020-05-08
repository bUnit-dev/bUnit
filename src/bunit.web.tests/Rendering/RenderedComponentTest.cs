using Bunit.TestAssets.SampleComponents;

using Shouldly;

using Xunit;

namespace Bunit
{
	public class RenderedComponentTest : TestContext
	{
		[Fact(DisplayName = "Call to Render() results in a render of component")]
		public void Test003()
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
	}
}
