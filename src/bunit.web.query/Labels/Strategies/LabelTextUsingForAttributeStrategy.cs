using AngleSharp.Dom;

namespace Bunit;

internal class LabelTextUsingForAttributeStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(IRenderedFragment renderedFragment, string labelText)
	{
		var labels = renderedFragment.FindAll("label");
		var matchingLabel = labels.SingleOrDefault(l => l.TextContent == labelText);

		return matchingLabel == null
			? null
			: renderedFragment.Find($"#{matchingLabel.GetAttribute("for")}");
	}
}
