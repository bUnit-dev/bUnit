using AngleSharp.Dom;
using System.Text;

namespace Bunit.Roles;

public class RoleNotFoundException : Exception
{
    public IReadOnlyList<string> AvailableRoles { get; }
    public INodeList Nodes { get; }

    public RoleNotFoundException(AriaRole role, IReadOnlyList<string> availableRoles, INodeList nodes)
        : base(BuildMessage(role, availableRoles, nodes))
    {
        AvailableRoles = availableRoles;
        Nodes = nodes;
    }

    private static string BuildMessage(AriaRole role, IReadOnlyList<string> availableRoles, INodeList nodes)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Unable to find element with role '{role.ToString().ToLowerInvariant()}'");
        sb.AppendLine();
        sb.AppendLine("Here are the available roles:");
        sb.AppendLine();

        foreach (var availableRole in availableRoles)
        {
            sb.AppendLine($"  {availableRole}:");
            sb.AppendLine();

            // Find elements with this role
            var elements = nodes.TryQuerySelectorAll($"[role='{availableRole}'], h1, h2, h3, h4, h5, h6");
            foreach (var element in elements)
            {
                var name = element.TextContent.Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    sb.AppendLine($"  Name \"{name}\":");
                }
                sb.AppendLine($"  <{element.TagName.ToLowerInvariant()} />");
                sb.AppendLine();
            }
        }

        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine();
        sb.AppendLine("Ignored nodes: comments, script, style");
        sb.AppendLine(nodes.ToString());

        return sb.ToString();
    }
} 