using AngleSharp.Diffing.Core;

namespace Bunit.Diffing
{
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
	}
}
