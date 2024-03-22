using AngleSharp.Dom;
using Bunit.Rendering;
using Bunit.Web.AngleSharp;

namespace Bunit;

/// <summary>
/// Helper methods for querying <see cref="RenderedFragment"/>.
/// </summary>
public static class RenderedFragmentExtensions
{
	/// <summary>
	/// Returns the first element from the rendered fragment or component under test,
	/// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal
	/// of the rendered nodes.
	/// </summary>
	/// <param name="renderedFragment">The rendered fragment to search.</param>
	/// <param name="cssSelector">The group of selectors to use.</param>
	public static IElement Find<TComponent>(this IRenderedComponent<TComponent> renderedFragment, string cssSelector)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedFragment);

		var result = renderedFragment.Nodes.QuerySelector(cssSelector);

		if (result is null)
			throw new ElementNotFoundException(cssSelector);

		return result.WrapUsing(new CssSelectorElementFactory((IRenderedComponent<IComponent>)renderedFragment, cssSelector));
	}

	/// <summary>
	/// Returns a refreshable collection of <see cref="IElement"/>s from the rendered fragment or component under test,
	/// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal
	/// of the rendered nodes.
	/// </summary>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="cssSelector">The group of selectors to use.</param>
	/// <param name="enableAutoRefresh">If true, the returned <see cref="IRefreshableElementCollection{IElement}"/> will automatically refresh its <see cref="IElement"/>s whenever the <paramref name="renderedComponent"/> changes.</param>
	/// <returns>An <see cref="IRefreshableElementCollection{IElement}"/>, that can be refreshed to execute the search again.</returns>
	public static IRefreshableElementCollection<IElement> FindAll<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, bool enableAutoRefresh = false)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);
		return new RefreshableElementCollection((IRenderedComponent<IComponent>)renderedComponent, cssSelector) { EnableAutoRefresh = enableAutoRefresh };
	}

	/// <summary>
	/// Finds the first component of type <typeparamref name="TChildComponent"/> in the render tree of
	/// this <see cref="RenderedFragment"/>.
	/// </summary>
	/// <typeparam name="TChildComponent">Type of component to find.</typeparam>
	/// <exception cref="ComponentNotFoundException">Thrown if a component of type <typeparamref name="TChildComponent"/> was not found in the render tree.</exception>
	/// <returns>The <see cref="RenderedComponent{T}"/>.</returns>
	public static IRenderedComponent<TChildComponent> FindComponent<TChildComponent>(this IRenderedComponent<IComponent> renderedFragment)
		where TChildComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedFragment);

		var renderer = renderedFragment.Services.GetRequiredService<BunitContext>().Renderer;
		return renderer.FindComponent<TChildComponent>(renderedFragment);
	}

	/// <summary>
	/// Finds all components of type <typeparamref name="TChildComponent"/> in the render tree of
	/// this <see cref="RenderedFragment"/>, in depth-first order.
	/// </summary>
	/// <typeparam name="TChildComponent">Type of components to find.</typeparam>
	/// <returns>The <see cref="RenderedComponent{T}"/>s.</returns>
	public static IReadOnlyList<IRenderedComponent<TChildComponent>> FindComponents<TChildComponent>(this IRenderedComponent<IComponent> renderedFragment)
		where TChildComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedFragment);

		var renderer = renderedFragment.Services.GetRequiredService<BunitContext>().Renderer;
		var components = renderer.FindComponents<TChildComponent>(renderedFragment);

		return components.ToArray();
	}

	/// <summary>
	/// Checks whether the render tree the <paramref name="renderedFragment"/> is the root of
	/// contains a component of type <typeparamref name="TChildComponent"/>.
	/// </summary>
	/// <typeparam name="TChildComponent">The type of component to look for in the render tree.</typeparam>
	/// <param name="renderedFragment">The render tree to search.</param>
	/// <returns>True if the <paramref name="renderedFragment"/> contains the <typeparamref name="TChildComponent"/>; otherwise false.</returns>
	public static bool HasComponent<TChildComponent>(this IRenderedComponent<IComponent> renderedFragment)
		where TChildComponent : IComponent => FindComponents<TChildComponent>(renderedFragment).Count > 0;
}
