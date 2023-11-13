using AngleSharp.Dom;

namespace Bunit.Labels.Strategies;

internal interface ILabelTextQueryStrategy
{
	IElement? FindElement(IRenderedFragment renderedFragment, string labelText);
}
