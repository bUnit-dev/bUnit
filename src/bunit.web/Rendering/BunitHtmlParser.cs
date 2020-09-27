using System;
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
		private static readonly string[] TABLE_SUB_ELEMENTS = { "CAPTION", "COLGROUP", "TBODY", "TFOOT", "THEAD", };
		private const string TBODY_SUB_ELEMENT = "TR";
		private static readonly string[] TR_SUB_ELEMENTS = { "TD", "TH" };
		private const string COLGROUP_SUB_ELEMENT = "COL";
		//private static readonly string[] HTML_SUB_ELEMENTS = { "HEAD", "BODY" };

		private readonly IBrowsingContext _context;
		private readonly IHtmlParser _htmlParser;
		private readonly IDocument _document;
		private readonly IElement _bodyContext;
		private readonly IElement _tableContext;
		private readonly IElement _tbodyContext;
		private readonly IElement _trContext;
		private readonly IElement _colgroupContext;

		/// <summary>
		/// Creates an instance of the parser with a AngleSharp context 
		/// without a <see cref="TestRenderer"/> registered.
		/// </summary>
		public BunitHtmlParser() : this(Configuration.Default.WithCss().With(new HtmlComparer()))
		{
		}

		/// <summary>
		/// Creates an instance of the parser with a AngleSharp context 
		/// with the <paramref name="testRenderer"/> registered.
		/// </summary>
		public BunitHtmlParser(ITestRenderer testRenderer, HtmlComparer htmlComparer)
			: this(Configuration.Default.WithCss().With(testRenderer).With(htmlComparer))
		{
		}

		private BunitHtmlParser(IConfiguration angleSharpConfiguration)
		{
			var config = angleSharpConfiguration.With(this);
			_context = BrowsingContext.New(config);
			_htmlParser = _context.GetService<IHtmlParser>();
			_document = _context.OpenNewAsync().Result;
			_bodyContext = _document.Body;
			_tableContext = _bodyContext.AppendElement(_document.CreateElement("table"));
			_tbodyContext = _tableContext.AppendElement(_document.CreateElement("tbody"));
			_trContext = _tbodyContext.AppendElement(_document.CreateElement("tr"));
			_colgroupContext = _tableContext.AppendElement(_document.CreateElement("colgroup"));
		}

		/// <summary>
		/// Parses a markup HTML string using AngleSharps HTML5 parser.
		/// </summary>
		/// <param name="markup">The markup to parse.</param>
		/// <returns>The <see cref="INodeList"/>.</returns>
		public INodeList Parse(string markup)
		{
			if (markup is null) throw new ArgumentNullException(nameof(markup));
			var parseContext = GetParseContext(markup);
			return _htmlParser.ParseFragment(markup, parseContext);
		}

		private IElement GetParseContext(string markup)
		{
			var startIndex = markup.IndexOfFirstNonWhitespaceChar();

			// verify that first non-whitespace characters is a '<'
			if (markup.Length > 0 && markup[startIndex].IsTagStart())
			{
				if (markup.StartsWithElements(TABLE_SUB_ELEMENTS, startIndex))
				{
					return _tableContext;
				}
				else if (markup.StartsWithElements(TR_SUB_ELEMENTS, startIndex))
				{
					return _trContext;
				}
				else if (markup.StartsWithElement(TBODY_SUB_ELEMENT, startIndex))
				{
					return _tbodyContext;
				}
				else if (markup.StartsWithElement(COLGROUP_SUB_ELEMENT, startIndex))
				{
					return _colgroupContext;
				}
			}

			return _bodyContext;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_document.Dispose();
			_context.Dispose();
		}
	}

	internal static class HtmlParserHelpers
	{
		internal static bool StartsWithElements(this string markup, string[] tags, int startIndex)
		{
			for (int i = 0; i < tags.Length; i++)
			{
				if (markup.StartsWithElement(tags[i], startIndex))
					return true;
			}
			return false;
		}

		internal static bool StartsWithElement(this string markup, string tag, int startIndex)
		{
			var matchesTag = tag.Length + 1 < markup.Length - startIndex;
			var charIndexAfterTag = tag.Length + startIndex + 1;

			if (matchesTag)
			{
				var charAfterTag = markup[charIndexAfterTag];
				matchesTag = char.IsWhiteSpace(charAfterTag) ||
							 charAfterTag == '>' ||
							 charAfterTag == '/';
			}

			// match characters in tag
			for (int i = 0; i < tag.Length && matchesTag; i++)
			{
				matchesTag = char.ToUpperInvariant(markup[startIndex + i + 1]) == tag[i];
			}

			// look for start tags end - '>'
			for (int i = charIndexAfterTag; i < markup.Length && matchesTag; i++)
			{
				if (markup[i] == '>') break;
			}

			return matchesTag;
		}

		internal static bool IsTagStart(this char c) => c == '<';

		internal static int IndexOfFirstNonWhitespaceChar(this string markup)
		{
			for (int i = 0; i < markup.Length; i++)
			{
				if (!char.IsWhiteSpace(markup, i))
					return i;
			}
			return 0;
		}
	}
}
