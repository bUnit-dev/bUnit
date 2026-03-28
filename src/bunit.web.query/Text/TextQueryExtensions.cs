using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit;

/// <summary>
/// Extension methods for querying <see cref="IRenderedComponent{TComponent}" /> by text content
/// </summary>
public static partial class TextQueryExtensions
{
	private static readonly HashSet<string> IgnoredNodeNames = new(StringComparer.OrdinalIgnoreCase) { "SCRIPT", "STYLE" };

	/// <summary>
	/// Returns the first element whose text content matches the given text.
	/// </summary>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="searchText">The text content to search for.</param>
	/// <param name="configureOptions">Method used to override the default behavior of FindByText.</param>
	/// <returns>The first element matching the specified text.</returns>
	/// <exception cref="TextNotFoundException">Thrown when no element matching the provided text is found.</exception>
	public static IElement FindByText(this IRenderedComponent<IComponent> renderedComponent, string searchText, Action<ByTextOptions>? configureOptions = null)
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);
		ArgumentNullException.ThrowIfNull(searchText);

		var options = ByTextOptions.Default;
		if (configureOptions is not null)
		{
			options = options with { };
			configureOptions.Invoke(options);
		}

		return FindByTextInternal(renderedComponent, searchText, options) ?? throw new TextNotFoundException(searchText);
	}

	/// <summary>
	/// Returns all elements whose text content matches the given text.
	/// </summary>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="searchText">The text content to search for.</param>
	/// <param name="configureOptions">Method used to override the default behavior of FindAllByText.</param>
	/// <returns>A read-only collection of elements matching the text. Returns an empty collection if no matches are found.</returns>
	public static IReadOnlyList<IElement> FindAllByText(this IRenderedComponent<IComponent> renderedComponent, string searchText, Action<ByTextOptions>? configureOptions = null)
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);
		ArgumentNullException.ThrowIfNull(searchText);

		var options = ByTextOptions.Default;
		if (configureOptions is not null)
		{
			options = options with { };
			configureOptions.Invoke(options);
		}

		return FindAllByTextInternal(renderedComponent, searchText, options);
	}

	internal static IElement? FindByTextInternal(this IRenderedComponent<IComponent> renderedComponent, string searchText, ByTextOptions options)
	{
		var elements = renderedComponent.Nodes.TryQuerySelectorAll(options.Selector);
		var normalizedSearchText = NormalizeWhitespace(searchText);

		foreach (var element in elements)
		{
			if (IgnoredNodeNames.Contains(element.NodeName))
				continue;

			var normalizedTextContent = NormalizeWhitespace(element.TextContent);

			if (normalizedTextContent.Equals(normalizedSearchText, options.ComparisonType))
				return element.WrapUsing(new ByTextElementFactory(renderedComponent, searchText, options));
		}

		return null;
	}

	internal static IReadOnlyList<IElement> FindAllByTextInternal(this IRenderedComponent<IComponent> renderedComponent, string searchText, ByTextOptions options)
	{
		var elements = renderedComponent.Nodes.TryQuerySelectorAll(options.Selector);
		var normalizedSearchText = NormalizeWhitespace(searchText);
		var results = new List<IElement>();
		var seen = new HashSet<IElement>();

		foreach (var element in elements)
		{
			if (IgnoredNodeNames.Contains(element.NodeName))
				continue;

			var normalizedTextContent = NormalizeWhitespace(element.TextContent);

			if (!normalizedTextContent.Equals(normalizedSearchText, options.ComparisonType))
				continue;

			var underlyingElement = element.Unwrap();
			if (seen.Add(underlyingElement))
			{
				results.Add(element.WrapUsing(new ByTextElementFactory(renderedComponent, searchText, options)));
			}
		}

		return results;
	}

	internal static string NormalizeWhitespace(string text)
	{
		var trimmed = text.Trim();
		return CollapseWhitespaceRegex().Replace(trimmed, " ");
	}

	[GeneratedRegex(@"\s+")]
	private static partial Regex CollapseWhitespaceRegex();
}
