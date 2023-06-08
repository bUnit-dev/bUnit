using System.Diagnostics;
using AngleSharp.Dom;
using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// Represents a rendered fragment.
/// </summary>
public interface IRenderedFragment : IDisposable
{
	/// <summary>
	/// Gets the total number times the fragment has been through its render life-cycle.
	/// </summary>
	int RenderCount { get; }

	/// <summary>
	/// Gets a value indicating whether the rendered component or fragment has been disposed by the <see cref="BunitRenderer"/>.
	/// </summary>
	bool IsDisposed { get; }

	/// <summary>
	/// Gets the id of the rendered component or fragment.
	/// </summary>
	int ComponentId { get; }

	/// <summary>
	/// Called by the owning <see cref="BunitRenderer"/> when it finishes a render.
	/// </summary>
	/// <param name="renderEvent">A <see cref="RenderEvent"/> that represents a render.</param>
	void OnRender(RenderEvent renderEvent);

	/// <summary>
	/// Gets the <see cref="IServiceProvider"/> used when rendering the component.
	/// </summary>
	IServiceProvider Services { get; }

	/// <summary>
	/// Adds or removes an event handler that will be triggered after each render of this <see cref="IRenderedFragment"/>.
	/// </summary>
	event EventHandler OnAfterRender;

	/// <summary>
	/// An event that is raised after the markup of the <see cref="IRenderedFragment"/> is updated.
	/// </summary>
	event EventHandler OnMarkupUpdated;

	/// <summary>
	/// Gets the HTML markup from the rendered fragment/component.
	/// </summary>
	string Markup { get; }

	/// <summary>
	/// Gets the AngleSharp <see cref="INodeList"/> based
	/// on the HTML markup from the rendered fragment/component.
	/// </summary>
	INodeList Nodes { get; }
}
