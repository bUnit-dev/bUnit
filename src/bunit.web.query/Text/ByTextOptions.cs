namespace Bunit;

/// <summary>
/// Allows overrides of behavior for FindByText method
/// </summary>
public record class ByTextOptions
{
	/// <summary>
	/// The default behavior used by FindByText if no overrides are specified
	/// </summary>
	internal static readonly ByTextOptions Default = new();

	/// <summary>
	/// The StringComparison used for comparing the desired text to the element's text content. Defaults to Ordinal (case sensitive).
	/// </summary>
	public StringComparison ComparisonType { get; set; } = StringComparison.Ordinal;

	/// <summary>
	/// The CSS selector used to filter which elements are searched. Defaults to "*" (all elements).
	/// </summary>
	public string Selector { get; set; } = "*";
}
