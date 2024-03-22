using System.Diagnostics;
using AngleSharp.Dom;
using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// Represents a rendered component.
/// </summary>
[DebuggerDisplay("Component={typeof(TComponent).Name,nq},RenderCount={RenderCount}")]
public sealed class RenderedComponent<TComponent> : IRenderedComponent<TComponent>
	where TComponent : IComponent
{
	private TComponent? instance;

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
	/// Gets the id of the rendered component or fragment.
	/// </summary>
	public int ComponentId { get; private set; }

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

	internal RenderedComponent(int componentId, IServiceProvider services)
	{
		ComponentId = componentId;
		Services = services;
		htmlParser = Services.GetRequiredService<BunitHtmlParser>();
	}

	internal RenderedComponent(int componentId, TComponent instance, RenderTreeFrameDictionary componentFrames, IServiceProvider services)
		: this(componentId, services)
	{
		this.instance = instance;
		RenderCount++;
		UpdateMarkup(componentFrames);
	}

	private void OnRenderInternal(RenderEvent renderEvent)
	{
		// checks if this is the first render, and if it is
		// tries to find the TComponent in the render event
		if (instance is null)
		{
			SetComponentAndID(renderEvent);
		}
	}

	private void SetComponentAndID(RenderEvent renderEvent)
	{
		if (TryFindComponent(renderEvent.Frames, ComponentId, out var id, out var component))
		{
			instance = component;
			ComponentId = id;
		}
		else
		{
			throw new InvalidOperationException("Component instance not found at expected position in render tree.");
		}
	}

	private static bool TryFindComponent(RenderTreeFrameDictionary framesCollection, int parentComponentId, out int componentId, out TComponent component)
	{
		componentId = -1;
		component = default!;

		var frames = framesCollection[parentComponentId];

		for (var i = 0; i < frames.Count; i++)
		{
			ref var frame = ref frames.Array[i];
			if (frame.FrameType == RenderTreeFrameType.Component)
			{
				if (frame.Component is TComponent c)
				{
					componentId = frame.ComponentId;
					component = c;
					return true;
				}

				if (TryFindComponent(framesCollection, frame.ComponentId, out componentId, out component))
				{
					return true;
				}
			}
		}

		return false;
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
	/// <param name="renderEvent">A <see cref="RenderEvent"/> that represents a render.</param>
	public void OnRender(RenderEvent renderEvent)
	{
		ArgumentNullException.ThrowIfNull(renderEvent);

		if (IsDisposed)
			return;

		var (rendered, changed, disposed) = renderEvent.GetRenderStatus(this);

		if (disposed)
		{
			Dispose();
			return;
		}

		if (rendered)
		{
			OnRenderInternal(renderEvent);
			RenderCount++;
		}

		if (changed)
		{
			UpdateMarkup(renderEvent.Frames);
		}

		// The order here is important, since consumers of the events
		// expect that markup has indeed changed when OnAfterRender is invoked
		// (assuming there are markup changes)
		if (changed)
			OnMarkupUpdated?.Invoke(this, EventArgs.Empty);

		if (rendered)
			OnAfterRender?.Invoke(this, EventArgs.Empty);
	}

	/// <summary>
	/// Updates the markup of the rendered fragment.
	/// </summary>
	private void UpdateMarkup(RenderTreeFrameDictionary framesCollection)
	{
		latestRenderNodes = null;
		var newMarkup = Htmlizer.GetHtml(ComponentId, framesCollection);

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
}
