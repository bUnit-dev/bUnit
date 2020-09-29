using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Bunit.Diffing;

namespace Bunit.Rendering
{
	/// <summary>
	/// A AngleSharp based HTML Parse that can parse markup strings
	/// into a <see cref="INodeList"/>.
	/// </summary>
	public sealed class BunitHtmlParser : IDisposable
	{
		private static readonly string[] TABLE_SUB_ELEMENTS = { "CAPTION", "COLGROUP", "TBODY", "TFOOT", "THEAD", };
		private const string TBODY_SUB_ELEMENT = "TR";
		private static readonly string[] TR_SUB_ELEMENTS = { "TD", "TH" };
		private const string COLGROUP_SUB_ELEMENT = "COL";
		private static readonly string[] SPECIAL_HTML_ELEMENTS = { "HTML", "HEAD", "BODY" };

		private readonly IBrowsingContext _context;
		private readonly IHtmlParser _htmlParser;
		private readonly List<IDocument> _documents = new List<IDocument>();

		/// <summary>
		/// Creates an instance of the parser with a AngleSharp context 
		/// without a <see cref="TestRenderer"/> registered.
		/// </summary>
		public BunitHtmlParser() : this(Configuration.Default.WithCss().With(new HtmlComparer())) { }

		/// <summary>
		/// Creates an instance of the parser with a AngleSharp context 
		/// with the <paramref name="testRenderer"/> registered.
		/// </summary>
		public BunitHtmlParser(ITestRenderer testRenderer, HtmlComparer htmlComparer)
			: this(Configuration.Default.WithCss().With(testRenderer).With(htmlComparer)) { }

		private BunitHtmlParser(IConfiguration angleSharpConfiguration)
		{
			var config = angleSharpConfiguration.With(this);
			_context = BrowsingContext.New(config);
			_htmlParser = _context.GetService<IHtmlParser>();
		}

		/// <summary>
		/// Parses a markup HTML string using AngleSharps HTML5 parser.
		/// </summary>
		/// <param name="markup">The markup to parse.</param>
		/// <returns>The <see cref="INodeList"/>.</returns>
		public INodeList Parse(string markup)
		{
			if (markup is null) throw new ArgumentNullException(nameof(markup));
			var (ctx, matchedElement) = GetParseContext(markup).GetAwaiter().GetResult();

			return ctx is null && matchedElement is not null
				? ParseSpecial(markup, matchedElement)
				: _htmlParser.ParseFragment(markup, ctx);
		}

		private INodeList ParseSpecial(string markup, string matchedElement)
		{
			var doc = _htmlParser.ParseDocument(markup);

			return matchedElement switch
			{
				"HTML" => new SingleNodeNodeList(doc.Body.ParentElement),
				"HEAD" => new SingleNodeNodeList(doc.Head),
				"BODY" => new SingleNodeNodeList(doc.Body),
				_ => throw new InvalidOperationException($"{matchedElement} should not be parsed by {nameof(ParseSpecial)}.")
			};
		}

		private async Task<(IElement? ctx, string? matchedElement)> GetParseContext(string markup)
		{
			var document = await GetNewDocument().ConfigureAwait(false);
			var startIndex = markup.IndexOfFirstNonWhitespaceChar();

			// verify that first non-whitespace characters is a '<'
			if (markup.Length > 0 && markup[startIndex].IsTagStart())
			{
				return GetParseContextFromTag(markup, startIndex, document);
			}
			else
			{
				return (document.Body, null);
			}
		}

		private static (IElement? ctx, string? matchedElement) GetParseContextFromTag(string markup, int startIndex, IDocument document)
		{
			IElement? context = null;
			string? matchedElement;

			if (markup.StartsWithElements(TABLE_SUB_ELEMENTS, startIndex, out matchedElement))
			{
				context = CreateTable();
			}
			else if (markup.StartsWithElements(TR_SUB_ELEMENTS, startIndex, out matchedElement))
			{
				context = CreateTable().AppendElement(document.CreateElement("tr"));
			}
			else if (markup.StartsWithElement(TBODY_SUB_ELEMENT, startIndex))
			{
				context = CreateTable().AppendElement(document.CreateElement("tbody"));
				matchedElement = TBODY_SUB_ELEMENT;
			}
			else if (markup.StartsWithElement(COLGROUP_SUB_ELEMENT, startIndex))
			{
				context = CreateTable().AppendElement(document.CreateElement("colgroup"));
				matchedElement = COLGROUP_SUB_ELEMENT;
			}
			else if (markup.StartsWithElements(SPECIAL_HTML_ELEMENTS, startIndex, out matchedElement)) { }
			else
			{
				context = document.Body;
			}

			return (context, matchedElement);

			IElement CreateTable() => document.Body.AppendElement(document.CreateElement("table"));
		}

		private async Task<IDocument> GetNewDocument()
		{
			var result = await _context.OpenNewAsync().ConfigureAwait(false);
			_documents.Add(result);
			return result;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_context.Dispose();
			foreach (var doc in _documents)
			{
				doc.Dispose();
			}
		}

		private class SingleNodeNodeList : INodeList
		{
			private readonly INode node;
			public INode this[int index]
			{
				get
				{
					if (index != 0) throw new IndexOutOfRangeException();
					return node;
				}
			}
			public int Length { get; } = 1;
			public SingleNodeNodeList(INode node) => this.node = node ?? throw new ArgumentNullException(nameof(node));
			public IEnumerator<INode> GetEnumerator()
			{
				yield return node;
			}
			public void ToHtml(TextWriter writer, IMarkupFormatter formatter) => node.ToHtml(writer, formatter);
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}
	}
}
