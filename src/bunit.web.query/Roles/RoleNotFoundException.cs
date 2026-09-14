namespace Bunit;

/// <summary>
/// Represents a failure to find an element in the searched target
/// using the specified role.
/// </summary>
public sealed class RoleNotFoundException : Exception
{
	/// <summary>
	/// Gets the role used to search with.
	/// </summary>
	public AriaRole Role { get; }

	/// <summary>
	/// Gets the accessible name filter that was applied, if any.
	/// </summary>
	public string? AccessibleName { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="RoleNotFoundException"/> class.
	/// </summary>
	/// <param name="role">The role that was searched for.</param>
	/// <param name="accessibleName">The accessible name filter that was applied, if any.</param>
	public RoleNotFoundException(AriaRole role, string? accessibleName = null)
		: base(CreateMessage(role, accessibleName))
	{
		Role = role;
		AccessibleName = accessibleName;
	}

	private static string CreateMessage(AriaRole role, string? accessibleName)
	{
		var roleString = role.ToString();
		return accessibleName is not null
			? $"Unable to find an element with the role '{roleString}' and accessible name '{accessibleName}'."
			: $"Unable to find an element with the role '{roleString}'.";
	}
}
