using System.Diagnostics;
using AngleSharp.Dom;
using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// Represents a rendered component.
/// </summary>
[DebuggerDisplay("Component={typeof(TComponent).Name,nq},RenderCount={RenderCount}")]
internal sealed class RenderedComponent<TComponent> : ComponentState, IRenderedComponent<TComponent>, IRenderedComponent
	where TComponent : IComponent
{
	private readonly BunitRenderer renderer;
	private readonly TComponent instance;

	[SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Owned by BunitServiceProvider, disposed by it.")]
	private readonly BunitHtmlParser htmlParser;
	private int renderCount;
	private string markup = string.Empty;
	private int markupStartIndex;
	private int markupEndIndex;
	private INodeList? latestRenderNodes;

	public bool IsDirty { get; set; }

	/// <summary>
	/// Gets the component under test.
	/// </summary>
	public TComponent Instance
	{
		get
		{
			EnsureComponentNotDisposed();
			return instance ?? throw new InvalidOperationException("Component has not rendered yet...");
		}
	}

	/// <summary>
	/// Gets a value indicating whether the rendered component or fragment has been disposed by the <see cref="BunitRenderer"/>.
	/// </summary>
	public bool IsDisposed { get; private set; }

	/// <summary>
	/// Gets the HTML markup from the rendered fragment/component.
	/// </summary>
	public string Markup
	{
		get
		{
			EnsureComponentNotDisposed();
			// Volatile read is necessary to ensure the updated markup
			// is available across CPU cores. Without it, the pointer to the
			// markup string can be stored in a CPUs register and not
			// get updated when another CPU changes the string.
			return Volatile.Read(ref markup);
		}
	}

	/// <summary>
	/// Adds or removes an event handler that will be triggered after
	/// each render of this <see cref="RenderedComponent{T}"/>.
	/// </summary>
	public event EventHandler? OnAfterRender;

	/// <summary>
	/// An event that is raised after the markup of the
	/// <see cref="RenderedComponent{T}"/> is updated.
	/// </summary>
	public event EventHandler? OnMarkupUpdated;

	/// <summary>
	/// Gets the total number times the fragment has been through its render life-cycle.
	/// </summary>
	public int RenderCount => renderCount;

	/// <summary>
	/// Gets the AngleSharp <see cref="INodeList"/> based
	/// on the HTML markup from the rendered fragment/component.
	/// </summary>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public INodeList Nodes
	{
		get
		{
			EnsureComponentNotDisposed();
			return latestRenderNodes ??= htmlParser.Parse(Markup);
		}
	}

	/// <summary>
	/// Gets the <see cref="IServiceProvider"/> used when rendering the component.
	/// </summary>
	public IServiceProvider Services { get; }

	int IRenderedComponent.RenderCount { get => renderCount; set { renderCount = value; } }

	public IRenderedComponent? Root { get; }

	public RenderedComponent(
		BunitRenderer renderer,
		int componentId,
		IComponent instance,
		IServiceProvider services,
		ComponentState? parentComponentState)
		: base(renderer, componentId, instance, parentComponentState)
	{
		Services = services;
		this.renderer = renderer;
		this.instance = (TComponent)instance;
		htmlParser = Services.GetRequiredService<BunitHtmlParser>();
		var parentRenderedComponent = parentComponentState as IRenderedComponent;
		Root = parentRenderedComponent?.Root ?? parentRenderedComponent;
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		if (IsDisposed)
			return;

		if (Root is not null)
			Root.IsDirty = true;

		IsDisposed = true;
		markup = string.Empty;
		OnAfterRender = null;
		OnMarkupUpdated = null;
	}

	/// <inheritdoc/>
	public override ValueTask DisposeAsync()
	{
		Dispose();
		return base.DisposeAsync();
	}

	public void SetMarkupIndices(int start, int end)
	{
		markupStartIndex = start;
		markupEndIndex = end;
		IsDirty = true;
	}

	/// <summary>
	/// Called by the owning <see cref="BunitRenderer"/> when it finishes a render.
	/// </summary>
	public void UpdateMarkup()
	{
		if (IsDisposed)
			return;

		if (Root is RenderedComponent<BunitRootComponent> root)
		{
			var newMarkup = root.markup[markupStartIndex..markupEndIndex];
			if (markup != newMarkup)
			{
				Volatile.Write(ref markup, newMarkup);
				latestRenderNodes = null;
				OnMarkupUpdated?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				// no change
			}
		}
		else
		{
			var newMarkup = Htmlizer.GetHtml(ComponentId, renderer);

			// Volatile write is necessary to ensure the updated markup
			// is available across CPU cores. Without it, the pointer to the
			// markup string can be stored in a CPUs register and not
			// get updated when another CPU changes the string.
			Volatile.Write(ref markup, newMarkup);
			latestRenderNodes = null;
			OnMarkupUpdated?.Invoke(this, EventArgs.Empty);
		}

		IsDirty = false;
		OnAfterRender?.Invoke(this, EventArgs.Empty);
	}

	/// <summary>
	/// Ensures that the underlying component behind the
	/// fragment has not been removed from the render tree.
	/// </summary>
	private void EnsureComponentNotDisposed()
	{
		if (IsDisposed)
			throw new ComponentDisposedException(ComponentId);
	}
}

