using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	/// <summary>
	/// Represents a rendered fragment.
	/// </summary>
	public interface IRenderedFragmentCore
	{
		/// <summary>
		/// Gets the id of the rendered component or fragment.
		/// </summary>
		int ComponentId { get; }

		/// <summary>
		/// Gets an <see cref="IObservable{RenderEvent}"/> which will provide subscribers with <see cref="RenderEvent"/>s 
		/// whenever the <see cref="IRenderedFragmentCore"/> is rendered.
		/// </summary>
		IObservable<RenderEvent> RenderEvents { get; }

		/// <summary>
		/// Gets the <see cref="IServiceProvider"/> used when rendering the component.
		/// </summary>
		IServiceProvider Services { get; }
	}

	public static class RenderedFragmentExtensions
	{
		/// <summary>
		/// Finds the first component of type <typeparamref name="TComponent"/> in the render tree of 
		/// this <see cref="IRenderedFragmentCore"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of component to find.</typeparam>
		/// <exception cref="ComponentNotFoundException">Thrown if a component of type <typeparamref name="TComponent"/> was not found in the render tree.</exception>
		/// <returns>The <see cref="IRenderedComponent{T}"/>.</returns>
		public static IRenderedComponent<TComponent> FindComponent<TComponent>(this IRenderedFragmentCore renderedFragment) where TComponent : IComponent
		{
			var renderer = renderedFragment.Services.GetRequiredService<TestRenderer>();
			var (id, component) = renderer.FindComponent<TComponent>(renderedFragment.ComponentId);
			return new RenderedComponent<TComponent>(renderedFragment.Services, id, component);
		}

		/// <summary>
		/// Finds all components of type <typeparamref name="TComponent"/> in the render tree of 
		/// this <see cref="IRenderedFragmentCore"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of components to find.</typeparam>
		/// <returns>The <see cref="IRenderedComponent{T}"/>s</returns>
		public static IReadOnlyList<IRenderedComponent<TComponent>> FindComponents<TComponent>(this IRenderedFragmentCore renderedFragment) where TComponent : IComponent
		{
			var renderer = renderedFragment.Services.GetRequiredService<TestRenderer>();
			var components = renderer.FindComponents<TComponent>(renderedFragment.ComponentId);
			var result = components.Count == 0 ? Array.Empty<IRenderedComponent<TComponent>>() : new IRenderedComponent<TComponent>[components.Count];

			for (int i = 0; i < components.Count; i++)
			{
				result[i] = new RenderedComponent<TComponent>(renderedFragment.Services, components[i].ComponentId, components[i].Component);
			}

			return result;
		}
	}
}
