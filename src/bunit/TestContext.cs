using Bunit.Extensions;
using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// A test context is a factory that makes it possible to create components under tests.
/// </summary>
public partial class TestContext : IDisposable, IAsyncDisposable
{
	private bool disposed;
	private BunitRenderer? testRenderer;

	/// <summary>
	/// Gets or sets the default wait timeout used by "WaitFor" operations.
	/// </summary>
	/// <remarks>The default is 30 seconds.</remarks>
	public static TimeSpan DefaultWaitTimeout { get; set; } = TimeSpan.FromSeconds(30);

	/// <summary>
	/// Gets the renderer used by the test context.
	/// </summary>
	public BunitRenderer Renderer => testRenderer ??= Services.GetRequiredService<BunitRenderer>();

	/// <summary>
	/// Gets the service collection and service provider that is used when a
	/// component is rendered by the test context.
	/// </summary>
	public TestServiceProvider Services { get; }

	/// <summary>
	/// Gets the <see cref="RootRenderTree"/> that all components rendered with the
	/// <c>Render&lt;TComponent&gt;()</c> methods, are rendered inside.
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
	/// Gets bUnits JSInterop, that allows setting up handlers for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> invocations
	/// that components under tests will issue during testing. It also makes it possible to verify that the invocations has happened as expected.
	/// </summary>
	public BunitJSInterop JSInterop { get; } = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="TestContext"/> class.
	/// </summary>
	public TestContext()
	{
		Services = new TestServiceProvider();
		Services.AddSingleton<ComponentFactoryCollection>(_ => ComponentFactories);
		Services.AddDefaultTestContextServices(this, JSInterop);
	}

	/// <summary>
	/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of the component to render.</typeparam>
	/// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters to pass to the component when it is rendered.</param>
	/// <returns>The rendered <typeparamref name="TComponent"/>.</returns>
	public virtual IRenderedComponent<TComponent> Render<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder = null)
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
	/// <returns>The <see cref="IRenderedComponent{TComponent}"/>.</returns>
	public virtual IRenderedComponent<TComponent> Render<TComponent>(RenderFragment renderFragment)
		where TComponent : IComponent
		=> this.RenderInsideRenderTree<TComponent>(renderFragment);

	/// <summary>
	/// Renders the <paramref name="renderFragment"/> and returns it as a <see cref="IRenderedFragment"/>.
	/// </summary>
	/// <param name="renderFragment">The render fragment to render.</param>
	/// <returns>The <see cref="IRenderedFragment"/>.</returns>
	public virtual IRenderedFragment Render(RenderFragment renderFragment)
		=> this.RenderInsideRenderTree(renderFragment);

	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <inheritdoc/>
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		Dispose(disposing: false);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Disposes of the test context resources that are asynchronous, in particular it disposes the <see cref="Services"/>
	/// service provider.s
	/// </summary>
	protected virtual async ValueTask DisposeAsyncCore()
	{
		if (disposed)
			return;

		disposed = true;

		await Services.DisposeAsync();
	}

	/// <summary>
	/// Disposes of the test context resources, in particular it disposes the <see cref="Services"/>
	/// </summary>
	/// <remarks>
	/// The disposing parameter should be false when called from a finalizer, and true when called from the
	/// <see cref="Dispose()"/> method. In other words, it is true when deterministically called and false when non-deterministically called.
	/// </remarks>
	/// <param name="disposing">Set to true if called from <see cref="Dispose()"/>, false if called from a finalizer.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (disposed || !disposing)
			return;

		disposed = true;

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
	/// Dummy method required to allow Blazor's compiler to generate
	/// C# from .razor files.
	/// </summary>
	protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }
}
