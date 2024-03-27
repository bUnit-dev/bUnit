using System;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Xunit;

namespace Bunit.Rendering;

public class BunitHtmlParserTest
{
	private static readonly BunitHtmlParser Parser = new();

	/// <summary>
	/// All HTML5 elements according to https://developer.mozilla.org/en-US/docs/Web/HTML/Element.
	/// </summary>
	public static readonly TheoryData<string> BodyHtmlElements = new TheoryData<string>
	{
		"base", "link", "meta", "style", "title",
		"address", "article", "aside", "footer", "header", "h1", "h2", "h3", "h4", "h5", "h6", "hgroup", "main", "nav", "section",
		"blockquote", "dd", "div", "dl", "dt", "figcaption", "figure", "hr", "li", "ol", "p", "pre", "ul",
		"a", "abbr", "b", "bdi", "bdo", "br", "cite", "code", "data", "dfn", "em", "i", "kbd", "mark", "q", "rb", "rp", "rt", "rtc", "ruby", "s", "samp", "small", "span", "strong", "sub", "sup", "time", "u", "var", "wbr",
		"area", "audio", "img", "map", "track", "video", "svg",
		"embed", "iframe", "object", "param", "picture", "source",
		"canvas", "noscript", "script",
		"del", "ins",
		"caption", "col", "colgroup", "table", "tbody", "td", "tfoot", "th", "thead", "tr",
		"button", "datalist", "fieldset", "form", "input", "label", "legend", "meter", "optgroup", "option", "output", "progress", "select", "textarea",
		"details", "dialog", "menu", "summary",
		"slot", "template",
		"acronym", "applet", "basefont", "bgsound", "big", "blink", "center", "command", "content", "dir", "element", "font", "keygen", "listing", "marquee", "menuitem", "multicol", "nextid", "nobr", "noembed", "noframes", "plaintext", "shadow", "spacer", "strike", "tt", "xmp",
		// "frame","frameset","image","isindex" // not supported
	};

	public static readonly IEnumerable<object[]> BodyHtmlAndSpecialElements =
		TheoryDataExtensions.AddRange(BodyHtmlElements.Clone(), "html", "head", "body");

	public static readonly TheoryData<string> SvgElements = new TheoryData<string>
	{
		"animate", "animateMotion", "animateTransform",
		"circle", "clipPath",
		"defs", "desc", "discard",
		"ellipse",
		"feBlend", "feColorMatrix", "feComponentTransfer", "feComposite", "feConvolveMatrix", "feDiffuseLighting", "feDisplacementMap", "feDistantLight", "feDropShadow", "feFlood", "feFuncA", "feFuncB", "feFuncG", "feFuncR", "feGaussianBlur", "feImage", "feMerge", "feMergeNode", "feMorphology", "feOffset", "fePointLight", "feSpecularLighting", "feSpotLight", "feTile", "feTurbulence", "filter", "foreignObject",
		"g",
		"hatch", "hatchpath",
		"line", "linearGradient",
		"marker", "mask", "metadata", "mpath",
		"path", "pattern", "polygon", "polyline",
		"radialGradient", "rect",
		"set", "stop", "switch", "symbol",
		"text", "textPath", "tspan",
		"use",
		"view"
		// "a", "image", "script", "svg", "title", "style" removed since they overlap with HTML elements
	};

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

	[Theory(DisplayName = "Parse() parses <TAG id=TAG>")]
	[MemberData(nameof(BodyHtmlAndSpecialElements))]
	public void Test001(string elementName)
	{
		var actual = Parser.Parse($@"<{elementName} id=""{elementName}"">").ToList();

		VerifyElementParsedWithId(elementName, actual);
	}

	[Theory(DisplayName = "Parse() parses <TAG> with whitespace before")]
	[MemberData(nameof(BodyHtmlElements))]
	public void Test002(string elementName)
	{
		var actual = Parser.Parse($@" {'\t'}{'\n'}{'\r'}{Environment.NewLine} <{elementName} id=""{elementName}"">").ToList();

		VerifyElementParsedWithId(elementName, actual);
	}

	[Theory(DisplayName = "Parse() parses <TAG>")]
	[MemberData(nameof(BodyHtmlElements))]
	public void Test003(string elementName)
	{
		var actual = Parser.Parse($@"<{elementName}>").ToList();

		actual.ShouldHaveSingleItem()
			.ShouldBeAssignableTo<IElement>()
			.NodeName.ShouldBe(elementName, StringCompareShould.IgnoreCase);
	}

	[Theory(DisplayName = "Parse() parses <TAG/>")]
	[MemberData(nameof(BodyHtmlElements))]
	public void Test004(string elementName)
	{
		var actual = Parser.Parse($@"<{elementName}/>").ToList();

		actual.ShouldHaveSingleItem()
			.ShouldBeAssignableTo<IElement>()
			.NodeName.ShouldBe(elementName, StringCompareShould.IgnoreCase);
	}

	[Theory(DisplayName = "Parse() parses <TAG ...")]
	[MemberData(nameof(BodyHtmlElements))]
	public void Test005(string elementName)
	{
		var input = $@" <{elementName} foo bar {'\t'}{Environment.NewLine}";
		var actual = Parser.Parse(input).ToList();

		actual.ShouldHaveSingleItem()
			.ShouldBeAssignableTo<IText>()
			.TextContent.ShouldBe(" ");
	}

	[Theory(DisplayName = "Parse() parses <TAG .../")]
	[MemberData(nameof(BodyHtmlElements))]
	public void Test006(string elementName)
	{
		var input = $@" <{elementName}/ ";
		var actual = Parser.Parse(input).ToList();

		actual.ShouldHaveSingleItem()
			.ShouldBeAssignableTo<IText>()
			.TextContent.ShouldBe(" ");
	}

	[Theory(DisplayName = "Parse() parses SVG element <TAG>")]
	[MemberData(nameof(SvgElements))]
	public void Test020(string elementName)
	{
		var actual = Parser.Parse($@"<{elementName}>").ToList();

		// SVG elements that are parsed outside of a SVG parent element will
		// be returned as an IHtmlUnknownElement, which is precise enough for our need.
		actual.ShouldHaveSingleItem()
			.ShouldBeAssignableTo<IHtmlUnknownElement>()
			.NodeName.ShouldBe(elementName, StringCompareShould.IgnoreCase);
	}

	[Fact(DisplayName = "Parse with full HTML markup")]
	public void Test021()
	{
		var actual = Parser.Parse("""
			<!DOCTYPE html>
			<html lang="en">
			  <head>
			    <meta charset="utf-8">
			    <title>title</title>
			    <link rel="stylesheet" href="style.css">
			    <script src="script.js"></script>
			  </head>
			  <body>
			    <!-- page content -->
			  </body>
			</html>
			""");

		actual.Length.ShouldBe(2);
		actual[0].ShouldBeAssignableTo<IDocumentType>();
		actual[1].ShouldBeAssignableTo<IHtmlHtmlElement>();
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
