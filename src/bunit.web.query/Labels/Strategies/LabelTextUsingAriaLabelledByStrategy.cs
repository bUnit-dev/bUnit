using AngleSharp.Dom;

namespace Bunit;

internal class LabelTextUsingAriaLabelledByStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedFragment renderedFragment, string labelText)
	{
		var elementsWithAriaLabelledBy = renderedFragment.FindAll("[aria-labelledby]");

		foreach (var element in elementsWithAriaLabelledBy)
		{
			var labelElements = renderedFragment.FindAll($"#{element.GetAttribute("aria-labelledby")}");
			if (labelElements.Count > 0 && labelElements[0].GetInnerText() == labelText)
				return element;
		}

		return null;
	}
}
