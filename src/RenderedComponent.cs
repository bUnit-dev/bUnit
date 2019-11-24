using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public class RenderedComponent<TComponent> : IRenderedFragment where TComponent : class, IComponent
    {
        private readonly TestRenderer _renderer;
        private readonly HtmlParser _htmlParser;
        private TComponent? _instance;

        protected ContainerComponent Container { get; }

        protected int ComponentId { get; set; }

        public TComponent Instance
        {
            get => _instance ?? throw new InvalidOperationException("Render component first before retrieving the instance.");
            private set => _instance = value;
        }

        internal RenderedComponent(TestRenderer renderer, HtmlParser htmlParser)
        {
            _renderer = renderer;
            _htmlParser = htmlParser;
            Container = new ContainerComponent(_renderer);
        }

        public string GetMarkup() => Htmlizer.GetHtml(_renderer, ComponentId);

        public INodeList GetNodes()
        {
            var markup = GetMarkup();
            return _htmlParser.Parse(markup);
        }

        internal void SetParametersAndRender(ParameterView parameters)
        {
            Container.RenderComponentUnderTest(typeof(TComponent), parameters);
            (ComponentId, Instance) = Container.GetComponent<TComponent>();
        }

        internal void Render(RenderFragment renderFragment)
        {
            Container.RenderComponentUnderTest(renderFragment);
            (ComponentId, Instance) = Container.GetComponent<TComponent>();
        }
    }
}
