using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal class LabelTextUsingForAttributeStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedFragment renderedFragment, string labelText)
	{
		var labels = renderedFragment.Nodes.QuerySelectorAll("label");
		var matchingLabel = labels.SingleOrDefault(l => l.TextContent == labelText);

		if (matchingLabel is null)
			return null;

		var matchingElement = renderedFragment.Nodes.QuerySelector($"#{matchingLabel.GetAttribute("for")}");

		if (matchingElement is null)
			return null;

		return matchingElement.WrapUsing(new ByLabelTextElementFactory(renderedFragment, labelText));
	}
}
