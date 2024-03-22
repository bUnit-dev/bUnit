using AngleSharp.Dom;

namespace Bunit.Labels.Strategies;

internal interface ILabelTextQueryStrategy
{
	IElement? FindElement(RenderedFragment renderedFragment, string labelText, ByLabelTextOptions options);
}
