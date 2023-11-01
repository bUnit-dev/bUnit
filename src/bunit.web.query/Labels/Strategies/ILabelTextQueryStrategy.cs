using AngleSharp.Dom;

namespace Bunit;

internal interface ILabelTextQueryStrategy
{
	IElement? FindElement(IRenderedFragment renderedFragment, string labelText);
}
