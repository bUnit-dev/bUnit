using Bunit.Rendering;

namespace Bunit.Extensions;

/// <summary>
/// Helper methods that make it easier to work directly with a <see cref="BunitRenderer"/>
/// in bUnit web.
/// </summary>
public static class TestRendererExtensions
{
	/// <summary>
	/// Renders a <typeparamref name="TComponent"/> with the parameters <paramref name="parameters"/> passed to it.
	/// </summary>
	/// <typeparam name = "TComponent" > The type of component to render.</typeparam>
	/// <param name="renderer">The renderer to use.</param>
	/// <param name="parameters">The parameters to pass to the component.</param>
	/// <returns>A <see cref="IRenderedComponent{TComponent}"/> that provides access to the rendered component.</returns>
	public static IRenderedComponent<TComponent> RenderComponent<TComponent>(this BunitRenderer renderer, params ComponentParameter[] parameters)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderer);

		return renderer.RenderComponent<TComponent>(new ComponentParameterCollection { parameters });
	}

	/// <summary>
	/// Renders a <typeparamref name="TComponent"/> with the parameters build with the <paramref name="parameterBuilder"/> passed to it.
	/// </summary>
	/// <typeparam name = "TComponent" > The type of component to render.</typeparam>
	/// <param name="renderer">The renderer to use.</param>
	/// <param name="parameterBuilder">The a builder to create parameters to pass to the component.</param>
	/// <returns>A <see cref="IRenderedComponent{TComponent}"/> that provides access to the rendered component.</returns>
	public static IRenderedComponent<TComponent> RenderComponent<TComponent>(this BunitRenderer renderer, Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderer);
		ArgumentNullException.ThrowIfNull(parameterBuilder);

		var builder = new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder);
		return renderer.RenderComponent<TComponent>(builder.Build());
	}
}
