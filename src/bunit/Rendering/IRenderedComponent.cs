using Bunit.Rendering;

namespace Bunit;

internal interface IRenderedComponent : IDisposable
{
	/// <summary>
	/// Gets the id of the rendered component or fragment.
	/// </summary>
	int ComponentId { get; }

	bool IsDisposed { get; }

	/// <summary>
	/// Gets the root of this component's render tree. A root returns itself.
	/// </summary>
	IRenderedComponentRoot Root { get; }

	/// <summary>
	/// Notifies subscribers that the document behind <see cref="IRenderedComponent{TComponent}.Nodes"/>
	/// was replaced, which invalidates every element previously handed out from it.
	/// </summary>
	void RaiseMarkupUpdated();

	/// <summary>
	/// Called by the owning <see cref="BunitRenderer"/> when it finishes a render.
	/// </summary>
	void UpdateState(bool hasRendered);
}

/// <summary>
/// The rendered component at the root of a render tree, which owns the markup and the
/// AngleSharp document every component in that tree reads from.
/// </summary>
internal interface IRenderedComponentRoot : IRenderedComponent
{
	RootMarkupSnapshot? MarkupSnapshot { get; }

	/// <summary>
	/// Regenerates the markup of the entire render tree.
	/// </summary>
	void RegenerateMarkup();
}
