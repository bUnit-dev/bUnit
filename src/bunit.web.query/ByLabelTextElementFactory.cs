using AngleSharp.Dom;

namespace Bunit;

using AngleSharpWrappers;


internal sealed class ByLabelTextElementFactory<TElement> : IElementFactory<TElement>
	where TElement : class, IElement
{
	private readonly IRenderedFragment testTarget;
	private readonly string labelText;
	private TElement? element;

	public ByLabelTextElementFactory(IRenderedFragment testTarget, TElement initialElement, string labelText)
	{
		this.testTarget = testTarget;
		element = initialElement;
		this.labelText = labelText;
		testTarget.OnMarkupUpdated += FragmentsMarkupUpdated;
	}

	private void FragmentsMarkupUpdated(object? sender, EventArgs args) => element = null;

	TElement IElementFactory<TElement>.GetElement()
	{
		if (element is null)
		{
			var queryResult = testTarget.FindByLabelTextInternal(labelText);
			element = queryResult as TElement;
		}

		return element ?? throw new ElementRemovedFromDomException(labelText);
	}
}
