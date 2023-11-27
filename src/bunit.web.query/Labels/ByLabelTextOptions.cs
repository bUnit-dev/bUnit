namespace Bunit;

/// <summary>
/// Allows overrides of behavior for FindByLabelText method
/// </summary>
public record ByLabelTextOptions
{
	/// <summary>
	/// The default behavior used by FindByLabelText if no overrides are specified
	/// </summary>
	public static readonly ByLabelTextOptions Default = new();

	/// <summary>
	/// The StringComparison used for comparing the desired Label Text to the resulting HTML. Defaults to Ordinal (case sensitive).
	/// </summary>
	public StringComparison ComparisonType { get; set; } = StringComparison.Ordinal;
}
