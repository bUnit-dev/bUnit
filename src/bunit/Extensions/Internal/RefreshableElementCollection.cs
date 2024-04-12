using System.Collections;
using System.Diagnostics;
using AngleSharp.Dom;

namespace Bunit;

[DebuggerDisplay("Selector={cssSelector}, AutoRefresh={enableAutoRefresh}")]
internal sealed class RefreshableElementCollection : IRefreshableElementCollection<IElement>
{
	private readonly RenderedFragment renderedFragment;
	private readonly string cssSelector;
	private IHtmlCollection<IElement> elements;
	private bool enableAutoRefresh;

	public bool EnableAutoRefresh
	{
		get => enableAutoRefresh;
		set
		{
			if (ShouldEnable(value))
			{
				renderedFragment.OnMarkupUpdated += RefreshInternal;
			}

			if (ShouldDisable(value))
			{
				renderedFragment.OnMarkupUpdated -= RefreshInternal;
			}

			enableAutoRefresh = value;
		}
	}

	private bool ShouldDisable(bool value) => !value && enableAutoRefresh;

	private bool ShouldEnable(bool value) => value && !enableAutoRefresh;

	internal RefreshableElementCollection(RenderedFragment renderedFragment, string cssSelector)
	{
		this.renderedFragment = renderedFragment;
		this.cssSelector = cssSelector;
		elements = renderedFragment.Nodes.QuerySelectorAll(cssSelector);
	}

	public void Refresh() => RefreshInternal(this, EventArgs.Empty);

	public IElement this[int index] => elements[index];

	public int Count => elements.Length;

	public IEnumerator<IElement> GetEnumerator() => elements.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void RefreshInternal(object? sender, EventArgs args)
	{
		elements = renderedFragment.Nodes.QuerySelectorAll(cssSelector);
	}
}
