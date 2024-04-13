using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal sealed class LabelTextUsingAriaLabelledByStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedComponent<IComponent> renderedComponent, string labelText, ByLabelTextOptions options)
	{
		var elementsWithAriaLabelledBy = renderedComponent.Nodes.TryQuerySelectorAll("[aria-labelledby]");

		foreach (var element in elementsWithAriaLabelledBy)
		{
			var labelElement = renderedComponent.Nodes.TryQuerySelector($"#{element.GetAttribute("aria-labelledby")}");
			if (labelElement is not null && labelElement.GetInnerText().Equals(labelText, options.ComparisonType))
				return element.WrapUsing(new ByLabelTextElementFactory(renderedComponent, labelText, options));
		}

		return null;
	}
}
