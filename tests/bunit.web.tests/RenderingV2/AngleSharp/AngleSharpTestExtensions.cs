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

	public static INodeList ToNodes(this string sourceCode, IConfiguration configuration = null)
	{
		var doc = ToHtmlDocument(sourceCode, configuration);
		return doc.Body.ChildNodes;
	}

	public static INodeList ToNodes(this string sourceCode, IElement contextElement, IConfiguration configuration = null)
	{
		var context = BrowsingContext.New(configuration ?? Configuration.Default);
		var htmlParser = context.GetService<IHtmlParser>();
		return htmlParser.ParseFragment(sourceCode, contextElement);
	}

	public static INodeList ToNodes(this string sourceCode, string contextElement, IConfiguration configuration = null)
	{
		var doc = string.Empty.ToHtmlDocument();
		var element = doc.CreateElement(contextElement);
		sourceCode.ToNodes(element, configuration);
		return element.ChildNodes;
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
		element.AppendNodes(sourceCode.ToNodes(element, configuration));
		return element;
	}
}
