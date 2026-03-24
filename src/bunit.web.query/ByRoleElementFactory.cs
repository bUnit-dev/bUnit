using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit;

internal sealed class ByRoleElementFactory : IElementWrapperFactory
{
	private readonly IRenderedComponent<IComponent> testTarget;
	private readonly AriaRole role;
	private readonly ByRoleOptions options;

	public Action? OnElementReplaced { get; set; }

	public ByRoleElementFactory(IRenderedComponent<IComponent> testTarget, AriaRole role, ByRoleOptions options)
	{
		this.testTarget = testTarget;
		this.role = role;
		this.options = options;
		testTarget.OnMarkupUpdated += FragmentsMarkupUpdated;
	}

	private void FragmentsMarkupUpdated(object? sender, EventArgs args)
		=> OnElementReplaced?.Invoke();

	public TElement GetElement<TElement>() where TElement : class, IElement
	{
		var element = testTarget.FindByRoleInternal(role, options) as TElement;

		return element ?? throw new ElementRemovedFromDomException(role.ToRoleString());
	}
}
