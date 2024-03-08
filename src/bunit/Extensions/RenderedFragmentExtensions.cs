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
	public static IElement Find(this RenderedFragment renderedFragment, string cssSelector)
	{
		ArgumentNullException.ThrowIfNull(renderedFragment);

		var result = renderedFragment.Nodes.QuerySelector(cssSelector);

		if (result is null)
			throw new ElementNotFoundException(cssSelector);

		return result.WrapUsing(new CssSelectorElementFactory(renderedFragment, cssSelector));
	}

	/// <summary>
	/// Returns a refreshable collection of <see cref="IElement"/>s from the rendered fragment or component under test,
	/// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal
	/// of the rendered nodes.
	/// </summary>
	/// <param name="renderedFragment">The rendered fragment to search.</param>
	/// <param name="cssSelector">The group of selectors to use.</param>
	/// <param name="enableAutoRefresh">If true, the returned <see cref="IRefreshableElementCollection{IElement}"/> will automatically refresh its <see cref="IElement"/>s whenever the <paramref name="renderedFragment"/> changes.</param>
	/// <returns>An <see cref="IRefreshableElementCollection{IElement}"/>, that can be refreshed to execute the search again.</returns>
	public static IRefreshableElementCollection<IElement> FindAll(this RenderedFragment renderedFragment, string cssSelector, bool enableAutoRefresh = false)
	{
		ArgumentNullException.ThrowIfNull(renderedFragment);
		return new RefreshableElementCollection(renderedFragment, cssSelector) { EnableAutoRefresh = enableAutoRefresh };
	}

	/// <summary>
	/// Finds the first component of type <typeparamref name="TComponent"/> in the render tree of
	/// this <see cref="RenderedFragment"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of component to find.</typeparam>
	/// <exception cref="ComponentNotFoundException">Thrown if a component of type <typeparamref name="TComponent"/> was not found in the render tree.</exception>
	/// <returns>The <see cref="RenderedComponent{T}"/>.</returns>
	public static RenderedComponent<TComponent> FindComponent<TComponent>(this RenderedFragment renderedFragment)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedFragment);

		var renderer = renderedFragment.Services.GetRequiredService<BunitContext>().Renderer;
		return renderer.FindComponent<TComponent>(renderedFragment);
	}

	/// <summary>
	/// Finds all components of type <typeparamref name="TComponent"/> in the render tree of
	/// this <see cref="RenderedFragment"/>, in depth-first order.
	/// </summary>
	/// <typeparam name="TComponent">Type of components to find.</typeparam>
	/// <returns>The <see cref="RenderedComponent{T}"/>s.</returns>
	public static IReadOnlyList<RenderedComponent<TComponent>> FindComponents<TComponent>(this RenderedFragment renderedFragment)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedFragment);

		var renderer = renderedFragment.Services.GetRequiredService<BunitContext>().Renderer;
		var components = renderer.FindComponents<TComponent>(renderedFragment);

		return components.OfType<RenderedComponent<TComponent>>().ToArray();
	}

	/// <summary>
	/// Checks whether the render tree the <paramref name="renderedFragment"/> is the root of
	/// contains a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">The type of component to look for in the render tree.</typeparam>
	/// <param name="renderedFragment">The render tree to search.</param>
	/// <returns>True if the <paramref name="renderedFragment"/> contains the <typeparamref name="TComponent"/>; otherwise false.</returns>
	public static bool HasComponent<TComponent>(this RenderedFragment renderedFragment)
		where TComponent : IComponent => FindComponents<TComponent>(renderedFragment).Count > 0;
}
