using System.Diagnostics;
using AngleSharp.Dom;
using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// Represents a rendered fragment.
/// </summary>
[DebuggerDisplay("Rendered:{RenderCount}")]
public class RenderedFragment : IDisposable
{
	[SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Owned by TestServiceProvider, disposed by it.")]
	private readonly BunitHtmlParser htmlParser;
	private string markup = string.Empty;
	private INodeList? latestRenderNodes;

	/// <summary>
	/// Adds or removes an event handler that will be triggered after each render of this <see cref="RenderedFragment"/>.
	/// </summary>
	public event EventHandler? OnAfterRender;

	/// <summary>
	/// An event that is raised after the markup of the <see cref="RenderedFragment"/> is updated.
	/// </summary>
	public event EventHandler? OnMarkupUpdated;

	/// <summary>
	/// Gets a value indicating whether the rendered component or fragment has been disposed by the <see cref="TestRenderer"/>.
	/// </summary>
	public bool IsDisposed { get; private set; }

	/// <summary>
	/// Gets the id of the rendered component or fragment.
	/// </summary>
	public int ComponentId { get; protected set; }

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
	public int RenderCount { get; protected set; }

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

	internal RenderedFragment(int componentId, IServiceProvider service)
	{
		ComponentId = componentId;
		Services = service;
		htmlParser = Services.GetRequiredService<BunitHtmlParser>();
	}

	/// <summary>
	/// Called by the owning <see cref="TestRenderer"/> when it finishes a render.
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
			((IDisposable)this).Dispose();
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
	protected void UpdateMarkup(RenderTreeFrameDictionary framesCollection)
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
	/// Extension point for the <see cref="OnRender"/> method.
	/// </summary>
	protected virtual void OnRenderInternal(RenderEvent renderEvent) { }

	/// <summary>
	/// Ensures that the underlying component behind the
	/// fragment has not been removed from the render tree.
	/// </summary>
	protected void EnsureComponentNotDisposed()
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

	/// <summary>
	/// Disposes of the render fragment content.
	/// </summary>
	/// <remarks>
	/// The disposing parameter should be false when called from a finalizer, and true when called from the
	/// <see cref="Dispose()"/> method. In other words, it is true when deterministically called and false when non-deterministically called.
	/// </remarks>
	/// <param name="disposing">Set to true if called from <see cref="Dispose()"/>, false if called from a finalizer.f.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (IsDisposed || !disposing)
			return;

		IsDisposed = true;
		markup = string.Empty;
		OnAfterRender = null;
		OnMarkupUpdated = null;
	}
}
