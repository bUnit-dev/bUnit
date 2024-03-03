using Bunit.Rendering;

namespace Bunit.Extensions;

/// <summary>
/// Extensions methods for <see cref="TestContext"/> types.
/// </summary>
public static class TestContextRenderExtensions
{
	/// <summary>
	/// Renders a component, declared in the <paramref name="renderFragment"/>, inside the <see cref="TestContext.RenderTree"/>.
	/// </summary>
	/// <typeparam name="TComponent">The type of component to render.</typeparam>
	/// <param name="testContext">Test context to use to render with.</param>
	/// <param name="renderFragment">The <see cref="RenderInsideRenderTree"/> that contains a declaration of the component.</param>
	/// <returns>A <see cref="IRenderedComponent{TComponent}"/>.</returns>
	public static IRenderedComponent<TComponent> RenderInsideRenderTree<TComponent>(this TestContext testContext, RenderFragment renderFragment)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(testContext);

		var baseResult = RenderInsideRenderTree(testContext, renderFragment);
		return testContext.Renderer.FindComponent<TComponent>(baseResult);
	}

	/// <summary>
	/// Renders a fragment, declared in the <paramref name="renderFragment"/>, inside the <see cref="TestContext.RenderTree"/>.
	/// </summary>
	/// <param name="testContext">Test context to use to render with.</param>
	/// <param name="renderFragment">The <see cref="RenderInsideRenderTree"/> to render.</param>
	/// <returns>A <see cref="IRenderedFragment"/>.</returns>
	public static IRenderedFragment RenderInsideRenderTree(this TestContext testContext, RenderFragment renderFragment)
	{
		ArgumentNullException.ThrowIfNull(testContext);

		// Wrap fragment in a FragmentContainer so the start of the test supplied
		// razor fragment can be found after, and then wrap in any layout components
		// added to the test context.
		var wrappedInFragmentContainer = FragmentContainer.Wrap(renderFragment);
		var wrappedInRenderTree = testContext.RenderTree.Wrap(wrappedInFragmentContainer);
		var resultBase = testContext.Renderer.RenderFragment(wrappedInRenderTree);

		return testContext.Renderer.FindComponent<FragmentContainer>(resultBase);
	}
}
