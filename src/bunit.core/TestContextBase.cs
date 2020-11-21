using System;
using Bunit.RazorTesting;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	/// <summary>
	/// A test context is a factory that makes it possible to create components under tests.
	/// </summary>
	public abstract class TestContextBase : IDisposable
	{
		private bool _disposed;
		private ITestRenderer? _testRenderer;

		/// <summary>
		/// Gets the renderer used by the test context.
		/// </summary>
		public ITestRenderer Renderer
		{
			get
			{
				if (_testRenderer is null)
				{
					_testRenderer = Services.GetRequiredService<ITestRenderer>();
				}
				return _testRenderer;
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
		public RootRenderTree RenderTree { get; } = new RootRenderTree();

		/// <summary>
		/// Creates a new instance of the <see cref="TestContextBase"/> class.
		/// </summary>
		protected TestContextBase()
		{
			Services = new TestServiceProvider();			
		}

		/// <summary>
		/// Renders a component, declared in the <paramref name="renderFragment"/>, inside the <see cref="RenderTree"/>.
		/// </summary>
		/// <typeparam name="TComponent">The type of component to render.</typeparam>
		/// <param name="renderFragment">The <see cref="RenderFragmentBase"/> that contains a declaration of the component.</param>
		/// <returns>A <see cref="IRenderedComponentBase{TComponent}"/>.</returns>
		protected IRenderedComponentBase<TComponent> RenderComponentBase<TComponent>(RenderFragment renderFragment) where TComponent : IComponent
		{
			// Wrap TComponent in any layout components added to the test context.
			// If one of the layout components is the same type as TComponent,
			// make sure to return the rendered component, not the layout component.			
			var resultBase = Renderer.RenderFragment(RenderTree.Wrap(renderFragment));

			// This ensures that the correct component is returned, in case an added layout component
			// is of type TComponent.
			var renderTreeTComponentCount = RenderTree.GetCountOf<TComponent>();
			var result = renderTreeTComponentCount > 0
				? Renderer.FindComponents<TComponent>(resultBase)[renderTreeTComponentCount]
				: Renderer.FindComponent<TComponent>(resultBase);

			return result;
		}

		/// <summary>
		/// Renders a fragment, declared in the <paramref name="renderFragment"/>, inside the <see cref="RenderTree"/>.
		/// </summary>
		/// <param name="renderFragment">The <see cref="RenderFragmentBase"/> to render.</param>
		/// <returns>A <see cref="IRenderedFragmentBase"/>.</returns>
		protected IRenderedFragmentBase RenderFragmentBase(RenderFragment renderFragment)
		{
			// Wrap fragment in a FragmentContainer so the start of the test supplied
			// razor fragment can be found after, and then wrap in any layout components
			// added to the test context.		
			var wrappedInFragmentContainer = FragmentContainer.Wrap(renderFragment);
			var wrappedInRenderTree = RenderTree.Wrap(wrappedInFragmentContainer);
			var resultBase = Renderer.RenderFragment(wrappedInRenderTree);

			return Renderer.FindComponent<FragmentContainer>(resultBase);
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes of the test context resources.
		/// </summary>
		/// <remarks>
		/// The disposing parameter should be false when called from a finalizer, and true when called from the
		/// <see cref="Dispose()"/> method. In other words, it is true when deterministically called and false when non-deterministically called.
		/// </remarks>
		/// <param name="disposing">Set to true if called from <see cref="Dispose()"/>, false if called from a finalizer.f</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed || !disposing)
				return;

			_disposed = true;

			// The service provider should dispose of any
			// disposable object it has created, when it is disposed.
			Services.Dispose();
		}
	}
}
