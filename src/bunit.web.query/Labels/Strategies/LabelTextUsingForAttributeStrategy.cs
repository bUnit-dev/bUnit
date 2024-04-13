using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal sealed class LabelTextUsingForAttributeStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedComponent<IComponent> renderedComponent, string labelText, ByLabelTextOptions options)
	{
		var matchingLabel = renderedComponent.Nodes.TryQuerySelectorAll("label")
			.SingleOrDefault(l => l.TextContent.Trim().Equals(labelText, options.ComparisonType));

		if (matchingLabel is null)
			return null;

		var matchingElement = renderedComponent.Nodes.TryQuerySelector($"#{matchingLabel.GetAttribute("for")}");

		if (matchingElement is null)
			return null;

		return matchingElement.WrapUsing(new ByLabelTextElementFactory(renderedComponent, labelText, options));
	}
}
