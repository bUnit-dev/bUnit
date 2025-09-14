using AngleSharp.Dom;

namespace Bunit.Labels.Strategies;

internal interface ILabelTextQueryStrategy
{
	IElement? FindElement(IRenderedComponent<IComponent> renderedComponent, string labelText, ByLabelTextOptions options);
}
