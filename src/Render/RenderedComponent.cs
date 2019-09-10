using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace Egil.RazorComponents.Testing.Render
{
    public class RenderedComponent<TComponent> where TComponent : IComponent
    {
        private readonly TestRenderer _renderer;
        private readonly ContainerComponent _containerTestRootComponent;
        private int _testComponentId;
        private TComponent _testComponentInstance;

        internal RenderedComponent(TestRenderer renderer)
        {
            _renderer = renderer;
            _containerTestRootComponent = new ContainerComponent(_renderer);
        }

        public TComponent Instance => _testComponentInstance;

        public string GetMarkup()
        {
            return Htmlizer.GetHtml(_renderer, _testComponentId);
        }

        internal void SetParametersAndRender(ParameterView parameters)
        {
            _containerTestRootComponent.RenderComponentUnderTest(typeof(TComponent), parameters);
            var foundTestComponent = _containerTestRootComponent.FindComponentUnderTest<TComponent>();
            _testComponentId = foundTestComponent.Id;
            _testComponentInstance = foundTestComponent.Component;
        }

        public HtmlNode Find(string selector)
        {
            return FindAll(selector).FirstOrDefault();
        }

        public ICollection<HtmlNode> FindAll(string selector)
        {
            // Rather than using HTML strings, it would be faster and more powerful
            // to implement Fizzler's APIs for walking directly over the rendered
            // frames, since Fizzler's core isn't tied to HTML (or HtmlAgilityPack).
            // The most awkward part of this will be handling Markup frames, since
            // they are HTML strings so would need to be parsed, or perhaps you can
            // pass through those calls into Fizzler.Systems.HtmlAgilityPack.

            var markup = GetMarkup();
            var html = new TestHtmlDocument(_renderer);

            html.LoadHtml(markup);
            return html.DocumentNode.QuerySelectorAll(selector).ToList();
        }
    }
}
