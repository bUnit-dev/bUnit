using Microsoft.AspNetCore.Components;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents an activator for <see cref="IRenderedFragmentBase"/> and <see cref="IRenderedComponentBase{TComponent}"/> types.
	/// </summary>
	public interface IRenderedComponentActivator
	{
		/// <summary>
		/// Creates an <see cref="IRenderedFragmentBase"/> with the specified <paramref name="componentId"/>.
		/// </summary>
		IRenderedFragmentBase CreateRenderedFragment(int componentId);

		/// <summary>
		/// Creates an <see cref="IRenderedComponentBase{TComponent}"/> with the specified <paramref name="componentId"/>.
		/// </summary>
		IRenderedComponentBase<TComponent> CreateRenderedComponent<TComponent>(int componentId)
			where TComponent : IComponent;

		/// <summary>
		/// Creates an <see cref="IRenderedComponentBase{TComponent}"/> with the specified <paramref name="componentId"/>,
		/// <paramref name="component"/>, and <paramref name="componentFrames"/>.
		/// </summary>
		IRenderedComponentBase<TComponent> CreateRenderedComponent<TComponent>(int componentId, TComponent component, RenderTreeFrameDictionary componentFrames)
			where TComponent : IComponent;
	}
}
