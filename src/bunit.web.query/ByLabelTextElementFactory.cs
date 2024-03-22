using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit;

internal sealed class ByLabelTextElementFactory : IElementWrapperFactory
{
	private readonly RenderedFragment testTarget;
	private readonly string labelText;
	private readonly ByLabelTextOptions options;

	public Action? OnElementReplaced { get; set; }

	public ByLabelTextElementFactory(RenderedFragment testTarget, string labelText, ByLabelTextOptions options)
	{
		this.testTarget = testTarget;
		this.labelText = labelText;
		this.options = options;
		testTarget.OnMarkupUpdated += FragmentsMarkupUpdated;
	}

	private void FragmentsMarkupUpdated(object? sender, EventArgs args)
		=> OnElementReplaced?.Invoke();

	public TElement GetElement<TElement>() where TElement : class, IElement
	{
		var element = testTarget.FindByLabelTextInternal(labelText, options) as TElement;

		return element ?? throw new ElementRemovedFromDomException(labelText);
	}
}
