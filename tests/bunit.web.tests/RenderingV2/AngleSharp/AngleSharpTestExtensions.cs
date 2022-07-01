using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace Bunit.RenderingV2.AngleSharp;

internal static class AngleSharpTestExtensions
{
	public static IDocument ToHtmlDocument(this string sourceCode, IConfiguration configuration = null)
	{
		var context = BrowsingContext.New(configuration ?? Configuration.Default);
		var htmlParser = context.GetService<IHtmlParser>();
		return htmlParser.ParseDocument(sourceCode);
	}

	public static IDocument ToHtmlDocument(this Stream content, IConfiguration configuration = null)
	{
		var context = BrowsingContext.New(configuration);
		var htmlParser = context.GetService<IHtmlParser>();
		return htmlParser.ParseDocument(content);
	}

	public static INodeList ToHtmlFragment(this string sourceCode, IElement contextElement = null, IConfiguration configuration = null)
	{
		var context = BrowsingContext.New(configuration);
		var htmlParser = context.GetService<IHtmlParser>();
		return htmlParser.ParseFragment(sourceCode, contextElement);
	}

	public static INodeList ToHtmlFragment(this string sourceCode, string contextElement, IConfiguration configuration = null)
	{
		var doc = string.Empty.ToHtmlDocument();
		var element = doc.CreateElement(contextElement);
		return sourceCode.ToHtmlFragment(element, configuration);
	}

	public static IElement ToElement(this string elementName, IConfiguration configuration = null)
	{
		var doc = string.Empty.ToHtmlDocument();
		return doc.CreateElement(elementName);
	}

	public static IElement ToNodesWithParent(this string sourceCode, string parentElement, IConfiguration configuration = null)
	{
		var doc = string.Empty.ToHtmlDocument();
		var element = doc.CreateElement(parentElement);
		element.AppendNodes(sourceCode.ToHtmlFragment(element, configuration));
		return element;
	}
}
