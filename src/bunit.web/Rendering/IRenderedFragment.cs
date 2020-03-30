using System;
using System.Collections.Generic;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <summary>
    /// Represents a rendered fragment.
    /// </summary>
    public interface IRenderedFragment
    {
        /// <summary>
        /// Gets the id of the rendered component or fragment.
        /// </summary>
        int ComponentId { get; }

        /// <summary>
        /// Gets an <see cref="IObservable{RenderEvent}"/> which will provide subscribers with <see cref="RenderEvent"/>s 
        /// whenever the <see cref="IRenderedFragment"/> is rendered.
        /// </summary>
        IObservable<RenderEvent> RenderEvents { get; }

        /// <summary>
        /// Finds the first component of type <typeparamref name="TComponent"/> in the render tree of 
        /// this <see cref="IRenderedFragment"/>.
        /// </summary>
        /// <typeparam name="TComponent">Type of component to find.</typeparam>
        /// <exception cref="ComponentNotFoundException">Thrown if a component of type <typeparamref name="TComponent"/> was not found in the render tree.</exception>
        /// <returns>The <see cref="IRenderedComponent{T}"/>.</returns>
        IRenderedComponent<TComponent> FindComponent<TComponent>() where TComponent : class, IComponent;

        /// <summary>
        /// Finds all components of type <typeparamref name="TComponent"/> in the render tree of 
        /// this <see cref="IRenderedFragment"/>.
        /// </summary>
        /// <typeparam name="TComponent">Type of components to find.</typeparam>
        /// <returns>The <see cref="IRenderedComponent{T}"/>s</returns>
        IReadOnlyList<IRenderedComponent<TComponent>> FindComponents<TComponent>() where TComponent : class, IComponent;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used when rendering the component.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Gets the HTML markup from the rendered fragment/component.
        /// </summary>
        string Markup { get; }

        /// <summary>
        /// Gets the AngleSharp <see cref="INodeList"/> based
        /// on the HTML markup from the rendered fragment/component.
        /// </summary>
        INodeList Nodes { get; }

        /// <summary>
        /// Performs a comparison of the markup produced by the initial rendering of the 
        /// fragment or component under test with the current rendering of the fragment 
        /// or component under test.
        /// </summary>
        /// <returns>A list of differences found.</returns>
        IReadOnlyList<IDiff> GetChangesSinceFirstRender();

        /// <summary>
        /// Performs a comparison of the markup produced by the rendering of the 
        /// fragment or component under test at the time the <see cref="SaveSnapshot"/> was called
        /// with the current rendering of the fragment or component under test.
        /// </summary>
        /// <returns>A list of differences found.</returns>
        IReadOnlyList<IDiff> GetChangesSinceSnapshot();

        /// <summary>
        /// Saves the markup from the current rendering of the fragment or component under test.
        /// Use the method <see cref="GetChangesSinceSnapshot"/> later to get the difference between
        /// the snapshot and the rendered markup at that time.
        /// </summary>
        void SaveSnapshot();
    }
}
