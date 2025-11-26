using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal sealed class LabelTextUsingForAttributeStrategy : ILabelTextQueryStrategy
{
	public IEnumerable<IElement> FindElements(IRenderedComponent<IComponent> renderedComponent, string labelText, ByLabelTextOptions options)
	{
		var matchingLabels = renderedComponent.Nodes.TryQuerySelectorAll("label")
			.Where(l => l.TextContent.Trim().Equals(labelText, options.ComparisonType));

		foreach (var matchingLabel in matchingLabels)
		{
			var forAttribute = matchingLabel.GetAttribute("for");
			if (string.IsNullOrEmpty(forAttribute))
				continue;

			var matchingElement = renderedComponent.Nodes.TryQuerySelector($"#{forAttribute}");
			if (matchingElement is not null)
				yield return matchingElement.WrapUsing(new ByLabelTextElementFactory(renderedComponent, labelText, options));
		}
	}
}
