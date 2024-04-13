using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal sealed class LabelTextUsingWrappedElementStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedComponent<IComponent> renderedComponent, string labelText, ByLabelTextOptions options)
	{
		var matchingLabel = renderedComponent.Nodes.TryQuerySelectorAll("label")
			.SingleOrDefault(l => l.GetInnerText().Trim().StartsWith(labelText, options.ComparisonType));

		var matchingElement = matchingLabel?
			.Children
			.SingleOrDefault(n => n.IsHtmlElementThatCanHaveALabel());

		return matchingElement?.WrapUsing(new ByLabelTextElementFactory(renderedComponent, labelText, options));
	}
}
