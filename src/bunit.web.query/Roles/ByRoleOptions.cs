namespace Bunit;

/// <summary>
/// Allows overrides of behavior for FindByRole method.
/// </summary>
public record class ByRoleOptions
{
	/// <summary>
	/// The default behavior used by FindByRole if no overrides are specified.
	/// </summary>
	internal static readonly ByRoleOptions Default = new();

	/// <summary>
	/// Gets or sets the accessible name to filter by. When set, only elements whose
	/// accessible name matches this value will be returned.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Gets or sets the heading level to filter by. Only applicable for the Heading role.
	/// Matches aria-level attribute or implicit level from h1-h6 elements.
	/// </summary>
	public int? Level { get; set; }

	/// <summary>
	/// Gets or sets the checked state to filter by.
	/// Matches aria-checked attribute or native checkbox/radio checked state.
	/// Use null to not filter, true to match checked, false to match unchecked.
	/// </summary>
	public bool? Checked { get; set; }

	/// <summary>
	/// Gets or sets the pressed state to filter by.
	/// Matches aria-pressed attribute on toggle buttons.
	/// </summary>
	public bool? Pressed { get; set; }

	/// <summary>
	/// Gets or sets the selected state to filter by.
	/// Matches aria-selected attribute or native option selected state.
	/// </summary>
	public bool? Selected { get; set; }

	/// <summary>
	/// Gets or sets the expanded state to filter by.
	/// Matches aria-expanded attribute.
	/// </summary>
	public bool? Expanded { get; set; }

	/// <summary>
	/// Gets or sets the disabled state to filter by.
	/// Matches aria-disabled attribute or native disabled attribute.
	/// </summary>
	public bool? Disabled { get; set; }

	/// <summary>
	/// Gets or sets whether the Name match should be exact (default true).
	/// When false, substring matching is used.
	/// </summary>
	public bool Exact { get; set; } = true;

	/// <summary>
	/// Gets or sets the StringComparison used for comparing the Name. Defaults to Ordinal (case sensitive).
	/// </summary>
	public StringComparison NameComparisonType { get; set; } = StringComparison.Ordinal;
}
