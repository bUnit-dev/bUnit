using System;
using Egil.RazorComponents.Testing.Diffing;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// A test context is a factory that makes it possible to create components under tests.
    /// </summary>
    public interface ITestContext
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
        /// Gets the HTML parser used to parse HTML produced by components and fragments in this test context.
        /// </summary>
        TestHtmlParser HtmlParser { get; }

        /// <summary>
        /// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        /// <typeparam name="TComponent">Type of the component to render</typeparam>
        /// <param name="parameters">Parameters to pass to the component when it is rendered</param>
        /// <returns>The rendered <typeparamref name="TComponent"/></returns>
        IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : class, IComponent;

        /// <summary>
        /// Executes the provided <paramref name="renderTrigger"/> action and waits for a render to occur.
        /// Use this when you have a component that is awaiting e.g. a service to return data to it before rendering again.
        /// </summary>
        /// <param name="renderTrigger">The action that somehow causes one or more components to render.</param>
        /// <param name="timeout">The maximum time to wait for the next render. If not provided the default is 1 second.</param>
        /// <exception cref="TimeoutException">Thrown when the next render did not happen within the specified <paramref name="timeout"/>.</exception>
        void WaitForNextRender(Action renderTrigger, TimeSpan? timeout = null);
    }
}