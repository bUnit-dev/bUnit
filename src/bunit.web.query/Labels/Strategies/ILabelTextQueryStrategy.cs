using AngleSharp.Dom;

namespace Bunit.Labels.Strategies;

internal interface ILabelTextQueryStrategy
{
	IElement? FindElement(IRenderedComponent<IComponent> renderedComponent, string labelText, ByLabelTextOptions options)
		=> FindElements(renderedComponent, labelText, options).FirstOrDefault();

	IEnumerable<IElement> FindElements(IRenderedComponent<IComponent> renderedComponent, string labelText, ByLabelTextOptions options);
}
