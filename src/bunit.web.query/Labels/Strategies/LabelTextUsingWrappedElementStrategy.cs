using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal class LabelTextUsingWrappedElementStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedFragment renderedFragment, string labelText)
	{
		var labels = renderedFragment.Nodes.QuerySelectorAll("label");
		var matchingLabel = labels.SingleOrDefault(l => l.InnerHtml.StartsWith(labelText));

		var matchingElement = matchingLabel?
			.Children
			.SingleOrDefault(n => n.IsHtmlElementThatCanHaveALabel());

		if (matchingElement is null)
			return null;

		return matchingElement.WrapUsing(new ByLabelTextElementFactory(renderedFragment, labelText));
	}
}
