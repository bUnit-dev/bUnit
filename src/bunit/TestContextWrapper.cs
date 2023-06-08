using Bunit.Rendering;

namespace Bunit;

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
	public BunitRenderer Renderer => TestContext?.Renderer ?? throw new InvalidOperationException("The TestContext has not been initialized.");

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
	/// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters to pass to the component when it is rendered.</param>
	/// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
	public virtual IRenderedComponent<TComponent> Render<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder = null)
		where TComponent : IComponent
		=> TestContext?.Render<TComponent>(parameterBuilder) ?? throw new InvalidOperationException("The TestContext has not been initialized.");

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

	/// <summary>
	/// Disposes all components rendered via this <see cref="TestContext"/>.
	/// </summary>
	public virtual void DisposeComponents() => TestContext?.DisposeComponents();

	/// <summary>
	/// Dummy method required to allow Blazor's compiler to generate
	/// C# from .razor files.
	/// </summary>
	protected virtual void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) { }
}
