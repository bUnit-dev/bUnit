namespace Bunit.Roles;

public class RoleNotFoundException : Exception
{
    public IReadOnlyList<string> AvailableRoles { get; }

    public RoleNotFoundException(AriaRole role, IReadOnlyList<string> availableRoles)
        : base($"Unable to find element with role '{role.ToString().ToLowerInvariant()}'. Available roles: {string.Join(", ", availableRoles)}")
    {
        AvailableRoles = availableRoles;
    }
} 