using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public class RenderedFragment : IRenderedFragment
    {
        private readonly TestRenderer _renderer;
        private readonly HtmlParser _htmlParser;
        private readonly int _componentId;
        private ContainerComponent _container;
        private string? markup;
        private INodeList? _nodes;

        internal RenderedFragment(TestRenderer renderer, HtmlParser htmlParser)
        {
            _renderer = renderer;
            _htmlParser = htmlParser;
            _container = new ContainerComponent(_renderer);
            _componentId = _container.ComponentId;
        }

        public string GetMarkup()
        {
            if (markup is null)
                markup = Htmlizer.GetHtml(_renderer, _componentId);
            return markup;
        }

        public INodeList GetNodes()
        {
            if(_nodes is null)
                _nodes = _htmlParser.Parse(GetMarkup());
            return _nodes;
        }

        internal void Render(RenderFragment renderFragment) => _container.RenderComponentUnderTest(renderFragment);
    }
}
