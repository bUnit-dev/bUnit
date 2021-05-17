using System;
using Bunit.ComponentFactories;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Bunit.Extensions
{
	/// <summary>
	/// Extensions methods for <see cref="TestContextBase"/> types.
	/// </summary>
	public static class TestContextBaseExtensions
	{
		/// <summary>
		/// Renders a component, declared in the <paramref name="renderFragment"/>, inside the <see cref="TestContextBase.RenderTree"/>.
		/// </summary>
		/// <typeparam name="TComponent">The type of component to render.</typeparam>
		/// <param name="testContext">Test context to use to render with.</param>
		/// <param name="renderFragment">The <see cref="RenderInsideRenderTree"/> that contains a declaration of the component.</param>
		/// <returns>A <see cref="IRenderedComponentBase{TComponent}"/>.</returns>
		public static IRenderedComponentBase<TComponent> RenderInsideRenderTree<TComponent>(this TestContextBase testContext, RenderFragment renderFragment)
			where TComponent : IComponent
		{
			if (testContext is null)
				throw new ArgumentNullException(nameof(testContext));

			// Wrap TComponent in any layout components added to the test context.
			// If one of the layout components is the same type as TComponent,
			// make sure to return the rendered component, not the layout component.
			var resultBase = testContext.Renderer.RenderFragment(testContext.RenderTree.Wrap(renderFragment));

			// This ensures that the correct component is returned, in case an added layout component
			// is of type TComponent.
			var renderTreeTComponentCount = testContext.RenderTree.GetCountOf<TComponent>();
			var result = renderTreeTComponentCount > 0
				? testContext.Renderer.FindComponents<TComponent>(resultBase)[renderTreeTComponentCount]
				: testContext.Renderer.FindComponent<TComponent>(resultBase);

			return result;
		}

		/// <summary>
		/// Renders a fragment, declared in the <paramref name="renderFragment"/>, inside the <see cref="TestContextBase.RenderTree"/>.
		/// </summary>
		/// <param name="testContext">Test context to use to render with.</param>
		/// <param name="renderFragment">The <see cref="RenderInsideRenderTree"/> to render.</param>
		/// <returns>A <see cref="IRenderedFragmentBase"/>.</returns>
		public static IRenderedFragmentBase RenderInsideRenderTree(this TestContextBase testContext, RenderFragment renderFragment)
		{
			if (testContext is null)
				throw new ArgumentNullException(nameof(testContext));

			// Wrap fragment in a FragmentContainer so the start of the test supplied
			// razor fragment can be found after, and then wrap in any layout components
			// added to the test context.
			var wrappedInFragmentContainer = FragmentContainer.Wrap(renderFragment);
			var wrappedInRenderTree = testContext.RenderTree.Wrap(wrappedInFragmentContainer);
			var resultBase = testContext.Renderer.RenderFragment(wrappedInRenderTree);

			return testContext.Renderer.FindComponent<FragmentContainer>(resultBase);
		}

#if NET5_0_OR_GREATER
		/// <summary>
		/// Renders the first component(root component) in the<paramref name="renderFragment"/>,
		/// and stubs out all other components, both child components as well as sibling components.
		/// </summary>
		/// <param name="testContext">Test context to use to render with.</param>
		/// <param name="renderFragment">The <see cref="RenderInsideRenderTree"/> to render.</param>
		/// <returns>A <see cref="IRenderedFragmentBase"/>.</returns>
		public static IRenderedFragmentBase ShallowRenderInsideRenderTree(this TestContextBase testContext, RenderFragment renderFragment)
		{
			if (testContext is null)
				throw new ArgumentNullException(nameof(testContext));

			testContext.ComponentFactories.Add(new ShallowRenderComponentFactory());
			return RenderInsideRenderTree(testContext, renderFragment);
		}
#endif
	}
}
