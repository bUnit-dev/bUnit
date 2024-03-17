using AngleSharp.Dom;
using Bunit.Labels.Strategies;

namespace Bunit;

/// <summary>
/// Extension methods for querying IRenderedFragments by Label
/// </summary>
public static class LabelQueryExtensions
{
	private static readonly IReadOnlyList<ILabelTextQueryStrategy> LabelTextQueryStrategies =
	[
		// This is intentionally in the order of most likely to minimize strategies tried to find the label
		new LabelTextUsingForAttributeStrategy(),
		new LabelTextUsingAriaLabelStrategy(),
		new LabelTextUsingWrappedElementStrategy(),
		new LabelTextUsingAriaLabelledByStrategy(),
	];

	/// <summary>
	/// Returns the first element (i.e. an input, select, textarea, etc. element) associated with the given label text.
	/// </summary>
	/// <param name="renderedFragment">The rendered fragment to search.</param>
	/// <param name="labelText">The text of the label to search (i.e. the InnerText of the Label, such as "First Name" for a `<label>First Name</label>`)</param>
	/// <param name="configureOptions">Method used to override the default behavior of FindByLabelText.</param>
	public static IElement FindByLabelText(this IRenderedFragment renderedFragment, string labelText, Action<ByLabelTextOptions>? configureOptions = null)
	{
		var options = ByLabelTextOptions.Default;
		if (configureOptions is not null)
		{
			options = options with { };
			configureOptions.Invoke(options);
		}

		return FindByLabelTextInternal(renderedFragment, labelText, options) ?? throw new LabelNotFoundException(labelText);
	}

	internal static IElement? FindByLabelTextInternal(this IRenderedFragment renderedFragment, string labelText, ByLabelTextOptions options)
	{
		foreach (var strategy in LabelTextQueryStrategies)
		{
			var element = strategy.FindElement(renderedFragment, labelText, options);

			if (element is not null)
				return element;
		}

		return null;
	}
}
