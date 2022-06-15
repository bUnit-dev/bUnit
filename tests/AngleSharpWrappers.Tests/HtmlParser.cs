using System;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace AngleSharpWrappers;

    public sealed class HtmlParser : IDisposable
    {
        private readonly IBrowsingContext _context;
        private readonly IHtmlParser _htmlParser;
        private readonly IDocument _document;

        public HtmlParser()
        {
            var config = Configuration.Default;
            _context = BrowsingContext.New(config);
            _htmlParser = _context.GetService<IHtmlParser>()!;
            _document = _context.OpenNewAsync().Result;
        }

        public INodeList Parse(string? html)
        {
            return _htmlParser.ParseFragment(html ?? string.Empty, _document.Body!);
        }

        public void Dispose()
        {
            _context.Dispose();
            _document.Dispose();
        }
    }
