using Bunit.Rendering;

namespace Bunit;

internal interface IRenderedComponent : IDisposable
{
	/// <summary>
	/// Gets the id of the rendered component or fragment.
	/// </summary>
	int ComponentId { get; }

	/// <summary>
	/// Called by the owning <see cref="BunitRenderer"/> when it finishes a render.
	/// </summary>
	void UpdateState(bool hasRendered, bool isMarkupGenerationRequired);
}
