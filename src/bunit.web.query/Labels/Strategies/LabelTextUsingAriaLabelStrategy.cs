using AngleSharp.Dom;

namespace Bunit;

internal class LabelTextUsingAriaLabelStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedFragment renderedFragment, string labelText)
	{
		var results = renderedFragment.FindAll($"[aria-label='{labelText}']");

		return results.Count == 0
			? null
			: results[0];
	}
}
