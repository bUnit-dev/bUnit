namespace Bunit;

/// <summary>
/// Represents a failure to find an element with the specified ARIA role.
/// </summary>
public sealed class RoleNotFoundException : Exception
{
	/// <summary>
	/// Gets the ARIA role used to search with.
	/// </summary>
	public AriaRole Role { get; }

	/// <summary>
	/// Gets the options used during the search.
	/// </summary>
	public ByRoleOptions Options { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="RoleNotFoundException"/> class.
	/// </summary>
	public RoleNotFoundException(AriaRole role, ByRoleOptions options)
		: base(BuildMessage(role, options))
	{
		Role = role;
		Options = options;
	}

	private static string BuildMessage(AriaRole role, ByRoleOptions options)
	{
		var message = $"Unable to find an element with the role '{role.ToRoleString()}'";

		var filters = new List<string>();
		if (options.Name is not null)
			filters.Add($"name: '{options.Name}'");
		if (options.Level.HasValue)
			filters.Add($"level: {options.Level.Value}");
		if (options.Checked.HasValue)
			filters.Add($"checked: {(options.Checked.Value == true ? "true" : "false")}");
		if (options.Pressed.HasValue)
			filters.Add($"pressed: {(options.Pressed.Value == true ? "true" : "false")}");
		if (options.Selected.HasValue)
			filters.Add($"selected: {(options.Selected.Value == true ? "true" : "false")}");
		if (options.Expanded.HasValue)
			filters.Add($"expanded: {(options.Expanded.Value == true ? "true" : "false")}");
		if (options.Disabled.HasValue)
			filters.Add($"disabled: {(options.Disabled.Value == true ? "true" : "false")}");

		if (filters.Count > 0)
			message += $" ({string.Join(", ", filters)})";

		return message + ".";
	}
}
