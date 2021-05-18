using System;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bunit
{
	/// <summary>
	/// Represents a class that wraps a <see cref="Bunit.TestContext"/>. Useful for
	/// simplifying writing tests in NUnit and MSTest, which cannot
	/// inherit directly from <see cref="Bunit.TestContext"/>.
	/// </summary>
	public abstract class TestContextWrapper
	{
		/// <summary>
		/// Gets an instance of the wrapped <see cref="Bunit.TestContext"/>, if any.
		/// </summary>
		protected virtual TestContext? TestContext { get; set; }

		/// <summary>
		/// Gets the renderer used by the test context.
		/// </summary>
		public ITestRenderer Renderer => TestContext?.Renderer ?? throw new InvalidOperationException("The TestContext has not been initialized.");

		/// <summary>
		/// Gets the service collection and service provider that is used when a
		/// component is rendered by the test context.
		/// </summary>
		public TestServiceProvider Services => TestContext?.Services ?? throw new InvalidOperationException("The TestContext has not been initialized.");

		/// <summary>
		/// Gets the service collection and service provider that is used when a
		/// component is rendered by the test context.
		/// </summary>
		public RootRenderTree RenderTree => TestContext?.RenderTree ?? throw new InvalidOperationException("The TestContext has not been initialized.");

		/// <summary>
		/// Gets bUnits JSInterop, that allows setting up handlers for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> invocations
		/// that components under tests will issue during testing. It also makes it possible to verify that the invocations has happened as expected.
		/// </summary>
		public BunitJSInterop JSInterop => TestContext?.JSInterop ?? throw new InvalidOperationException("The TestContext has not been initialized.");

		/// <summary>
		/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of the component to render.</typeparam>
		/// <param name="parameters">Parameters to pass to the component when it is rendered.</param>
		/// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
		public virtual IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters)
			where TComponent : IComponent
			=> TestContext?.RenderComponent<TComponent>(parameters) ?? throw new InvalidOperationException("The TestContext has not been initialized.");

		/// <summary>
		/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of the component to render.</typeparam>
		/// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters to pass to the component when it is rendered.</param>
		/// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
		public virtual IRenderedComponent<TComponent> RenderComponent<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder)
			where TComponent : IComponent
			=> TestContext?.RenderComponent<TComponent>(parameterBuilder) ?? throw new InvalidOperationException("The TestContext has not been initialized.");

		/// <summary>
		/// Renders the <paramref name="renderFragment"/> and returns the first <typeparamref name="TComponent"/> in the resulting render tree.
		/// </summary>
		/// <remarks>
		/// Calling this method is equivalent to calling <c>Render(renderFragment).FindComponent&lt;TComponent&gt;()</c>.
		/// </remarks>
		/// <typeparam name="TComponent">The type of component to find in the render tree.</typeparam>
		/// <param name="renderFragment">The render fragment to render.</param>
		/// <returns>The <see cref="IRenderedComponent{TComponent}"/>.</returns>
		public virtual IRenderedComponent<TComponent> Render<TComponent>(RenderFragment renderFragment)
			where TComponent : IComponent
			=> TestContext?.Render<TComponent>(renderFragment) ?? throw new InvalidOperationException("The TestContext has not been initialized.");

		/// <summary>
		/// Renders the <paramref name="renderFragment"/> and returns it as a <see cref="IRenderedFragment"/>.
		/// </summary>
		/// <param name="renderFragment">The render fragment to render.</param>
		/// <returns>The <see cref="IRenderedFragment"/>.</returns>
		public virtual IRenderedFragment Render(RenderFragment renderFragment)
			=> TestContext?.Render(renderFragment) ?? throw new InvalidOperationException("The TestContext has not been initialized.");

#if NET5_0_OR_GREATER
		/// <summary>
		/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>,
		/// and stubs out all child components.
		/// </summary>
		/// <remarks>
		/// If you pass child components to <typeparamref name="TComponent"/> through parameters,
		/// these will be stubbed out as well.
		/// </remarks>
		/// <typeparam name="TComponent">Type of the component to render.</typeparam>
		/// <param name="parameters">Parameters to pass to the component when it is rendered.</param>
		/// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
		public virtual IRenderedComponent<TComponent> ShallowRenderComponent<TComponent>(params ComponentParameter[] parameters)
			where TComponent : IComponent
			=> TestContext?.ShallowRenderComponent<TComponent>(parameters) ?? throw new InvalidOperationException("The TestContext has not been initialized.");

		/// <summary>
		/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>,
		/// and stubs out all child components.
		/// </summary>
		/// <remarks>
		/// If you pass child components to <typeparamref name="TComponent"/> through parameters,
		/// these will be stubbed out as well.
		/// </remarks>
		/// <typeparam name="TComponent">Type of the component to render.</typeparam>
		/// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters
		/// to pass to the component when it is rendered.</param>
		/// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
		public virtual IRenderedComponent<TComponent> ShallowRenderComponent<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder)
			where TComponent : IComponent
			=> TestContext?.ShallowRenderComponent<TComponent>(parameterBuilder) ?? throw new InvalidOperationException("The TestContext has not been initialized.");

		/// <summary>
		/// Renders the first component (root component) in the <paramref name="renderFragment"/>,
		/// and stubs out all other components, both child components as well as sibling components.
		/// </summary>
		/// <remarks>
		/// Calling this method is equivalent to calling <c>ShallowRender(renderFragment).FindComponent&lt;TComponent&gt;()</c>.
		/// </remarks>
		/// <typeparam name="TComponent">The type of component to find in the render tree.</typeparam>
		/// <param name="renderFragment">The render fragment to render.</param>
		/// <returns>The <see cref="IRenderedComponent{TComponent}"/>.</returns>
		public virtual IRenderedComponent<TComponent> ShallowRender<TComponent>(RenderFragment renderFragment)
			where TComponent : IComponent
			=> TestContext?.ShallowRender<TComponent>(renderFragment) ?? throw new InvalidOperationException("The TestContext has not been initialized.");

		/// <summary>
		/// Renders the first component (root component) in the <paramref name="renderFragment"/>,
		/// and stubs out all other components, both child components as well as sibling components.
		/// </summary>
		/// <param name="renderFragment">The render fragment to render.</param>
		/// <returns>The <see cref="IRenderedFragment"/>.</returns>
		public virtual IRenderedFragment ShallowRender(RenderFragment renderFragment)
			=> TestContext?.ShallowRender(renderFragment) ?? throw new InvalidOperationException("The TestContext has not been initialized.");
#endif

		/// <summary>
		/// Dummy method required to allow Blazor's compiler to generate
		/// C# from .razor files.
		/// </summary>
		protected virtual void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) { }
	}
}
