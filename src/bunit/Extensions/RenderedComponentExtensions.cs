using AngleSharp.Dom;
using Bunit.Rendering;
using Bunit.Web.AngleSharp;

namespace Bunit;

/// <summary>
/// Helper methods for querying <see cref="IRenderedComponent{TComponent}"/>.
/// </summary>
public static class RenderedComponentExtensions
{
	/// <summary>
	/// Returns the first element from the rendered fragment or component under test,
	/// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal
	/// of the rendered nodes.
	/// </summary>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="cssSelector">The group of selectors to use.</param>
	public static IElement Find<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent
		=> Find<TComponent, IElement>(renderedComponent, cssSelector);

	/// <summary>
	/// Returns the first element of type <typeparamref name="TElement"/> from the rendered fragment or component under test,
	/// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal
	/// of the rendered nodes.
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of element to find (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="cssSelector">The group of selectors to use.</param>
	/// <exception cref="ElementNotFoundException">Thrown if no element matches the <paramref name="cssSelector"/>.</exception>
	public static TElement Find<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent
		where TElement : class, IElement
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		var result = renderedComponent.Nodes.QuerySelector(cssSelector);

		if (result is null)
			throw new ElementNotFoundException(cssSelector);

		if (result is not TElement)
			throw new ElementNotFoundException(
				$"The element matching '{cssSelector}' is of type '{result.GetType().Name}', not '{typeof(TElement).Name}'.");

		return (TElement)result.WrapUsing(new CssSelectorElementFactory((IRenderedComponent<IComponent>)renderedComponent, cssSelector));
	}

	/// <summary>
	/// Returns a refreshable collection of <see cref="IElement"/>s from the rendered fragment or component under test,
	/// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal
	/// of the rendered nodes.
	/// </summary>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="cssSelector">The group of selectors to use.</param>
	/// <returns>An <see cref="IReadOnlyList{IElement}"/>, that can be refreshed to execute the search again.</returns>
	public static IReadOnlyList<IElement> FindAll<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent
		=> FindAll<TComponent, IElement>(renderedComponent, cssSelector);

	/// <summary>
	/// Returns a collection of elements of type <typeparamref name="TElement"/> from the rendered fragment or component under test,
	/// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal
	/// of the rendered nodes. Only elements matching the type <typeparamref name="TElement"/> are returned.
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of elements to find (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The rendered fragment to search.</param>
	/// <param name="cssSelector">The group of selectors to use.</param>
	/// <returns>An <see cref="IReadOnlyList{TElement}"/> containing only elements matching the specified type.</returns>
	public static IReadOnlyList<TElement> FindAll<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent
		where TElement : class, IElement
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		return renderedComponent.Nodes.QuerySelectorAll(cssSelector).OfType<TElement>().ToArray();
	}

	/// <summary>
	/// Finds the first component of type <typeparamref name="TChildComponent"/> in the render tree of
	/// this <see cref="IRenderedComponent{TComponent}"/>.
	/// </summary>
	/// <typeparam name="TChildComponent">Type of component to find.</typeparam>
	/// <exception cref="ComponentNotFoundException">Thrown if a component of type <typeparamref name="TChildComponent"/> was not found in the render tree.</exception>
	/// <returns>The <see cref="RenderedComponent{T}"/>.</returns>
	public static IRenderedComponent<TChildComponent> FindComponent<TChildComponent>(this IRenderedComponent<IComponent> renderedComponent)
		where TChildComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		var renderer = renderedComponent.Services.GetRequiredService<BunitContext>().Renderer;
		return renderer.FindComponent<TChildComponent>(renderedComponent);
	}

	/// <summary>
	/// Finds all components of type <typeparamref name="TChildComponent"/> in the render tree of
	/// this <see cref="IRenderedComponent{TComponent}"/>, in depth-first order.
	/// </summary>
	/// <typeparam name="TChildComponent">Type of components to find.</typeparam>
	/// <returns>The <see cref="RenderedComponent{T}"/>s.</returns>
	public static IReadOnlyList<IRenderedComponent<TChildComponent>> FindComponents<TChildComponent>(this IRenderedComponent<IComponent> renderedComponent)
		where TChildComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		var renderer = renderedComponent.Services.GetRequiredService<BunitContext>().Renderer;
		var components = renderer.FindComponents<TChildComponent>(renderedComponent);

		return components.ToArray();
	}

	/// <summary>
	/// Checks whether the render tree the <paramref name="renderedComponent"/> is the root of
	/// contains a component of type <typeparamref name="TChildComponent"/>.
	/// </summary>
	/// <typeparam name="TChildComponent">The type of component to look for in the render tree.</typeparam>
	/// <param name="renderedComponent">The render tree to search.</param>
	/// <returns>True if the <paramref name="renderedComponent"/> contains the <typeparamref name="TChildComponent"/>; otherwise false.</returns>
	public static bool HasComponent<TChildComponent>(this IRenderedComponent<IComponent> renderedComponent)
		where TChildComponent : IComponent => FindComponents<TChildComponent>(renderedComponent).Count > 0;
}
