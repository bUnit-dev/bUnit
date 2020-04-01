using System;
using System.Collections.Generic;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	public static class WebRenderedFragmentExtensions
	{
		/// <summary>
		/// Finds the first component of type <typeparamref name="TComponent"/> in the render tree of 
		/// this <see cref="IRenderedFragment"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of component to find.</typeparam>
		/// <exception cref="ComponentNotFoundException">Thrown if a component of type <typeparamref name="TComponent"/> was not found in the render tree.</exception>
		/// <returns>The <see cref="IRenderedComponent{T}"/>.</returns>
		public static IRenderedComponent<TComponent> FindComponent<TComponent>(this IRenderedFragment renderedFragment) where TComponent : IComponent
		{
			var renderer = renderedFragment.Services.GetRequiredService<TestRenderer>();
			var (id, component) = renderer.FindComponent<TComponent>(renderedFragment.ComponentId);
			return new RenderedComponent<TComponent>(renderedFragment.Services, id, component);
		}

		/// <summary>
		/// Finds all components of type <typeparamref name="TComponent"/> in the render tree of 
		/// this <see cref="IRenderedFragment"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of components to find.</typeparam>
		/// <returns>The <see cref="IRenderedComponent{T}"/>s</returns>
		public static IReadOnlyList<IRenderedComponent<TComponent>> FindComponents<TComponent>(this IRenderedFragment renderedFragment) where TComponent : IComponent
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
