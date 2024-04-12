using System.Collections;
using System.Diagnostics;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Bunit.Diffing;

namespace Bunit.Rendering;

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
	private static readonly string[] SpecialHtmlElements = { "HTML", "HEAD", "BODY", "!DOCTYPE" };

	private readonly IBrowsingContext context;
	private readonly HtmlParser htmlParser;
	private readonly List<IDocument> documents = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitHtmlParser"/> class
	/// with a AngleSharp context without a <see cref="BunitRenderer"/> registered.
	/// </summary>
	public BunitHtmlParser()
		: this(Configuration.Default.WithCss().With(new HtmlComparer())) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitHtmlParser"/> class
	/// with a AngleSharp context registered.
	/// </summary>
	public BunitHtmlParser(HtmlComparer htmlComparer, TestContext testContext)
		: this(Configuration.Default.WithCss()
			  .With(htmlComparer ?? throw new ArgumentNullException(nameof(htmlComparer)))
			  .With(testContext ?? throw new ArgumentNullException(nameof(testContext)))
			  .With(testContext.Renderer))
	{ }

	private BunitHtmlParser(IConfiguration angleSharpConfiguration)
	{
		var config = angleSharpConfiguration.With(this);
		context = BrowsingContext.New(config);
		var parseOptions = new HtmlParserOptions
		{
			IsKeepingSourceReferences = true,
		};
		htmlParser = new HtmlParser(parseOptions, context);
	}

	/// <summary>
	/// Parses a markup HTML string using AngleSharps HTML5 parser.
	/// </summary>
	/// <param name="markup">The markup to parse.</param>
	/// <returns>The <see cref="INodeList"/>.</returns>
	public INodeList Parse([StringSyntax("Html")]string markup)
	{
		ArgumentNullException.ThrowIfNull(markup);

		var document = GetNewDocumentAsync().GetAwaiter().GetResult();

		var (ctx, matchedElement) = GetParseContext(markup, document);

		return ctx is null && matchedElement is not null
			? ParseSpecial(markup, matchedElement)
			: htmlParser.ParseFragment(markup, ctx!);
	}

	private SingleNodeNodeList ParseSpecial(string markup, string matchedElement)
	{
		var doc = htmlParser.ParseDocument(markup);

		return matchedElement switch
		{
			"HTML" => new SingleNodeNodeList(doc.Body?.ParentElement),
			"HEAD" => new SingleNodeNodeList(doc.Head),
			"BODY" => new SingleNodeNodeList(doc.Body),
			"!DOCTYPE" => doc.ChildNodes,
			_ => throw new InvalidOperationException($"{matchedElement} should not be parsed by {nameof(ParseSpecial)}."),
		};
	}

	private static (IElement? Context, string? MatchedElement) GetParseContext(
		ReadOnlySpan<char> markup,
		IDocument document)
	{
		var startIndex = markup.IndexOfFirstNonWhitespaceChar();

		// verify that first non-whitespace characters is a '<'
		if (markup.Length > 0 && markup[startIndex].IsTagStart())
		{
			return GetParseContextFromTag(markup, startIndex, document);
		}

		return (Context: document.Body, MatchedElement: null);
	}

	private static (IElement? Context, string? MatchedElement) GetParseContextFromTag(
		ReadOnlySpan<char> markup,
		int startIndex,
		IDocument document)
	{
		Debug.Assert(document.Body is not null, "Body of the document should never be null at this point.");

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

		public int Length => 1;

		public int Count => 1;

		public SingleNodeNodeList(INode? node) => this.node = node ?? throw new ArgumentNullException(nameof(node));

		public IEnumerator<INode> GetEnumerator()
		{
			yield return node;
		}

		public void ToHtml(TextWriter writer, IMarkupFormatter formatter) => node.ToHtml(writer, formatter);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
