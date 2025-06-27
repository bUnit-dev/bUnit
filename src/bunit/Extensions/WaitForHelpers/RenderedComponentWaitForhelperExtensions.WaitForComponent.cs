namespace Bunit;

/// <summary>
/// Provides extension methods for waiting on components within a rendered Blazor component.
/// </summary>
public static partial class RenderedComponentWaitForHelperExtensions
{
	/// <summary>
	/// Waits until the specified component is rendered in the DOM.
	/// </summary>
	/// <param name="renderedComponent">The rendered component to find the component in.</param>
	/// <param name="timeout">The maximum time to wait for the element to appear.</param>
	/// <typeparam name="TComponent">The target component type to wait for.</typeparam>
	/// <returns>See <see cref="IRenderedComponent{TComponent}"/>.</returns>
	public static IRenderedComponent<TComponent> WaitForComponent<TComponent>(
		this IRenderedComponent<IComponent> renderedComponent,
		TimeSpan? timeout = null)
		where TComponent : IComponent
	{
		renderedComponent.WaitForState(renderedComponent.HasComponent<TComponent>, timeout);
		return renderedComponent.FindComponent<TComponent>();
	}

	/// <summary>
	/// Waits until the specified number of components are rendered in the DOM and returns their instances.
	/// </summary>
	/// <param name="renderedComponent">The rendered component in which to search for instances of the specified component.</param>
	/// <param name="matchComponentCount">The minimum amount component instances to wait for.</param>
	/// <param name="timeout">The maximum time to wait for the components to appear. Defaults to no specific timeout if not provided.</param>
	/// <typeparam name="TComponent">The target component type to wait for.</typeparam>
	/// <returns>A read-only collection of <see cref="IRenderedComponent{TComponent}"/> instances representing the found components.</returns>
	public static IReadOnlyCollection<IRenderedComponent<TComponent>> WaitForComponents<TComponent>(
		this IRenderedComponent<IComponent> renderedComponent,
		int matchComponentCount,
		TimeSpan? timeout = null)
		where TComponent : IComponent
	{
		renderedComponent.WaitForState(
			() => renderedComponent.FindComponents<TComponent>().Count == matchComponentCount,
			timeout);
		return renderedComponent.FindComponents<TComponent>();
	}

	/// <summary>
	/// Waits until the specified component is rendered in the DOM and returns all instances of that component when the first instance is found.
	/// </summary>
	/// <param name="renderedComponent">The rendered component in which to search for instances of the specified component.</param>
	/// <param name="timeout">The maximum time to wait for the components to appear. Defaults to no specific timeout if not provided.</param>
	/// <typeparam name="TComponent">The target component type to wait for.</typeparam>
	/// <returns>A read-only collection of <see cref="IRenderedComponent{TComponent}"/> instances representing the found components.</returns>
	public static IReadOnlyCollection<IRenderedComponent<TComponent>> WaitForComponents<TComponent>(
		this IRenderedComponent<IComponent> renderedComponent,
		TimeSpan? timeout = null)
		where TComponent : IComponent
	=> renderedComponent.WaitForComponents<TComponent>(1, timeout);
}
