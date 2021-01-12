using System;
using AngleSharp.Dom;
using AngleSharpWrappers;

namespace Bunit
{
	internal sealed class ElementFactory<TElement> : IElementFactory<TElement>
	    where TElement : class, IElement
	{
		private readonly IRenderedFragment testTarget;
		private readonly string cssSelector;
		private TElement? element;

		public ElementFactory(IRenderedFragment testTarget, TElement initialElement, string cssSelector)
		{
			this.testTarget = testTarget;
			this.cssSelector = cssSelector;
			element = initialElement;
			testTarget.OnMarkupUpdated += FragmentsMarkupUpdated;
		}

		private void FragmentsMarkupUpdated(object? sender, EventArgs args) => element = null;

		TElement IElementFactory<TElement>.GetElement()
		{
			if (element is null)
			{
				var queryResult = testTarget.Nodes.QuerySelector(cssSelector);
				if (queryResult is TElement element)
					this.element = element;
			}

			return element ?? throw new ElementRemovedFromDomException(cssSelector);
		}
	}
}
