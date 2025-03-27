using AngleSharp.Dom;
using System.Collections.Generic;
using System.Linq;

namespace Bunit.Roles;

/// <summary>
/// Exception thrown when an element with the specified role is not found.
/// </summary>
public class RoleNotFoundException : System.Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleNotFoundException"/> class.
    /// </summary>
    /// <param name="role">The ARIA role that was searched for.</param>
    /// <param name="name">The accessible name that was searched for (if any).</param>
    /// <param name="availableRoles">A list of available roles in the DOM.</param>
    public RoleNotFoundException(AriaRole role, string? name, IEnumerable<string> availableRoles)
        : base(BuildMessage(role, name, availableRoles))
    {
    }

    private static string BuildMessage(AriaRole role, string? name, IEnumerable<string> availableRoles)
    {
        var roleStr = role.ToString().ToLowerInvariant();
        var message = $"Unable to find an element with role '{roleStr}'";

        if (!string.IsNullOrEmpty(name))
        {
            message += $" and name '{name}'";
        }

        if (availableRoles.Any())
        {
            var availableRolesStr = string.Join(", ", availableRoles);
            message += $". Available roles are: {availableRolesStr}";
        }
        else
        {
            message += ". No roles are available in the document.";
        }

        return message;
    }
} 