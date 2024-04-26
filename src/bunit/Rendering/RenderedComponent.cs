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

	private string markup = string.Empty;
	private INodeList? latestRenderNodes;

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
	/// Gets the total number times the fragment has been through its render life-cycle.
	/// </summary>
	public int RenderCount { get; private set; }

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
	}

	/// <summary>
	/// Adds or removes an event handler that will be triggered after each render of this <see cref="RenderedComponent{T}"/>.
	/// </summary>
	public event EventHandler? OnAfterRender;

	/// <summary>
	/// An event that is raised after the markup of the <see cref="RenderedComponent{T}"/> is updated.
	/// </summary>
	public event EventHandler? OnMarkupUpdated;

	/// <summary>
	/// Called by the owning <see cref="BunitRenderer"/> when it finishes a render.
	/// </summary>
	public void UpdateState(bool hasRendered, bool isMarkupGenerationRequired)
	{
		if (IsDisposed)
			return;

		if (hasRendered)
		{
			RenderCount++;
		}

		if (isMarkupGenerationRequired)
		{
			UpdateMarkup();
			OnMarkupUpdated?.Invoke(this, EventArgs.Empty);
		}

		// The order here is important, since consumers of the events
		// expect that markup has indeed changed when OnAfterRender is invoked
		// (assuming there are markup changes)
		if (hasRendered)
			OnAfterRender?.Invoke(this, EventArgs.Empty);
	}

	/// <summary>
	/// Updates the markup of the rendered fragment.
	/// </summary>
	private void UpdateMarkup()
	{
		latestRenderNodes = null;
		var newMarkup = Htmlizer.GetHtml(ComponentId, renderer);

		// Volatile write is necessary to ensure the updated markup
		// is available across CPU cores. Without it, the pointer to the
		// markup string can be stored in a CPUs register and not
		// get updated when another CPU changes the string.
		Volatile.Write(ref markup, newMarkup);
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

	/// <inheritdoc/>
	public void Dispose()
	{
		if (IsDisposed)
			return;

		IsDisposed = true;
		markup = string.Empty;
		OnAfterRender = null;
		OnMarkupUpdated = null;
	}

	public override ValueTask DisposeAsync()
	{
		Dispose();
		return base.DisposeAsync();
	}
}
