using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit;

internal sealed class ByTextElementFactory : IElementWrapperFactory
{
	private readonly IRenderedComponent<IComponent> testTarget;
	private readonly string searchText;
	private readonly ByTextOptions options;

	public Action? OnElementReplaced { get; set; }

	public ByTextElementFactory(IRenderedComponent<IComponent> testTarget, string searchText, ByTextOptions options)
	{
		this.testTarget = testTarget;
		this.searchText = searchText;
		this.options = options;
		testTarget.OnMarkupUpdated += FragmentsMarkupUpdated;
	}

	private void FragmentsMarkupUpdated(object? sender, EventArgs args)
		=> OnElementReplaced?.Invoke();

	public TElement GetElement<TElement>() where TElement : class, IElement
	{
		var element = testTarget.FindByTextInternal(searchText, options) as TElement;

		return element ?? throw new ElementRemovedFromDomException(searchText);
	}
}
