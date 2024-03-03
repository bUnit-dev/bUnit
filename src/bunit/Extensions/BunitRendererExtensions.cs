using Bunit.Rendering;

namespace Bunit.Extensions;

/// <summary>
/// Helper methods that make it easier to work directly with a <see cref="BunitRenderer"/>
/// in bUnit web.
/// </summary>
public static class BunitRendererExtensions
{
	/// <summary>
	/// Renders a <typeparamref name="TComponent"/> with the parameters <paramref name="parameters"/> passed to it.
	/// </summary>
	/// <typeparam name = "TComponent" > The type of component to render.</typeparam>
	/// <param name="renderer">The renderer to use.</param>
	/// <param name="parameters">The parameters to pass to the component.</param>
	/// <returns>A <see cref="RenderedComponent{TComponent}"/> that provides access to the rendered component.</returns>
	public static RenderedComponent<TComponent> Render<TComponent>(this BunitRenderer renderer, params ComponentParameter[] parameters)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderer);

		var resultBase = renderer.Render<TComponent>(new ComponentParameterCollection { parameters });
		if (resultBase is RenderedComponent<TComponent> result)
			return result;

		throw new InvalidOperationException($"The renderer did not produce the expected type.");
	}

	/// <summary>
	/// Renders a <typeparamref name="TComponent"/> with the parameters build with the <paramref name="parameterBuilder"/> passed to it.
	/// </summary>
	/// <typeparam name = "TComponent" > The type of component to render.</typeparam>
	/// <param name="renderer">The renderer to use.</param>
	/// <param name="parameterBuilder">The a builder to create parameters to pass to the component.</param>
	/// <returns>A <see cref="RenderedComponent{TComponent}"/> that provides access to the rendered component.</returns>
	public static RenderedComponent<TComponent> Render<TComponent>(this BunitRenderer renderer, Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderer);
		ArgumentNullException.ThrowIfNull(parameterBuilder);

		var builder = new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder);
		var resultBase = renderer.Render<TComponent>(builder.Build());
		if (resultBase is RenderedComponent<TComponent> result)
			return result;

		throw new InvalidOperationException($"The renderer did not produce the expected type.");
	}
}
