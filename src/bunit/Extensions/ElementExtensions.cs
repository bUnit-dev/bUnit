using AngleSharp.Dom;
using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// Provides extension methods for <see cref="IElement"/> to find components rendered inside them.
/// </summary>
public static class ElementExtensions
{
	/// <summary>
	/// Retrieves the first component of type <typeparamref name="TComponent"/> that is rendered inside the specified <paramref name="element"/>.
	/// </summary>
	public static IRenderedComponent<TComponent> FindComponent<TComponent>(this IElement element)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(element);

		var componentAccessor = element.GetComponentAccessor();
		if (componentAccessor is null)
		{
			throw new InvalidOperationException(
				$"Unable to find component of type {typeof(TComponent).Name} for the given element.");
		}

		var component = componentAccessor.Component;
		if (component.Instance is TComponent)
		{
			return (IRenderedComponent<TComponent>)component;
		}

		var renderer = GetRendererFromComponent(component);
		var foundComponent = renderer.FindComponentForElement<TComponent>(element);
		if (foundComponent is not null)
		{
			return foundComponent;
		}

		throw new InvalidOperationException($"Unable to find component of type {typeof(TComponent).Name} for the given element.");
	}

	private static BunitRenderer GetRendererFromComponent(IRenderedComponent<IComponent> component)
		=> component.Services.GetRequiredService<BunitContext>().Renderer;
}
