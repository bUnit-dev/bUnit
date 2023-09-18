using Bunit.Rendering;

namespace Bunit.Extensions;

/// <summary>
/// Helper methods that make it easier to work directly with a <see cref="BunitRenderer"/>
/// in bUnit web.
/// </summary>
public static class BunitRendererExtensions
{
	/// <summary>
	/// Renders a <typeparamref name="TComponent"/> with the parameters build with the <paramref name="parameterBuilder"/> passed to it.
	/// </summary>
	/// <typeparam name = "TComponent" > The type of component to render.</typeparam>
	/// <param name="renderer">The renderer to use.</param>
	/// <param name="parameterBuilder">The a builder to create parameters to pass to the component.</param>
	/// <returns>A <see cref="RenderedComponent{TComponent}"/> that provides access to the rendered component.</returns>
	public static RenderedComponent<TComponent> Render<TComponent>(this BunitRenderer renderer, Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder = null)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderer);

		var builder = new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder);
		var renderFragment = builder.Build().ToRenderFragment<TComponent>();
		var renderedFragment = renderer.RenderFragment(renderFragment);
		return renderedFragment.FindComponent<TComponent>();
	}
}
