using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Egil.RazorComponents.Testing
{
    public class RenderedFragment : IRenderedFragment
    {
        private readonly RenderFragment _renderFragment;
        protected int ComponentId { get; }
        protected ContainerComponent Container { get; }
        public TestHost TestContext { get; }

        internal RenderedFragment(TestHost testContext, RenderFragment renderFragment)
        {
            TestContext = testContext;
            _renderFragment = renderFragment;
            Container = new ContainerComponent(testContext.Renderer);
            ComponentId = Container.ComponentId;
            Container.RenderComponentUnderTest(_renderFragment);
        }

        public string GetMarkup()
        {
            return Htmlizer.GetHtml(TestContext.Renderer, ComponentId);
        }

        public INodeList GetNodes()
        {
            return TestContext.HtmlParser.Parse(GetMarkup());
        }
    }
}
