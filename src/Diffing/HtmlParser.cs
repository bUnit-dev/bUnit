using AngleSharp.Html.Parser;
using AngleSharp;
using AngleSharp.Dom;
using System;
using Egil.RazorComponents.Testing.Diffing;

namespace Egil.RazorComponents.Testing
{
    public sealed class HtmlParser : IDisposable
    {
        private readonly IBrowsingContext _context;
        private readonly IHtmlParser _htmlParser;
        private readonly IDocument _document;

        public HtmlParser(TestRenderer testRenderer, HtmlComparer comparer)
        {
            var config = Configuration.Default
                .WithCss()
                .With(testRenderer)
                .With(comparer)
                .With(this);

            _context = BrowsingContext.New(config);
            _htmlParser = _context.GetService<IHtmlParser>();
            _document = _context.OpenNewAsync().Result;
        }

        public INodeList Parse(string html)
        {
            return _htmlParser.ParseFragment(html, _document.Body);
        }

        public void Dispose()
        {
            _document.Dispose();
        }
    }
}