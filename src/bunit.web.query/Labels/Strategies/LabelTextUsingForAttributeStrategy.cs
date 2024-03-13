using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal sealed class LabelTextUsingForAttributeStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedFragment renderedFragment, string labelText, ByLabelTextOptions options)
	{
		var matchingLabel = renderedFragment.Nodes.TryQuerySelectorAll("label")
			.SingleOrDefault(l => l.TextContent.Equals(labelText, options.ComparisonType));

		if (matchingLabel is null)
			return null;

		var matchingElement = renderedFragment.Nodes.TryQuerySelector($"#{matchingLabel.GetAttribute("for")}");

		if (matchingElement is null)
			return null;

		return matchingElement.WrapUsing(new ByLabelTextElementFactory(renderedFragment, labelText, options));
	}
}
