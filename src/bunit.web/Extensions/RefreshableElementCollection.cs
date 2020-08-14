using System.Collections;
using System.Collections.Generic;

using AngleSharp.Dom;

namespace Bunit
{
	internal class RefreshableElementCollection : IRefreshableElementCollection<IElement>
	{
		private readonly IRenderedFragment _renderedFragment;
		private readonly string _cssSelector;
		private IHtmlCollection<IElement> _elements;
		private bool _enableAutoRefresh;

		public bool EnableAutoRefresh
		{
			get => _enableAutoRefresh;
			set
			{
				if (ShouldEnable(value))
				{
					_renderedFragment.OnMarkupUpdated += Refresh;
				}
				if (ShouldDisable(value))
				{
					_renderedFragment.OnMarkupUpdated -= Refresh;
				}
				_enableAutoRefresh = value;
			}
		}

		private bool ShouldDisable(bool value) => !value && _enableAutoRefresh;
		private bool ShouldEnable(bool value) => value && !_enableAutoRefresh;

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
