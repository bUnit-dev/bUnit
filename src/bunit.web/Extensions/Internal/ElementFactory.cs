using System;
using AngleSharp.Dom;
using AngleSharpWrappers;

namespace Bunit
{
    internal sealed class ElementFactory<TElement> : ConcurrentRenderEventSubscriber, IElementFactory<TElement>
        where TElement : class, IElement
    {
        private readonly IRenderedFragment _testTarget;
        private readonly string _cssSelector;
        private TElement? _element;

        public ElementFactory(IRenderedFragment testTarget, TElement initialElement, string cssSelector)
            : base((testTarget ?? throw new ArgumentNullException(nameof(testTarget))).RenderEvents)
        {
            _testTarget = testTarget;
            _cssSelector = cssSelector;
            _element = initialElement;
        }

        public override void OnNext(RenderEvent value)
        {
            if (value.HasChangesTo(_testTarget))
                _element = null;
        }

        TElement IElementFactory<TElement>.GetElement()
        {
            if (_element is null)
            {
                var queryResult = _testTarget.Nodes.QuerySelector(_cssSelector);
                if (queryResult is TElement element)
                    _element = element;
            }
            return _element ?? throw new ElementNotFoundException();
        }
    }
}
