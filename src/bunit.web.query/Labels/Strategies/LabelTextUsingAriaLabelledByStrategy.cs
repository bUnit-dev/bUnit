using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal class LabelTextUsingAriaLabelledByStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedFragment renderedFragment, string labelText)
	{
		var elementsWithAriaLabelledBy = renderedFragment.Nodes.QuerySelectorAll("[aria-labelledby]");

		foreach (var element in elementsWithAriaLabelledBy)
		{
			var labelElement = renderedFragment.Nodes.QuerySelector($"#{element.GetAttribute("aria-labelledby")}");
			if (labelElement.GetInnerText() == labelText)
				return element.WrapUsing(new ByLabelTextElementFactory(renderedFragment, labelText));
		}

		return null;
	}
}
