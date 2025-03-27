using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Roles.Strategies;

internal sealed class ImplicitRoleStrategy : IRoleQueryStrategy
{
    private static readonly IReadOnlyDictionary<AriaRole, string[]> ImplicitRoles = new Dictionary<AriaRole, string[]>
    {
        [AriaRole.Button] = ["button"],
        [AriaRole.Listbox] = ["select"],
        [AriaRole.Combobox] = ["select"],
        [AriaRole.Heading] = ["h1", "h2", "h3", "h4", "h5", "h6"],
        [AriaRole.Group] = ["details"],
    };

    public IElement? FindElement(IRenderedComponent<IComponent> renderedComponent, AriaRole role)
    {
        if (!ImplicitRoles.TryGetValue(role, out var possibleElements))
            return null;

        foreach (var elementName in possibleElements)
        {
            var elements = renderedComponent.Nodes.TryQuerySelectorAll(elementName);
            foreach (var e in elements)
            {
                // For select elements, check if it's a listbox or combobox
                if (elementName == "select")
                {
                    if (role == AriaRole.Listbox && e.HasAttribute("multiple"))
                        return e;
                    if (role == AriaRole.Combobox && !e.HasAttribute("multiple"))
                        return e;
                }
                else
                {
                    return e;
                }
            }
        }

        return null;
    }
} 