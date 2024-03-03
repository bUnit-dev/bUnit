namespace Bunit.Rendering;

/// <summary>
/// Represents an activator for <see cref="RenderedFragment"/> and <see cref="RenderedComponent{TComponent}"/> types.
/// </summary>
public interface IRenderedComponentActivator
{
	/// <summary>
	/// Creates an <see cref="RenderedFragment"/> with the specified <paramref name="componentId"/>.
	/// </summary>
	RenderedFragment CreateRenderedFragment(int componentId);

	/// <summary>
	/// Creates an <see cref="RenderedComponent{TComponent}"/> with the specified <paramref name="componentId"/>.
	/// </summary>
	RenderedComponent<TComponent> CreateRenderedComponent<TComponent>(int componentId)
		where TComponent : IComponent;

	/// <summary>
	/// Creates an <see cref="RenderedComponent{TComponent}"/> with the specified <paramref name="componentId"/>,
	/// <paramref name="component"/>, and <paramref name="componentFrames"/>.
	/// </summary>
	RenderedComponent<TComponent> CreateRenderedComponent<TComponent>(int componentId, TComponent component, RenderTreeFrameDictionary componentFrames)
		where TComponent : IComponent;
}
