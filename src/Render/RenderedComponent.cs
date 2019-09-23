using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Io;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace Egil.RazorComponents.Testing.Render
{
    public class RenderedComponent<TComponent> where TComponent : class, IComponent
    {
        private readonly IBrowsingContext _browsingContext = BrowsingContext.New(Configuration.Default);
        private readonly TestRenderer _renderer;
        private readonly ContainerComponent _containerTestRootComponent;
        private int _testComponentId;
        private TComponent? _testComponentInstance;

        internal RenderedComponent(TestRenderer renderer)
        {
            _renderer = renderer;
            _containerTestRootComponent = new ContainerComponent(_renderer);
        }

        public TComponent? Instance => _testComponentInstance;

        public string GetMarkup()
        {
            return Htmlizer.GetHtml(_renderer, _testComponentId);
        }

        internal void SetParametersAndRender(ParameterView parameters)
        {
            //_containerTestRootComponent.RenderComponentUnderTest(typeof(TComponent), parameters);
            //(_testComponentId, _testComponentInstance) = _containerTestRootComponent.FindComponentUnderTest<TComponent>();
        }

        public IElement Find(string selector)
        {
            return FindAll(selector).FirstOrDefault();
        }

        public IHtmlCollection<IElement> FindAll(string selector)
        {
            var markup = GetMarkup();

            var document = _browsingContext.OpenAsync(x => x.Content(markup)).GetAwaiter().GetResult();
            return document.QuerySelectorAll(selector);

            //var parser = context.GetService<IHtmlParser>();
            //var rootElement = document.QuerySelector(root) ?? document.CreateElement(root);
            //var actualFragment = parser.ParseFragment(actual, rootElement);

            //var html = new TestHtmlDocument(_renderer);

            //html.LoadHtml(markup);
            //return html.DocumentNode.QuerySelectorAll(selector).ToList();
        }
    }
}
