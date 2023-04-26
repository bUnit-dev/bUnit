using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Bunit.Diffing;

/// <summary>
/// Blazor Diffing Helpers.
/// </summary>
public static class BlazorDiffingHelpers
{
	/// <summary>
	/// Represents a diffing filter that removes all special Blazor attributes added by the /<see cref="Htmlizer"/>.
	/// </summary>
	public static FilterDecision BlazorAttributeFilter(in AttributeComparisonSource attrSource, FilterDecision currentDecision)
	{
		if (currentDecision == FilterDecision.Exclude)
			return currentDecision;

		if (Htmlizer.IsBlazorAttribute(attrSource.Attribute.Name))
			return FilterDecision.Exclude;

		return currentDecision;
	}

	// Based on the forward searching node matcher in AngleSharp.Diffing,
	// but this also tries to match nodes that are IHtmlUnknownElement
	// but have the same NodeType and NodeName. This allow us to
	// deal with SVG elements that are parsed without the proper context,
	// i.e. inside an <SVG> element.
	internal static IEnumerable<Comparison> UnknownElementMatch(
		IDiffContext context,
		SourceCollection controlSources,
		SourceCollection testSources)
	{
		var lastMatchedTestNodeIndex = -1;
		foreach (var control in controlSources.GetUnmatched().Where(c => c.Node is IElement))
		{
			var comparison = TryFindMatchingNodes(control, testSources, lastMatchedTestNodeIndex + 1);
			if (comparison.HasValue)
			{
				yield return comparison.Value;
				lastMatchedTestNodeIndex = comparison.Value.Test.Index;
			}
		}
	}

	private static Comparison? TryFindMatchingNodes(in ComparisonSource control, SourceCollection testSources, int startIndex)
	{
		foreach (var test in testSources.GetUnmatched(startIndex).Where(t => t.Node is IElement))
		{
			if (control.Node is IHtmlUnknownElement
				|| test.Node is IHtmlUnknownElement
				&& control.Node.NodeType == test.Node.NodeType
				&& control.Node.NodeName.Equals(test.Node.NodeName, StringComparison.OrdinalIgnoreCase))
			{
				return new Comparison(control, test);
			}
		}
		return null;
	}
}
