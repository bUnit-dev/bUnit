using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit;

internal sealed class ByLabelTextElementFactory : IElementWrapperFactory
{
	private readonly IRenderedFragment testTarget;
	private readonly string labelText;

	public Action? OnElementReplaced { get; set; }

	public ByLabelTextElementFactory(IRenderedFragment testTarget, string labelText)
	{
		this.testTarget = testTarget;
		this.labelText = labelText;
		testTarget.OnMarkupUpdated += FragmentsMarkupUpdated;
	}

	private void FragmentsMarkupUpdated(object? sender, EventArgs args)
		=> OnElementReplaced?.Invoke();

	public TElement GetElement<TElement>() where TElement : class, IElement
	{
		var element = testTarget.FindByLabelTextInternal(labelText) as TElement;

		return element ?? throw new ElementRemovedFromDomException(labelText);
	}
}
