using System.Collections;
using System.Collections.Generic;
using AngleSharp.Dom;
using Bunit.Rendering.RenderEvents;

namespace Bunit
{
	internal class RefreshableElementCollection : IRefreshableElementCollection<IElement>
    {
        private readonly IRenderedFragment _renderedFragment;
        private readonly string _cssSelector;
        private IHtmlCollection<IElement> _elements;
        private ComponentChangeEventSubscriber? _changeEvents;
        private bool _enableAutoRefresh = false;

        public bool EnableAutoRefresh
        {
            get => _enableAutoRefresh;
            set
            {
                // not enabled and should enable
                if (value && !_enableAutoRefresh)
                {
                    _changeEvents?.Unsubscribe();
                    _changeEvents = new ComponentChangeEventSubscriber(_renderedFragment, _ => Refresh());
                }
                if (!value && _enableAutoRefresh)
                {
                    _changeEvents?.Unsubscribe();
                    _changeEvents = null;
                }
                _enableAutoRefresh = value;
            }
        }

        public RefreshableElementCollection(IRenderedFragment renderedFragment, string cssSelector)
        {
            _renderedFragment = renderedFragment;
            _cssSelector = cssSelector;
            _elements = RefreshInternal();
        }

        public void Refresh()
        {
            _elements = RefreshInternal();
        }

        public IElement this[int index] => _elements[index];

        public int Count => _elements.Length;

        public IEnumerator<IElement> GetEnumerator() => _elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IHtmlCollection<IElement> RefreshInternal() => _renderedFragment.Nodes.QuerySelectorAll(_cssSelector);
    }
}
