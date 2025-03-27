using AngleSharp.Dom;
using System.Text;

namespace Bunit.Roles;

public class RoleNotFoundException : Exception
{
    public IReadOnlyList<string> AvailableRoles { get; }
    public INodeList Nodes { get; }
    public bool HiddenOptionEnabled { get; }

    public RoleNotFoundException(AriaRole role, IReadOnlyList<string> availableRoles, INodeList nodes, bool hiddenOptionEnabled)
        : base(BuildMessage(role, availableRoles, nodes, hiddenOptionEnabled))
    {
        AvailableRoles = availableRoles;
        Nodes = nodes;
        HiddenOptionEnabled = hiddenOptionEnabled;
    }

    private static string BuildMessage(AriaRole role, IReadOnlyList<string> availableRoles, INodeList nodes, bool hiddenOptionEnabled)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Unable to find element with role '{role.ToString().ToLowerInvariant()}'");
        sb.AppendLine();

        if (!availableRoles.Any())
        {
            if (!hiddenOptionEnabled)
            {
                sb.AppendLine("There are no accessible roles. But there might be some inaccessible roles. If you wish to access them, then set the `hidden` option to `true`. Learn more about this here: https://testing-library.com/docs/dom-testing-library/api-queries#byrole");
            }
            else
            {
                sb.AppendLine("There are no available roles.");
            }
            sb.AppendLine();
        }
        else
        {
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
        }

        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine();
        sb.AppendLine("Ignored nodes: comments, script, style");
        
        // Serialize the HTML properly
        foreach (var node in nodes)
        {
            if (node is IElement element)
            {
                sb.AppendLine(element.OuterHtml);
            }
        }

        return sb.ToString();
    }
} 