using AngleSharp.Dom;

namespace Bunit;

public static class LabelQueryExtensions
{
	public static IElement FindByLabelText(this IRenderedFragment renderedFragment, string labelText)
	{
		try
		{
			// TODO: switch to strategy pattern?
			var element = FindByLabelTextWithForAttribute(renderedFragment, labelText);

			if (element != null)
				return element;

			element = FindByLabelTextWithWrappedElement(renderedFragment, labelText);
			if (element != null)
				return element;

			element = FindByLabelWithAriaLabelAttribute(renderedFragment, labelText);

			if (element != null)
				return element;
		}
		catch (DomException exception) when (exception.Message == "The string did not match the expected pattern.")
		{
			throw new ElementNotFoundException(labelText);
		}

		throw new ElementNotFoundException(labelText);

	}

	private static IElement? FindByLabelTextWithForAttribute(IRenderedFragment renderedFragment, string labelText)
	{
		var labels = renderedFragment.FindAll("label");
		var matchingLabel = labels.SingleOrDefault(l => l.TextContent == labelText);

		return matchingLabel == null
			? null
			: renderedFragment.Find($"#{matchingLabel.GetAttribute("for")}");
	}

	private static IElement? FindByLabelTextWithWrappedElement(IRenderedFragment renderedFragment, string labelText)
	{
		var labels = renderedFragment.FindAll("label");
		var matchingLabel = labels.SingleOrDefault(l => l.InnerHtml.StartsWith(labelText));

		return matchingLabel?
			.Children
			.SingleOrDefault(n => n.NodeName == "INPUT");
	}

	private static IElement? FindByLabelWithAriaLabelAttribute(IRenderedFragment renderedFragment, string labelText)
	{
		var results = renderedFragment.FindAll($"[aria-label='{labelText}']");

		return results.Count == 0
			? null
			: results[0];
	}
}
