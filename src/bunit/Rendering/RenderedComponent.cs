using System.Diagnostics;
using AngleSharp.Dom;
using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// Represents a rendered component.
/// </summary>
/// <remarks>
/// The markup and the AngleSharp document belong to the root of the render tree and are
/// shared by every component in it; this type exposes the slice of both that this
/// component rendered. See <see cref="RootMarkupSnapshot"/>.
/// </remarks>
[DebuggerDisplay("Component={typeof(TComponent).Name,nq},RenderCount={RenderCount}")]
internal class RenderedComponent<TComponent> : ComponentState, IRenderedComponent<TComponent>, IRenderedComponent
	where TComponent : IComponent
{
	private readonly TComponent instance;
	private readonly object sliceGate = new();
	private readonly IRenderedComponentRoot? renderTreeRoot;
	private Slice? slice;

	private protected BunitRenderer OwningRenderer { get; }

	[SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Owned by BunitServiceProvider, disposed by it.")]
	private protected BunitHtmlParser HtmlParser { get; }

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

	/// <inheritdoc/>
	public virtual IRenderedComponentRoot Root => renderTreeRoot
		?? throw new InvalidOperationException("A rendered component that is not a render tree root always has a parent.");

	/// <summary>
	/// Gets the HTML markup from the rendered fragment/component.
	/// </summary>
	public string Markup
	{
		get
		{
			EnsureComponentNotDisposed();
			return GetSlice()?.Markup ?? string.Empty;
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
			return GetSlice()?.GetNodes(HtmlParser) ?? NodeRangeList.Empty;
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
		OwningRenderer = renderer;
		HtmlParser = services.GetRequiredService<BunitHtmlParser>();
		this.instance = (TComponent)instance;
		renderTreeRoot = (parentComponentState as IRenderedComponent)?.Root;
	}

	/// <summary>
	/// Adds or removes an event handler that will be triggered after each render of this <see cref="RenderedComponent{T}"/>.
	/// </summary>
	public event EventHandler? OnAfterRender;

	/// <summary>
	/// An event that is raised after the markup of the <see cref="RenderedComponent{T}"/> is updated.
	/// </summary>
	public event EventHandler? OnMarkupUpdated;

	/// <inheritdoc/>
	public void RaiseMarkupUpdated()
	{
		if (IsDisposed)
			return;

		OnMarkupUpdated?.Invoke(this, EventArgs.Empty);
	}

	/// <inheritdoc/>
	public void UpdateState(bool hasRendered)
	{
		if (IsDisposed)
			return;

		if (hasRendered)
		{
			RenderCount++;
			OnAfterRender?.Invoke(this, EventArgs.Empty);
		}
	}

	/// <summary>
	/// Gets this component's slice of the current snapshot, recomputing it when
	/// the snapshot has been replaced.
	/// </summary>
	private Slice? GetSlice()
	{
		lock (sliceGate)
		{
			var current = Root.MarkupSnapshot;

			if (current is null)
				return null;

			if (slice is null || !ReferenceEquals(slice.Snapshot, current))
			{
				slice = new Slice(current, this, OwningRenderer);
			}

			return slice;
		}
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
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposing || IsDisposed)
			return;

		IsDisposed = true;
		slice = null;
		OnAfterRender = null;
		OnMarkupUpdated = null;
		OwningRenderer.OnRenderedComponentDisposed(this);
	}

	public override ValueTask DisposeAsync()
	{
		Dispose();
		return base.DisposeAsync();
	}

	/// <summary>
	/// One component's view of a <see cref="RootMarkupSnapshot"/>.
	/// </summary>
	private sealed class Slice
	{
		private readonly RootMarkupSnapshot snapshot;
		private readonly IRenderedComponent owner;
		private readonly BunitRenderer renderer;
		private readonly object gate = new();
		private INodeList? nodes;

		public RootMarkupSnapshot Snapshot => snapshot;

		public string Markup { get; }

		public Slice(RootMarkupSnapshot snapshot, IRenderedComponent owner, BunitRenderer renderer)
		{
			this.snapshot = snapshot;
			this.owner = owner;
			this.renderer = renderer;
			Markup = snapshot.GetMarkup(owner.ComponentId);
		}

		public INodeList GetNodes(BunitHtmlParser htmlParser)
		{
			lock (gate)
			{
				if (nodes is not null)
					return nodes;

				if (snapshot.TryGetNodeView(owner.ComponentId, out var view))
				{
					nodes = view;
					return nodes;
				}

				// Registered so nodes from this private document can still be traced
				// back to a component by GetOwningComponent.
				nodes = htmlParser.Parse(Markup);
				if (nodes.Length > 0 && nodes[0].Owner is { } document)
				{
					renderer.RegisterPrivateDocument(document, owner);
				}

				return nodes;
			}
		}
	}
}
