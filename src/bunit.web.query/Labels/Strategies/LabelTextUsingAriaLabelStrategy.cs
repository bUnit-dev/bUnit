using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Labels.Strategies;

internal sealed class LabelTextUsingAriaLabelStrategy : ILabelTextQueryStrategy
{
	public IEnumerable<IElement> FindElements(IRenderedComponent<IComponent> renderedComponent, string labelText, ByLabelTextOptions options)
	{
		var caseSensitivityQualifier = options.ComparisonType switch
		{
			StringComparison.OrdinalIgnoreCase => "i",
			StringComparison.InvariantCultureIgnoreCase => "i",
			StringComparison.CurrentCultureIgnoreCase => "i",
			_ => ""
		};

		var elements = renderedComponent.Nodes.TryQuerySelectorAll($"[aria-label='{labelText}'{caseSensitivityQualifier}]");

		foreach (var element in elements)
		{
			yield return element.WrapUsing(new ByLabelTextElementFactory(renderedComponent, labelText, options));
		}
	}
}
