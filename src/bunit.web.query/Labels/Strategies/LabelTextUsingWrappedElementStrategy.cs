using AngleSharp.Dom;

namespace Bunit;

internal class LabelTextUsingWrappedElementStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedFragment renderedFragment, string labelText)
	{
		var labels = renderedFragment.FindAll("label");
		var matchingLabel = labels.SingleOrDefault(l => l.InnerHtml.StartsWith(labelText));

		return matchingLabel?
			.Children
			.SingleOrDefault(n => n.IsHtmlElementThatCanHaveALabel());
	}
}
