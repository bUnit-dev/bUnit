using System;
using System.Linq;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Bunit.Extensions
{
	/// <summary>
	/// Extensions methods for <see cref="TestContextBase"/> types.
	/// </summary>
	public static class TestContextBaseRenderExtensions
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

			var baseResult = RenderInsideRenderTree(testContext, renderFragment);
			return testContext.Renderer.FindComponent<TComponent>(baseResult);
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

#if NET5_0_OR_GREATER
			EnsureNoPreviousShallowRender(testContext);
#endif

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
		/// Renders a component, declared in the <paramref name="renderFragment"/>, inside the <see cref="TestContextBase.RenderTree"/>.
		/// </summary>
		/// <typeparam name="TComponent">The type of component to render.</typeparam>
		/// <param name="testContext">Test context to use to render with.</param>
		/// <param name="renderFragment">The <see cref="RenderInsideRenderTree"/> that contains a declaration of the component.</param>
		/// <param name="options">Render options for the <see cref="TestDoubles.Stub{TComponent}"/> components.</param>
		/// <returns>A <see cref="IRenderedComponentBase{TComponent}"/>.</returns>
		public static IRenderedComponentBase<TComponent> ShallowRenderInsideRenderTree<TComponent>(this TestContextBase testContext, RenderFragment renderFragment, TestDoubles.StubOptions? options = null)
			where TComponent : IComponent
		{
			if (testContext is null)
				throw new ArgumentNullException(nameof(testContext));

			var baseResult = ShallowRenderInsideRenderTree(testContext, renderFragment);
			return testContext.Renderer.FindComponent<TComponent>(baseResult);
		}

		/// <summary>
		/// Renders the first component(root component) in the<paramref name="renderFragment"/>,
		/// and stubs out all other components, both child components as well as sibling components.
		/// </summary>
		/// <param name="testContext">Test context to use to render with.</param>
		/// <param name="renderFragment">The <see cref="RenderInsideRenderTree"/> to render.</param>
		/// <param name="options">Render options for the <see cref="TestDoubles.Stub{TComponent}"/> components.</param>
		/// <returns>A <see cref="IRenderedFragmentBase"/>.</returns>
		public static IRenderedFragmentBase ShallowRenderInsideRenderTree(this TestContextBase testContext, RenderFragment renderFragment, TestDoubles.StubOptions? options = null)
		{
			if (testContext is null)
				throw new ArgumentNullException(nameof(testContext));

			testContext.ComponentFactories.Add(new ComponentFactories.ShallowRenderComponentFactory(options));
			var wrappedInShallowRenderContainer = ShallowRenderContainer.Wrap(renderFragment);
			return RenderInsideRenderTree(testContext, wrappedInShallowRenderContainer);
		}

		private static void EnsureNoPreviousShallowRender(TestContextBase testContext)
		{
			if (testContext.ComponentFactories.Any(x => x is ComponentFactories.ShallowRenderComponentFactory cf && cf.HasShallowRendered))
			{
				throw new InvalidOperationException("The test context has previously been used to perform a shallow render of " +
													"a component or render fragment. " +
													"Rendering another component or render fragment with the same test context " +
													"is not currently supported, since it can result in unpredictable results.");
			}
		}
#endif
	}
}
