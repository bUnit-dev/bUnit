using System;

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

using Bunit.Rendering;

namespace Bunit.Diffing
{
	/// <summary>
	/// A AngleSharp based HTML Parse that can parse markup strings
	/// into a <see cref="INodeList"/>.
	/// </summary>
	public sealed class HtmlParser : IDisposable
	{
		private readonly IBrowsingContext _context;
		private readonly IHtmlParser _htmlParser;
		private readonly IDocument _document;

		/// <summary>
		/// Creates an instance of the parser with a AngleSharp context 
		/// without a <see cref="TestRenderer"/> registered.
		/// </summary>
		public HtmlParser()
		{
			var config = Configuration.Default
				.WithCss()
				.With(new HtmlComparer())
				.With(this);

			_context = BrowsingContext.New(config);
			_htmlParser = _context.GetService<IHtmlParser>();
			_document = _context.OpenNewAsync().Result;
		}

		/// <summary>
		/// Creates an instance of the parser with a AngleSharp context 
		/// with the <paramref name="testRenderer"/> registered.
		/// </summary>
		public HtmlParser(ITestRenderer testRenderer, HtmlComparer htmlComparer)
		{
			var config = Configuration.Default
				.WithCss()
				.With(testRenderer) // added to allow elements to find the renderer to trigger events
				.With(htmlComparer)
				.With(this);

			_context = BrowsingContext.New(config);
			_htmlParser = _context.GetService<IHtmlParser>();
			_document = _context.OpenNewAsync().Result;
		}

		/// <summary>
		/// Parses a markup HTML string using AngleSharps HTML5 parser.
		/// </summary>
		/// <param name="markup">The markup to parse.</param>
		/// <returns>The <see cref="INodeList"/>.</returns>
		public INodeList Parse(string markup)
		{
			return _htmlParser.ParseFragment(markup, _document.Body);
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_document.Dispose();
			_context.Dispose();
		}
	}
}
