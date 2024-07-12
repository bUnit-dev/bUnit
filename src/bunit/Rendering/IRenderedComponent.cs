using AngleSharp.Dom;
using Bunit.Rendering;

namespace Bunit;

internal interface IRenderedComponent : IDisposable
{
	void UpdateMarkup();

	void SetMarkupIndices(int start, int end);

	bool IsDirty { get; set; }

	RenderedRootComponent Root { get; }
}

/// <summary>
/// Represents a rendered component.
/// </summary>
public interface IRenderedComponent<out TComponent> : IDisposable
	where TComponent : IComponent
{
	/// <summary>
	/// Gets the component under test.
	/// </summary>
	TComponent Instance { get; }

	/// <summary>
	/// Gets the HTML markup from the rendered fragment/component.
	/// </summary>
	string Markup { get; }

	/// <summary>
	/// Gets a value indicating whether the rendered component or fragment has been disposed by the <see cref="BunitRenderer"/>.
	/// </summary>
	bool IsDisposed { get; }

	/// <summary>
	/// Gets the id of the rendered component or fragment.
	/// </summary>
	int ComponentId { get; }

	/// <summary>
	/// Gets the total number times the fragment has been through its render life-cycle.
	/// </summary>
	int RenderCount { get; }

	/// <summary>
	/// Gets the AngleSharp <see cref="INodeList"/> based
	/// on the HTML markup from the rendered fragment/component.
	/// </summary>
	INodeList Nodes { get; }

	/// <summary>
	/// Gets the <see cref="IServiceProvider"/> used when rendering the component.
	/// </summary>
	IServiceProvider Services { get; }

	/// <summary>
	/// Adds or removes an event handler that will be triggered after each render of this <see cref="IRenderedComponent{TComponent}"/>.
	/// </summary>
	event EventHandler? OnAfterRender;

	/// <summary>
	/// An event that is raised after the markup of the <see cref="IRenderedComponent{TComponent}"/> is updated.
	/// </summary>
	event EventHandler? OnMarkupUpdated;
}
