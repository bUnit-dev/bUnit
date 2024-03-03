using Bunit.Extensions;
using Bunit.Rendering;
using Microsoft.Extensions.Logging;

namespace Bunit;

/// <summary>
/// A test context is a factory that makes it possible to create components under tests.
/// </summary>
public class TestContext : IDisposable
{
	private bool disposed;
	private TestRenderer? testRenderer;

	/// <summary>
	/// Gets or sets the default wait timeout used by "WaitFor" operations, i.e. <see cref="RenderedFragmentWaitForHelperExtensions.WaitForAssertion(RenderedFragment, Action, TimeSpan?)"/>.
	/// </summary>
	/// <remarks>The default is 1 second.</remarks>
	public static TimeSpan DefaultWaitTimeout { get; set; } = TimeSpan.FromSeconds(1);

	/// <summary>
	/// Gets the renderer used by the test context.
	/// </summary>
	public TestRenderer Renderer => testRenderer ??= CreateTestRenderer();

	/// <summary>
	/// Gets bUnits JSInterop, that allows setting up handlers for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> invocations
	/// that components under tests will issue during testing. It also makes it possible to verify that the invocations has happened as expected.
	/// </summary>
	public BunitJSInterop JSInterop { get; } = new BunitJSInterop();

	/// <summary>
	/// Gets the service collection and service provider that is used when a
	/// component is rendered by the test context.
	/// </summary>
	public TestServiceProvider Services { get; }

	/// <summary>
	/// Gets the <see cref="RootRenderTree"/> that all components rendered with the
	/// <c>RenderComponent&lt;TComponent&gt;()</c> methods, are rendered inside.
	/// </summary>
	/// <remarks>
	/// Use this to add default layout- or root-components which a component under test
	/// should be rendered under.
	/// </remarks>
	public RootRenderTree RenderTree { get; } = new();

	/// <summary>
	/// Gets the <see cref="ComponentFactoryCollection"/>. Factories added to it
	/// will be used to create components during testing, starting with the last added
	/// factory. If no factories in the collection can create a requested component,
	/// then the default Blazor factory is used.
	/// </summary>
	public ComponentFactoryCollection ComponentFactories { get; } = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="TestContext"/> class.
	/// </summary>
	public TestContext()
	{
		Services = new TestServiceProvider();
		Services.AddSingleton<ComponentFactoryCollection>(_ => ComponentFactories);
		Services.AddDefaultTestContextServices(this, JSInterop);
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Disposes of the test context resources, in particular it disposes the <see cref="Services"/>
	/// service provider. Any async services registered with the service provider will disposed first,
	/// but their disposal will not be awaited..
	/// </summary>
	/// <remarks>
	/// The disposing parameter should be false when called from a finalizer, and true when called from the
	/// <see cref="Dispose()"/> method. In other words, it is true when deterministically called and false when non-deterministically called.
	/// </remarks>
	/// <param name="disposing">Set to true if called from <see cref="Dispose()"/>, false if called from a finalizer.f.</param>
	[SuppressMessage("Reliability", "CA2012:Use ValueTasks correctly", Justification = "Explicitly ignoring DisposeAsync to avoid breaking changes to API surface.")]
	protected virtual void Dispose(bool disposing)
	{
		if (disposed || !disposing)
			return;

		disposed = true;

		// Ensure the renderer is disposed before all others,
		// otherwise a render cycle may be ongoing and try to access
		// the service provider to perform operations.
		if (testRenderer is IDisposable renderer)
		{
			renderer.Dispose();
		}

		// Ignore the async task as GetAwaiter().GetResult() can cause deadlock
		// and implementing IAsyncDisposable in TestContext will be a breaking change.
		//
		// NOTE: This has to be called before Services.Dispose().
		// If there are IAsyncDisposable services registered, calling Dispose first
		// causes the service provider to throw an exception.
		_ = Services.DisposeAsync();

		// The service provider should dispose of any
		// disposable object it has created, when it is disposed.
		Services.Dispose();
	}

	/// <summary>
	/// Disposes all components rendered via this <see cref="TestContext"/>.
	/// </summary>
	public void DisposeComponents()
	{
		Renderer.DisposeComponents();
	}

	/// <summary>
	/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of the component to render.</typeparam>
	/// <param name="parameters">Parameters to pass to the component when it is rendered.</param>
	/// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
	public virtual RenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters)
		where TComponent : IComponent
	{
		var renderFragment = new ComponentParameterCollection { parameters }
			.ToRenderFragment<TComponent>();

		return Render<TComponent>(renderFragment);
	}

	/// <summary>
	/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of the component to render.</typeparam>
	/// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters to pass to the component when it is rendered.</param>
	/// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
	public virtual RenderedComponent<TComponent> RenderComponent<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder = null)
		where TComponent : IComponent
	{
		var renderFragment = new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder)
			.Build()
			.ToRenderFragment<TComponent>();

		return Render<TComponent>(renderFragment);
	}

	/// <summary>
	/// Renders the <paramref name="renderFragment"/> and returns the first <typeparamref name="TComponent"/> in the resulting render tree.
	/// </summary>
	/// <remarks>
	/// Calling this method is equivalent to calling <c>Render(renderFragment).FindComponent&lt;TComponent&gt;()</c>.
	/// </remarks>
	/// <typeparam name="TComponent">The type of component to find in the render tree.</typeparam>
	/// <param name="renderFragment">The render fragment to render.</param>
	/// <returns>The <see cref="RenderedComponent{TComponent}"/>.</returns>
	public virtual RenderedComponent<TComponent> Render<TComponent>(RenderFragment renderFragment)
		where TComponent : IComponent
		=> this.RenderInsideRenderTree<TComponent>(renderFragment);

	/// <summary>
	/// Renders the <paramref name="renderFragment"/> and returns it as a <see cref="RenderedFragment"/>.
	/// </summary>
	/// <param name="renderFragment">The render fragment to render.</param>
	/// <returns>The <see cref="RenderedFragment"/>.</returns>
	public virtual RenderedFragment Render(RenderFragment renderFragment)
		=> this.RenderInsideRenderTree(renderFragment);

	/// <summary>
	/// Dummy method required to allow Blazor's compiler to generate
	/// C# from .razor files.
	/// </summary>
	protected virtual void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) { }

	private TestRenderer CreateTestRenderer()
	{
		var renderedComponentActivator = Services.GetRequiredService<IRenderedComponentActivator>();
		var logger = Services.GetRequiredService<ILoggerFactory>();

		var componentActivator = Services.GetService<IComponentActivator>();
		return componentActivator is null
			? new TestRenderer(renderedComponentActivator, Services, logger)
			: new TestRenderer(renderedComponentActivator, Services, logger, componentActivator);
	}
}
