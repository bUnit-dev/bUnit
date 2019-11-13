using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Strategies;
using AngleSharp.Diffing;
using AngleSharp.Html.Parser;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Diffing.Core;

namespace Egil.RazorComponents.Testing
{
    public sealed class HtmlComparer : IHtmlComparer
    {
        private readonly IBrowsingContext _context;
        private readonly IHtmlParser _htmlParser;
        private readonly IDocument _document;
        private readonly HtmlDifferenceEngine _differenceEngine;

        public HtmlComparer()
        {
            var diffOptions = new DiffingStrategyPipeline();
            diffOptions.AddDefaultOptions();
            _differenceEngine = new HtmlDifferenceEngine(diffOptions);
            var config = Configuration.Default.WithCss();
            _context = BrowsingContext.New(config);
            _htmlParser = _context.GetService<IHtmlParser>();
            _document = _context.OpenNewAsync().Result;
        }

        public List<IDiff> Compare(string controlHtml, string testHtml)
        {
            return _differenceEngine.Compare(Parse(controlHtml), Parse(testHtml)).ToList();
        }

        public void Dispose()
        {
            _document.Dispose();
        }

        private INodeList Parse(string html)
        {
            return _htmlParser.ParseFragment(html, _document.Body);
        }
    }
}