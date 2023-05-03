using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Represents a rendered fragment.
/// </summary>
public interface IRenderedFragment : IRenderedFragmentBase
{
	/// <summary>
	/// An event that is raised after the markup of the <see cref="IRenderedFragmentBase"/> is updated.
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
