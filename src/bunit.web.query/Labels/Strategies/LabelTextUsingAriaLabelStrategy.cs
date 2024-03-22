using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal sealed class LabelTextUsingAriaLabelStrategy : ILabelTextQueryStrategy
{
	public IElement? FindElement(RenderedFragment renderedFragment, string labelText, ByLabelTextOptions options)
	{
		var caseSensitivityQualifier = options.ComparisonType switch
		{
			StringComparison.OrdinalIgnoreCase => "i",
			StringComparison.InvariantCultureIgnoreCase => "i",
			StringComparison.CurrentCultureIgnoreCase => "i",
			_ => ""
		};

		var element = renderedFragment.Nodes.TryQuerySelector($"[aria-label='{labelText}'{caseSensitivityQualifier}]");

		if (element is null)
			return null;

		return element.WrapUsing(new ByLabelTextElementFactory(renderedFragment, labelText, options));
	}
}
