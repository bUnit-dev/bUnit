namespace Bunit.TestIds;

/// <summary>
/// Allows overrides of behavior for FindByTestId method
/// </summary>
public record class ByTestIdOptions
{
	internal static readonly ByTestIdOptions Default = new();

	/// <summary>
	/// The StringComparison used for comparing the desired Test ID to the resulting HTML. Defaults to Ordinal (case sensitive).
	/// </summary>
	public StringComparison ComparisonType { get; set; } = StringComparison.Ordinal;

	/// <summary>
	/// The name of the attribute used for finding Test IDs. Defaults to "data-testid".
	/// </summary>
	public string TestIdAttribute { get; set; } = "data-testid";
}
