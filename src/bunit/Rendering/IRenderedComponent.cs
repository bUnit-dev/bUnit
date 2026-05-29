using Bunit.Rendering;

namespace Bunit;

internal interface IRenderedComponent : IDisposable
{
	/// <summary>
	/// Gets the id of the rendered component or fragment.
	/// </summary>
	int ComponentId { get; }

	/// <summary>
	/// Gets the root component ID for shared DOM resolution, or <c>null</c> if this is a root component.
	/// </summary>
	int? RootComponentId { get; }

	/// <summary>
	/// Called by the owning <see cref="BunitRenderer"/> when it finishes a render.
	/// </summary>
	void UpdateState(bool hasRendered, bool isMarkupGenerationRequired);

	/// <summary>
	/// Sets the root component ID for shared DOM resolution.
	/// </summary>
	void SetRootComponentId(int rootComponentId);
}
