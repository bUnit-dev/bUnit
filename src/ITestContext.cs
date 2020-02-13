using System;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <summary>
    /// A test context is a factory that makes it possible to create components under tests.
    /// </summary>
    public interface ITestContext : IDisposable
    {
        /// <summary>
        /// Gets the service collection and service provider that is used when a 
        /// component is rendered by the test context.
        /// </summary>
        TestServiceProvider Services { get; }

        /// <summary>
        /// Gets the renderer used to render the components and fragments in this test context.
        /// </summary>
        TestRenderer Renderer { get; }

        /// <summary>
        /// Parses a markup HTML string using the AngleSharps HTML5 parser
        /// and returns a list of nodes.
        /// </summary>
        /// <param name="markup">The markup to parse.</param>
        /// <returns>The <see cref="INodeList"/>.</returns>
        INodeList CreateNodes(string markup);

        /// <summary>
        /// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        /// <typeparam name="TComponent">Type of the component to render</typeparam>
        /// <param name="parameters">Parameters to pass to the component when it is rendered</param>
        /// <returns>The rendered <typeparamref name="TComponent"/></returns>
        IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : class, IComponent;
    }
}