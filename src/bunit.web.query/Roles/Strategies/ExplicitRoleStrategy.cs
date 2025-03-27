using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Roles.Strategies;

internal sealed class ExplicitRoleStrategy : IRoleQueryStrategy
{
    public IElement? FindElement(IRenderedComponent<IComponent> renderedComponent, AriaRole role)
    {
        var roleString = role.ToString().ToLowerInvariant();
        return renderedComponent.Nodes.TryQuerySelector($"[role='{roleString}']");
    }
} 