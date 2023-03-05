namespace Bunit;

/// <inheritdoc/>
public interface IRenderedComponent<out TComponent> : IRenderedFragmentBase
	where TComponent : IComponent
{
	/// <summary>
	/// Gets the component under test.
	/// </summary>
	TComponent Instance { get; }
}
