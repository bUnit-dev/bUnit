using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AngleSharp;
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
	private readonly TComponent instance;
	private string markup = string.Empty;
	private int markupStartIndex;
	private int markupEndIndex;
	private INodeList? latestRenderNodes;
	private bool isDirty;

	public bool IsDirty
	{
		get => isDirty; set
		{
			if (value)
			{
				RenderCount++;
			}

			isDirty = value;
		}
	}

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
	public int RenderCount { get; internal set; }

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

	private readonly BunitHtmlParser htmlParser;

	public RenderedRootComponent Root { get; }

	public RenderedComponent(
		BunitRenderer renderer,
		int componentId,
		IComponent instance,
		IServiceProvider services,
		ComponentState? parentComponentState)
		: base(renderer, componentId, instance, parentComponentState)
	{
		Services = services;
		htmlParser = services.GetRequiredService<BunitHtmlParser>();
		this.instance = (TComponent)instance;
		Root = (parentComponentState as IRenderedComponent)?.Root
			?? parentComponentState as RenderedRootComponent
			?? throw new ArgumentNullException(nameof(parentComponentState), "A rendered component must have a root/parent");
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		if (IsDisposed)
			return;

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
	}

	/// <summary>
	/// Called by the owning <see cref="BunitRenderer"/> when it finishes a render.
	/// </summary>
	public void UpdateMarkup()
	{
		if (IsDisposed)
			return;

		var newMarkup = Root.Markup[markupStartIndex..markupEndIndex];
		if (markup != newMarkup)
		{
			Volatile.Write(ref markup, newMarkup);
			latestRenderNodes = null;
			OnMarkupUpdated?.Invoke(this, EventArgs.Empty);
		}

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


/// <summary>
/// Represents a list of Node instances or nodes.
/// </summary>
internal sealed class NodeList : INodeList
{
	#region Fields

	private readonly List<INode> entries;

	/// <summary>
	/// Gets an empty node-list. Shouldn't be modified.
	/// </summary>
	internal static readonly NodeList Empty = [];

	#endregion

	#region ctor

	internal NodeList()
	{
		entries = [];
	}

	#endregion

	#region Index

	public INode this[Int32 index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => entries[index];
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => entries[index] = value;
	}

	INode INodeList.this[Int32 index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => this[index];
	}

	#endregion

	#region Properties

	public Int32 Length
	{
		get => entries.Count;
	}

	#endregion

	#region Internal Methods

	internal void Add(INode node) => entries.Add(node);

	internal void AddRange(NodeList nodeList) => entries.AddRange(nodeList.entries);

	internal void Insert(Int32 index, Node node) => entries.Insert(index, node);

	internal void Remove(INode node) => entries.Remove(node);

	internal void RemoveAt(Int32 index) => entries.RemoveAt(index);

	internal Boolean Contains(INode node) => entries.Contains(node);

	#endregion

	#region Methods

	public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
	{
		for (var i = 0; i < entries.Count; i++)
		{
			entries[i].ToHtml(writer, formatter);
		}
	}

	#endregion

	#region IEnumerable Implementation

	public List<INode>.Enumerator GetEnumerator() => entries.GetEnumerator();
	IEnumerator<INode> IEnumerable<INode>.GetEnumerator() => entries.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => entries.GetEnumerator();

	#endregion
}

