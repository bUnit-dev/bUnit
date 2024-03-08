using Bunit.Rendering;

namespace Bunit.Extensions;

/// <summary>
/// Extensions methods for <see cref="BunitContext"/> types.
/// </summary>
public static class BunitContextRenderExtensions
{
	/// <summary>
	/// Renders a component, declared in the <paramref name="renderFragment"/>, inside the <see cref="BunitContext.RenderTree"/>.
	/// </summary>
	/// <typeparam name="TComponent">The type of component to render.</typeparam>
	/// <param name="bunitContext">Test context to use to render with.</param>
	/// <param name="renderFragment">The <see cref="RenderInsideRenderTree"/> that contains a declaration of the component.</param>
	/// <returns>A <see cref="RenderedComponent{TComponent}"/>.</returns>
	public static RenderedComponent<TComponent> RenderInsideRenderTree<TComponent>(this BunitContext bunitContext, RenderFragment renderFragment)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(bunitContext);

		var baseResult = RenderInsideRenderTree(bunitContext, renderFragment);
		return bunitContext.Renderer.FindComponent<TComponent>(baseResult);
	}

	/// <summary>
	/// Renders a fragment, declared in the <paramref name="renderFragment"/>, inside the <see cref="BunitContext.RenderTree"/>.
	/// </summary>
	/// <param name="bunitContext">Test context to use to render with.</param>
	/// <param name="renderFragment">The <see cref="RenderInsideRenderTree"/> to render.</param>
	/// <returns>A <see cref="RenderedFragment"/>.</returns>
	public static RenderedFragment RenderInsideRenderTree(this BunitContext bunitContext, RenderFragment renderFragment)
	{
		ArgumentNullException.ThrowIfNull(bunitContext);

		// Wrap fragment in a FragmentContainer so the start of the test supplied
		// razor fragment can be found after, and then wrap in any layout components
		// added to the test context.
		var wrappedInFragmentContainer = FragmentContainer.Wrap(renderFragment);
		var wrappedInRenderTree = bunitContext.RenderTree.Wrap(wrappedInFragmentContainer);
		var resultBase = bunitContext.Renderer.RenderFragment(wrappedInRenderTree);

		return bunitContext.Renderer.FindComponent<FragmentContainer>(resultBase);
	}
}
