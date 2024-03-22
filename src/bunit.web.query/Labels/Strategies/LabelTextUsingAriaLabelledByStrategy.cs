using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal sealed class LabelTextUsingAriaLabelledByStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(RenderedFragment renderedFragment, string labelText, ByLabelTextOptions options)
	{
		var elementsWithAriaLabelledBy = renderedFragment.Nodes.TryQuerySelectorAll("[aria-labelledby]");

		foreach (var element in elementsWithAriaLabelledBy)
		{
			var labelElement = renderedFragment.Nodes.TryQuerySelector($"#{element.GetAttribute("aria-labelledby")}");
			if (labelElement is not null && labelElement.GetInnerText().Equals(labelText, options.ComparisonType))
				return element.WrapUsing(new ByLabelTextElementFactory(renderedFragment, labelText, options));
		}

		return null;
	}
}
