using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
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
		private const string TbodySubElements = "TR";
		private const string ColgroupSubElement = "COL";
		private static readonly string[] TableSubElements = { "CAPTION", "COLGROUP", "TBODY", "TFOOT", "THEAD", };
		private static readonly string[] TrSubElements = { "TD", "TH" };
		private static readonly string[] SpecialHtmlElements = { "HTML", "HEAD", "BODY" };

		private readonly IBrowsingContext context;
		private readonly IHtmlParser htmlParser;
		private readonly List<IDocument> documents = new();

		/// <summary>
		/// Initializes a new instance of the <see cref="BunitHtmlParser"/> class
		/// with a AngleSharp context without a <see cref="TestRenderer"/> registered.
		/// </summary>
		public BunitHtmlParser()
			: this(Configuration.Default.WithCss().With(new HtmlComparer())) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="BunitHtmlParser"/> class
		/// with a AngleSharp context that includes a the <paramref name="testRenderer"/> registered.
		/// </summary>
		public BunitHtmlParser(ITestRenderer testRenderer, HtmlComparer htmlComparer, TestContextBase testContext)
			: this(Configuration.Default.WithCss()
				  .With(testRenderer ?? throw new ArgumentNullException(nameof(testRenderer)))
				  .With(htmlComparer ?? throw new ArgumentNullException(nameof(htmlComparer)))
				  .With(testContext ?? throw new ArgumentNullException(nameof(testContext))))
		{ }

		private BunitHtmlParser(IConfiguration angleSharpConfiguration)
		{
			var config = angleSharpConfiguration.With(this);
			context = BrowsingContext.New(config);
			htmlParser = context.GetService<IHtmlParser>();
		}

		/// <summary>
		/// Parses a markup HTML string using AngleSharps HTML5 parser.
		/// </summary>
		/// <param name="markup">The markup to parse.</param>
		/// <returns>The <see cref="INodeList"/>.</returns>
		public INodeList Parse(string markup)
		{
			if (markup is null)
				throw new ArgumentNullException(nameof(markup));
			var (ctx, matchedElement) = GetParseContextAsync(markup).GetAwaiter().GetResult();

			return ctx is null && matchedElement is not null
				? ParseSpecial(markup, matchedElement)
				: htmlParser.ParseFragment(markup, ctx);
		}

		private INodeList ParseSpecial(string markup, string matchedElement)
		{
			var doc = htmlParser.ParseDocument(markup);

			return matchedElement switch
			{
				"HTML" => new SingleNodeNodeList(doc.Body.ParentElement),
				"HEAD" => new SingleNodeNodeList(doc.Head),
				"BODY" => new SingleNodeNodeList(doc.Body),
				_ => throw new InvalidOperationException($"{matchedElement} should not be parsed by {nameof(ParseSpecial)}."),
			};
		}

		private async Task<(IElement? Context, string? MatchedElement)> GetParseContextAsync(string markup)
		{
			var document = await GetNewDocumentAsync().ConfigureAwait(false);
			var startIndex = markup.IndexOfFirstNonWhitespaceChar();

			// verify that first non-whitespace characters is a '<'
			if (markup.Length > 0 && markup[startIndex].IsTagStart())
			{
				return GetParseContextFromTag(markup, startIndex, document);
			}

			return (Context: document.Body, MatchedElement: null);
		}

		private static (IElement? Context, string? MatchedElement) GetParseContextFromTag(string markup, int startIndex, IDocument document)
		{
			IElement? result = null;

			if (markup.StartsWithElements(TableSubElements, startIndex, out var matchedElement))
			{
				result = CreateTable();
			}
			else if (markup.StartsWithElements(TrSubElements, startIndex, out matchedElement))
			{
				result = CreateTable().AppendElement(document.CreateElement("tr"));
			}
			else if (markup.StartsWithElement(TbodySubElements, startIndex))
			{
				result = CreateTable().AppendElement(document.CreateElement("tbody"));
				matchedElement = TbodySubElements;
			}
			else if (markup.StartsWithElement(ColgroupSubElement, startIndex))
			{
				result = CreateTable().AppendElement(document.CreateElement("colgroup"));
				matchedElement = ColgroupSubElement;
			}
			else if (markup.StartsWithElements(SpecialHtmlElements, startIndex, out matchedElement))
			{
				// default case, nothing to do.
			}
			else
			{
				result = document.Body;
			}

			return (Context: result, MatchedElement: matchedElement);

			IElement CreateTable() => document.Body.AppendElement(document.CreateElement("table"));
		}

		private async Task<IDocument> GetNewDocumentAsync()
		{
			var result = await context.OpenNewAsync().ConfigureAwait(false);
			documents.Add(result);
			return result;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			context.Dispose();
			foreach (var doc in documents)
			{
				doc.Dispose();
			}
		}

		private sealed class SingleNodeNodeList : INodeList, IReadOnlyList<INode>
		{
			private readonly INode node;

			public INode this[int index]
			{
				get
				{
					if (index != 0)
						throw new IndexOutOfRangeException();

					return node;
				}
			}

			public int Length { get; } = 1;

			public int Count { get; } = 1;

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
