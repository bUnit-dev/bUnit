using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// Extension methods for navigating the component tree an
/// <see cref="IRenderedComponent{TComponent}"/> is part of.
/// </summary>
public static class RenderedComponentTreeExtensions
{
	/// <summary>
	/// Gets the component that rendered the <paramref name="renderedComponent"/>,
	/// or <see langword="null"/> if it is the outermost component in its render tree.
	/// </summary>
	public static IRenderedComponent<IComponent>? Parent(this IRenderedComponent<IComponent> renderedComponent)
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		var parent = ToComponentState(renderedComponent).ParentComponentState;

		return parent is null || IsBunitInfrastructure(parent)
			? null
			: (IRenderedComponent<IComponent>)parent;
	}

	/// <summary>
	/// Gets the outermost component in the render tree the
	/// <paramref name="renderedComponent"/> is part of.
	/// </summary>
	public static IRenderedComponent<IComponent> Root(this IRenderedComponent<IComponent> renderedComponent)
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		var result = renderedComponent;
		while (result.Parent() is { } parent)
		{
			result = parent;
		}

		return result;
	}

	/// <summary>
	/// Gets the components the <paramref name="renderedComponent"/> was rendered inside of,
	/// from its closest parent to the outermost component in the render tree.
	/// </summary>
	public static IEnumerable<IRenderedComponent<IComponent>> GetAncestors(this IRenderedComponent<IComponent> renderedComponent)
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		return Iterate(renderedComponent);

		static IEnumerable<IRenderedComponent<IComponent>> Iterate(IRenderedComponent<IComponent> renderedComponent)
		{
			for (var parent = renderedComponent.Parent(); parent is not null; parent = parent.Parent())
			{
				yield return parent;
			}
		}
	}

	/// <summary>
	/// Gets the components rendered directly by the <paramref name="renderedComponent"/>,
	/// in render order. Components rendered by those children are not included.
	/// </summary>
	public static IReadOnlyList<IRenderedComponent<IComponent>> GetChildren(this IRenderedComponent<IComponent> renderedComponent)
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		var renderer = renderedComponent.Services.GetRequiredService<BunitContext>().Renderer;

		return renderer.GetChildComponents(renderedComponent.ComponentId);
	}

	/// <summary>
	/// Gets the <typeparamref name="TComponent"/> components rendered directly by the
	/// <paramref name="renderedComponent"/>, in render order.
	/// </summary>
	/// <typeparam name="TComponent">Type of child components to get.</typeparam>
	public static IReadOnlyList<IRenderedComponent<TComponent>> GetChildren<TComponent>(this IRenderedComponent<IComponent> renderedComponent)
		where TComponent : IComponent
	{
		var result = new List<IRenderedComponent<TComponent>>();

		foreach (var child in renderedComponent.GetChildren())
		{
			if (child.Instance is TComponent)
			{
				result.Add((IRenderedComponent<TComponent>)child);
			}
		}

		return result;
	}

	internal static ComponentState ToComponentState(IRenderedComponent<IComponent> renderedComponent)
		=> (ComponentState)renderedComponent;

	/// <summary>
	/// Components bUnit wraps the component under test in are not part of the tree a test sees.
	/// </summary>
	/// <remarks>
	/// A <see cref="CascadingValue{TValue}"/> counts as infrastructure only when everything above
	/// it does too; one written by the user inside their own component is a regular part of the tree.
	/// </remarks>
	internal static bool IsBunitInfrastructure(ComponentState componentState)
	{
		if (componentState.Component is BunitRootComponent or ContainerFragment)
		{
			return true;
		}

		var componentType = componentState.Component.GetType();
		if (componentType.IsGenericType && componentType.GetGenericTypeDefinition() == typeof(CascadingValue<>))
		{
			return componentState.ParentComponentState is { } parent && IsBunitInfrastructure(parent);
		}

		return false;
	}
}
