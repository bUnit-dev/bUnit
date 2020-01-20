using Egil.RazorComponents.Testing.EventDispatchExtensions;
using Egil.RazorComponents.Testing.SampleComponents;
using Shouldly;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    public class RenderComponentTest : ComponentTestFixture
    {
        [Fact(DisplayName = "GetNodes should return the same instance " +
                            "when a render has not resulted in any changes")]
        public void Test003()
        {
            var cut = RenderComponent<Wrapper>(ChildContent("<div>"));
            var initialNodes = cut.GetNodes();

            cut.Render();
            cut.SetParametersAndRender(ChildContent("<div>"));

            Assert.Same(initialNodes, cut.GetNodes());
        }

        [Fact(DisplayName = "GetNodes should return new instance " +
                            "when a SetParametersAndRender has caused changes to DOM tree")]
        public void Tets004()
        {
            var cut = RenderComponent<Wrapper>(ChildContent("<div>"));
            var initialNodes = cut.GetNodes();

            cut.SetParametersAndRender(ChildContent("<p>"));

            Assert.NotSame(initialNodes, cut.GetNodes());
            cut.Find("p").ShouldNotBeNull();
        }

        [Fact(DisplayName = "GetNodes should return new instance " +
                            "when a Render has caused changes to DOM tree")]
        public void Tets005()
        {
            var cut = RenderComponent<RenderCounter>();
            var initialNodes = cut.GetNodes();

            cut.Render();

            Assert.NotSame(initialNodes, cut.GetNodes());
        }

        [Fact(DisplayName = "GetNodes should return new instance " +
                            "when a event handler trigger has caused changes to DOM tree")]
        public void Tets006()
        {
            var cut = RenderComponent<ClickCounter>();
            var initialNodes = cut.GetNodes();

            cut.Find("button").Click();

            Assert.NotSame(initialNodes, cut.GetNodes());
        }


    }

}
