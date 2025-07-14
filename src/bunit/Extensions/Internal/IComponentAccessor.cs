namespace Bunit;

/// <summary>
/// Interface for accessing the component that owns an element.
/// </summary>
internal interface IComponentAccessor
{
	/// <summary>
	/// Gets the component that owns this element.
	/// </summary>
	IRenderedComponent<IComponent> Component { get; }
}
