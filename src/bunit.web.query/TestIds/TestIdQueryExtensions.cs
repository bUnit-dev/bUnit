using AngleSharp.Dom;
using Bunit.TestIds;

namespace Bunit;

/// <summary>
/// Extension methods for querying <see cref="IRenderedComponent{TComponent}" /> by Test ID
/// </summary>
public static class TestIdQueryExtensions
{
	/// <summary>
	/// Returns the first element with the specified Test ID.
	/// </summary>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="testId">The Test ID to search for (e.g. "myTestId" in &lt;span data-testid="myTestId"&gt;).</param>
	/// <param name="configureOptions">Method used to override the default behavior of FindByTestId.</param>
	/// <returns>The first element matching the specified role and options.</returns>
	/// <exception cref="TestIdNotFoundException">Thrown when no element matching the provided testId is found.</exception>
	public static IElement FindByTestId(this IRenderedComponent<IComponent> renderedComponent, string testId, Action<ByTestIdOptions>? configureOptions = null)
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);
		ArgumentNullException.ThrowIfNull(testId);

		var options = ByTestIdOptions.Default;
		if (configureOptions is not null)
		{
			options = options with { };
			configureOptions.Invoke(options);
		}

		var elems = renderedComponent.Nodes.TryQuerySelectorAll($"[{options.TestIdAttribute}]");

		foreach (var elem in elems)
		{
			var attr = elem.GetAttribute(options.TestIdAttribute);
			if (attr is not null && attr.Equals(testId, options.ComparisonType))
				return elem;
		}

		throw new TestIdNotFoundException(testId);
	}
}
