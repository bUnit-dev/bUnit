using AngleSharp.Css.Dom;
using AngleSharp.Css.Parser;
using AngleSharp.Dom;

namespace Bunit;

internal static class NodeListExtensions
{
	internal static IElement? TryQuerySelector(this INodeList nodes, string cssSelector)
	{
		if (nodes.Length == 0 ||
		    nodes[0].Owner?.Context.GetService<ICssSelectorParser>() is not ICssSelectorParser cssParser)
			return null;

		if (cssParser.ParseSelector(cssSelector) is not ISelector selector)
			return null;

		return nodes.QuerySelector(selector);
	}

	internal static IEnumerable<IElement> TryQuerySelectorAll(this INodeList nodes, string cssSelector)
	{
		if (nodes.Length == 0 ||
		    nodes[0].Owner?.Context.GetService<ICssSelectorParser>() is not ICssSelectorParser cssParser)
			return Enumerable.Empty<IElement>();

		if (cssParser.ParseSelector(cssSelector) is not ISelector selector)
			return Enumerable.Empty<IElement>();

		return nodes.QuerySelectorAll(selector);
	}
}
