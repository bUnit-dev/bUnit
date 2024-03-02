using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// A test context is a factory that makes it possible to create components under tests.
/// </summary>
public abstract class TestContextBase : IDisposable
{
	private bool disposed;
	private ITestRenderer? testRenderer;

	/// <summary>
	/// Gets or sets the default wait timeout used by "WaitFor" operations, i.e. <see cref="RenderedFragmentWaitForHelperExtensions.WaitForAssertion(IRenderedFragmentBase, Action, TimeSpan?)"/>.
	/// </summary>
	/// <remarks>The default is 1 second.</remarks>
	public static TimeSpan DefaultWaitTimeout { get; set; } = TimeSpan.FromSeconds(1);

	/// <summary>
	/// Gets the renderer used by the test context.
	/// </summary>
	public ITestRenderer Renderer => testRenderer ??= CreateTestRenderer();

	/// <summary>
	/// Creates the renderer.
	/// </summary>
	protected abstract ITestRenderer CreateTestRenderer();

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
	/// Initializes a new instance of the <see cref="TestContextBase"/> class.
	/// </summary>
	protected TestContextBase()
	{
		Services = new TestServiceProvider();
		Services.AddSingleton<ComponentFactoryCollection>(_ => ComponentFactories);
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
#pragma warning disable S3966 // On purpose to dispose synchronously and asynchronously
		Services.Dispose();
#pragma warning restore S3966
	}

	/// <summary>
	/// Disposes all components rendered via this <see cref="TestContextBase"/>.
	/// </summary>
	public void DisposeComponents()
	{
		Renderer.DisposeComponents();
	}
}
