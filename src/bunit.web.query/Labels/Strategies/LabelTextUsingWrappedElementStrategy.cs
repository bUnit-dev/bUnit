using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal sealed class LabelTextUsingWrappedElementStrategy : ILabelTextQueryStrategy
{
	public IEnumerable<IElement> FindElements(IRenderedComponent<IComponent> renderedComponent, string labelText, ByLabelTextOptions options)
	{
		var matchingLabels = renderedComponent.Nodes.TryQuerySelectorAll("label")
			.Where(l => l.GetInnerText().Trim().StartsWith(labelText, options.ComparisonType));

		foreach (var matchingLabel in matchingLabels)
		{
			var matchingElements = matchingLabel.Children.Where(n => n.IsHtmlElementThatCanHaveALabel());
			foreach (var matchingElement in matchingElements)
			{
				yield return matchingElement.WrapUsing(new ByLabelTextElementFactory(renderedComponent, labelText, options));
			}
		}
	}
}
