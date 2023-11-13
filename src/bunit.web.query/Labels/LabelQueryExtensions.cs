using AngleSharp.Dom;
using Bunit.Labels.Strategies;

namespace Bunit;

public static class LabelQueryExtensions
{
	private static readonly List<ILabelTextQueryStrategy> LabelTextQueryStrategies = new()
	{
		// This is intentionally in the order of most likely to minimize strategies tried to find the label
		new LabelTextUsingForAttributeStrategy(),
		new LabelTextUsingAriaLabelStrategy(),
		new LabelTextUsingWrappedElementStrategy(),
		new LabelTextUsingAriaLabelledByStrategy(),
	};

	/// <summary>
	/// Returns the first element (i.e. an input, select, textarea, etc. element) associated with the given label text.
	/// </summary>
	/// <param name="renderedFragment">The rendered fragment to search.</param>
	/// <param name="labelText">The text of the label to search (i.e. the InnerText of the Label, such as "First Name" for a `<label>First Name</label>`)</param>
	public static IElement FindByLabelText(this IRenderedFragment renderedFragment, string labelText)
	{
		return FindByLabelTextInternal(renderedFragment, labelText) ?? throw new ElementNotFoundException(labelText);
	}

	/// <summary>
	/// Returns the first element (i.e. an input, select, textarea, etc. element) associated with the given label text.
	/// </summary>
	/// <param name="renderedFragment">The rendered fragment to search.</param>
	/// <param name="labelText">The text of the label to search (i.e. the InnerText of the Label, such as "First Name" for a `<label>First Name</label>`)</param>
	internal static IElement? FindByLabelTextInternal(this IRenderedFragment renderedFragment, string labelText)
	{
		try
		{
			foreach (var strategy in LabelTextQueryStrategies)
			{
				var element = strategy.FindElement(renderedFragment, labelText);

				if (element != null)
					return element;
			}
		}
		catch (DomException exception) when (exception.Message == "The string did not match the expected pattern.")
		{
			return null;
		}

		return null;
	}
}
