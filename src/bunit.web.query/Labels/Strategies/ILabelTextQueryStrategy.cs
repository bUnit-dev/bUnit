using AngleSharp.Dom;

namespace Bunit.Labels.Strategies;

internal interface ILabelTextQueryStrategy
{
	IElement? FindElement(IRenderedComponent<IComponent> renderedFragment, string labelText, ByLabelTextOptions options);
}
