using AngleSharp.Dom;

namespace Bunit.Labels.Strategies;

internal class LabelTextUsingAriaLabelStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedFragment renderedFragment, string labelText)
	{
		var element = renderedFragment.Nodes.QuerySelector($"[aria-label='{labelText}']");

		if (element is null)
			return null;

		return ElementWrapperFactory.CreateByLabelText(element, renderedFragment, labelText);
	}
}
