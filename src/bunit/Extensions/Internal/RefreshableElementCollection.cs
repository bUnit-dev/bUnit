using System.Collections;
using System.Diagnostics;
using AngleSharp.Dom;

namespace Bunit;

[DebuggerDisplay("Selector={cssSelector}, AutoRefresh={enableAutoRefresh}")]
internal sealed class RefreshableElementCollection : IRefreshableElementCollection<IElement>
{
	private readonly IRenderedComponent<IComponent> renderedComponent;
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
				renderedComponent.OnMarkupUpdated += RefreshInternal;
			}

			if (ShouldDisable(value))
			{
				renderedComponent.OnMarkupUpdated -= RefreshInternal;
			}

			enableAutoRefresh = value;
		}
	}

	private bool ShouldDisable(bool value) => !value && enableAutoRefresh;

	private bool ShouldEnable(bool value) => value && !enableAutoRefresh;

	internal RefreshableElementCollection(IRenderedComponent<IComponent> renderedComponent, string cssSelector)
	{
		this.renderedComponent = renderedComponent;
		this.cssSelector = cssSelector;
		elements = renderedComponent.Nodes.QuerySelectorAll(cssSelector);
	}

	public void Refresh() => RefreshInternal(this, EventArgs.Empty);

	public IElement this[int index] => elements[index];

	public int Count => elements.Length;

	public IEnumerator<IElement> GetEnumerator() => elements.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void RefreshInternal(object? sender, EventArgs args)
	{
		elements = renderedComponent.Nodes.QuerySelectorAll(cssSelector);
	}
}
