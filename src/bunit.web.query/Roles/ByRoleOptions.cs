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
	/// The StringComparison used for comparing the accessible name. Defaults to Ordinal (case sensitive).
	/// </summary>
	public StringComparison ComparisonType { get; set; } = StringComparison.Ordinal;

	/// <summary>
	/// If true, includes elements that are normally excluded from the accessibility tree.
	/// Elements can be excluded via aria-hidden="true" or the hidden attribute.
	/// Defaults to false.
	/// </summary>
	public bool Hidden { get; set; }

	/// <summary>
	/// Filters elements by their accessible name (e.g., aria-label, button text, label text).
	/// When null, no filtering by name is applied.
	/// </summary>
	public string? Name { get; set; }

	/// <summary>
	/// Filters elements by their selected state (aria-selected).
	/// Use for elements like tabs or options.
	/// </summary>
	public bool? Selected { get; set; }

	/// <summary>
	/// Filters elements by their checked state (aria-checked or native checked).
	/// Use for checkboxes and radio buttons.
	/// </summary>
	public bool? Checked { get; set; }

	/// <summary>
	/// Filters elements by their pressed state (aria-pressed).
	/// Use for toggle buttons.
	/// </summary>
	public bool? Pressed { get; set; }

	/// <summary>
	/// Filters elements by their current state (aria-current).
	/// Can be true/false or a specific token like "page", "step", "location", "date", "time".
	/// </summary>
	public object? Current { get; set; }

	/// <summary>
	/// Filters elements by their expanded state (aria-expanded).
	/// Use for elements like accordions, dropdowns, or tree items.
	/// </summary>
	public bool? Expanded { get; set; }

	/// <summary>
	/// Filters elements by their busy state (aria-busy).
	/// </summary>
	public bool? Busy { get; set; }

	/// <summary>
	/// Filters headings by their level (1-6).
	/// Only applicable when searching for the "heading" role.
	/// </summary>
	public int? Level { get; set; }
}
