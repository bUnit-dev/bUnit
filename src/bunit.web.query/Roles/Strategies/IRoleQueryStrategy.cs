using AngleSharp.Dom;

namespace Bunit.Roles.Strategies;

internal interface IRoleQueryStrategy
{
    IElement? FindElement(IRenderedComponent<IComponent> renderedComponent, AriaRole role);
} 