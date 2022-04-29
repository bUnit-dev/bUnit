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
	/// Gets the renderer used by the test context.
	/// </summary>
	public ITestRenderer Renderer
	{
		get
		{
			if (testRenderer is null)
			{
				testRenderer = Services.GetRequiredService<ITestRenderer>();
			}

			return testRenderer;
		}
	}

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

#if NET5_0_OR_GREATER
	/// <summary>
	/// Gets the <see cref="ComponentFactoryCollection"/>. Factories added to it
	/// will be used to create components during testing, starting with the last added
	/// factory. If no factories in the collection can create a requested component,
	/// then the default Blazor factory is used.
	/// </summary>
	public ComponentFactoryCollection ComponentFactories { get; } = new();
#endif

	/// <summary>
	/// Initializes a new instance of the <see cref="TestContextBase"/> class.
	/// </summary>
	protected TestContextBase()
	{
		Services = new TestServiceProvider();
#if NET5_0_OR_GREATER
		Services.AddSingleton<IComponentActivator>(new BunitComponentActivator(ComponentFactories));
#endif
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Disposes of the test context resources.
	/// </summary>
	/// <remarks>
	/// The disposing parameter should be false when called from a finalizer, and true when called from the
	/// <see cref="Dispose()"/> method. In other words, it is true when deterministically called and false when non-deterministically called.
	/// </remarks>
	/// <param name="disposing">Set to true if called from <see cref="Dispose()"/>, false if called from a finalizer.f.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (disposed || !disposing)
			return;

		disposed = true;

		// The service provider should dispose of any
		// disposable object it has created, when it is disposed.
		Services.Dispose();
	}

	/// <summary>
	/// Disposes all components rendered via this <see cref="TestContextBase"/>.
	/// </summary>
	public void DisposeComponents()
	{
		Renderer.DisposeComponents();
	}
}
