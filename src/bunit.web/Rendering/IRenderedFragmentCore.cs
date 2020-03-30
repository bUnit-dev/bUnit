using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

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
        /// Finds the first component of type <typeparamref name="TComponent"/> in the render tree of 
        /// this <see cref="IRenderedFragmentCore"/>.
        /// </summary>
        /// <typeparam name="TComponent">Type of component to find.</typeparam>
        /// <exception cref="ComponentNotFoundException">Thrown if a component of type <typeparamref name="TComponent"/> was not found in the render tree.</exception>
        /// <returns>The <see cref="IRenderedComponent{T}"/>.</returns>
        IRenderedComponent<TComponent> FindComponent<TComponent>() where TComponent : class, IComponent;

        /// <summary>
        /// Finds all components of type <typeparamref name="TComponent"/> in the render tree of 
        /// this <see cref="IRenderedFragmentCore"/>.
        /// </summary>
        /// <typeparam name="TComponent">Type of components to find.</typeparam>
        /// <returns>The <see cref="IRenderedComponent{T}"/>s</returns>
        IReadOnlyList<IRenderedComponent<TComponent>> FindComponents<TComponent>() where TComponent : class, IComponent;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used when rendering the component.
        /// </summary>
        IServiceProvider Services { get; }
    }
}
