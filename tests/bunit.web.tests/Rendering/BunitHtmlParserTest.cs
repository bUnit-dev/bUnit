using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using Shouldly;
using Xunit;

namespace Bunit.Rendering
{
	public class BunitHtmlParserTest
	{
		private static readonly BunitHtmlParser Parser = new();

		/// <summary>
		/// All HTML5 elements according to https://developer.mozilla.org/en-US/docs/Web/HTML/Element.
		/// </summary>
		public static readonly IEnumerable<string[]> BodyHtmlElements = new[]
		{
			"base", "link", "meta", "style", "title",
			"address", "article", "aside", "footer", "header", "h1", "h2", "h3", "h4", "h5", "h6", "hgroup", "main", "nav", "section",
			"blockquote", "dd", "div", "dl", "dt", "figcaption", "figure", "hr", "li", "ol", "p", "pre", "ul",
			"a", "abbr", "b", "bdi", "bdo", "br", "cite", "code", "data", "dfn", "em", "i", "kbd", "mark", "q", "rb", "rp", "rt", "rtc", "ruby", "s", "samp", "small", "span", "strong", "sub", "sup", "time", "u", "var", "wbr",
			"area", "audio", "img", "map", "track", "video",
			"embed", "iframe", "object", "param", "picture", "source",
			"canvas", "noscript", "script",
			"del", "ins",
			"caption", "col", "colgroup", "table", "tbody", "td", "tfoot", "th", "thead", "tr",
			"button", "datalist", "fieldset", "form", "input", "label", "legend", "meter", "optgroup", "option", "output", "progress", "select", "textarea",
			"details", "dialog", "menu", "summary",
			"slot", "template",
			"acronym", "applet", "basefont", "bgsound", "big", "blink", "center", "command", "content", "dir", "element", "font", "keygen", "listing", "marquee", "menuitem", "multicol", "nextid", "nobr", "noembed", "noframes", "plaintext", "shadow", "spacer", "strike", "tt", "xmp",
			// "frame","frameset","image","isindex" // not supported
		}.Select(x => new[] { x });

		public static readonly IEnumerable<string[]> BodyHtmlAndSpecialElements = BodyHtmlElements.Concat(
			new[] { "html", "head", "body", }.Select(x => new[] { x }));

		[Fact(DisplayName = "Parse() called with null")]
		public void ParseCalledWithNull()
		{
			Should.Throw<ArgumentNullException>(() => Parser.Parse(null!));
		}

		[Theory(DisplayName = "Parse() called with text only")]
		[InlineData("  ")]
		[InlineData("FOO BAR")]
		public void ParseWithWhitespaceOnly(string text)
		{
			var actual = Parser.Parse(text);

			actual.ShouldHaveSingleItem().TextContent.ShouldBe(text);
		}

		[Theory(DisplayName = "Parse() passed <TAG id=TAG>")]
		[MemberData(nameof(BodyHtmlAndSpecialElements))]
		public void Test001(string elementName)
		{
			var actual = Parser.Parse($@"<{elementName} id=""{elementName}"">").ToList();

			VerifyElementParsedWithId(elementName, actual);
		}

		[Theory(DisplayName = "Parse() passed <TAG> with whitespace before")]
		[MemberData(nameof(BodyHtmlElements))]
		public void Test002(string elementName)
		{
			var actual = Parser.Parse($@" {'\t'}{'\n'}{'\r'}{Environment.NewLine} <{elementName} id=""{elementName}"">").ToList();

			VerifyElementParsedWithId(elementName, actual);
		}

		[Theory(DisplayName = "Parse() passed <TAG>")]
		[MemberData(nameof(BodyHtmlElements))]
		public void Test003(string elementName)
		{
			var actual = Parser.Parse($@"<{elementName}>").ToList();

			actual.ShouldHaveSingleItem()
				.ShouldBeAssignableTo<IElement>()
				.NodeName.ShouldBe(elementName, StringCompareShould.IgnoreCase);
		}

		[Theory(DisplayName = "Parse() passed <TAG/>")]
		[MemberData(nameof(BodyHtmlElements))]
		public void Test004(string elementName)
		{
			var actual = Parser.Parse($@"<{elementName}/>").ToList();

			actual.ShouldHaveSingleItem()
				.ShouldBeAssignableTo<IElement>()
				.NodeName.ShouldBe(elementName, StringCompareShould.IgnoreCase);
		}

		[Theory(DisplayName = "Parse() passed <TAG ...")]
		[MemberData(nameof(BodyHtmlElements))]
		public void Test005(string elementName)
		{
			var input = $@" <{elementName} foo bar {'\t'}{Environment.NewLine}";
			var actual = Parser.Parse(input).ToList();

			actual.ShouldHaveSingleItem()
				.TextContent.ShouldBe(" ");
		}

		[Theory(DisplayName = "Parse() passed <TAG .../")]
		[MemberData(nameof(BodyHtmlElements))]
		public void Test006(string elementName)
		{
			var input = $@" <{elementName}/ ";
			var actual = Parser.Parse(input).ToList();

			actual.ShouldHaveSingleItem()
				.TextContent.ShouldBe(" ");
		}

		private static void VerifyElementParsedWithId(string expectedElementName, List<INode> actual)
		{
			var elm = actual.OfType<IElement>()
				.ShouldHaveSingleItem()
				.ShouldBeAssignableTo<IElement>();

			elm.ShouldSatisfyAllConditions(
				() => elm.NodeName.ShouldBe(expectedElementName, StringCompareShould.IgnoreCase),
				() => elm.Id.ShouldBe(expectedElementName));
		}
	}
}
