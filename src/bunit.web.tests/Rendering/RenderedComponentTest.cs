using Bunit.TestAssets.SampleComponents;

using Shouldly;

using Xunit;

namespace Bunit
{
	public class RenderedComponentTest : ComponentTestFixture
	{
		[Fact(DisplayName = "Nodes should return the same instance " +
							"when a render has not resulted in any changes")]
		public void Test003()
		{
			var cut = RenderComponent<Wrapper>(ChildContent("<div>"));
			var initialNodes = cut.Nodes;

			cut.Render();
			cut.SetParametersAndRender(ChildContent("<div>"));

			Assert.Same(initialNodes, cut.Nodes);
		}

		[Fact(DisplayName = "Nodes should return new instance " +
							"when a SetParametersAndRender has caused changes to DOM tree")]
		public void Test004()
		{
			var cut = RenderComponent<Wrapper>(ChildContent("<div>"));
			var initialNodes = cut.Nodes;

			cut.SetParametersAndRender(ChildContent("<p>"));

			Assert.NotSame(initialNodes, cut.Nodes);
			cut.Find("p").ShouldNotBeNull();
		}

		[Fact(DisplayName = "Nodes should return new instance " +
							"when a Render has caused changes to DOM tree")]
		public void Test005()
		{
			var cut = RenderComponent<RenderCounter>();
			var initialNodes = cut.Nodes;

			cut.Render();

			Assert.NotSame(initialNodes, cut.Nodes);
		}
	}
}
