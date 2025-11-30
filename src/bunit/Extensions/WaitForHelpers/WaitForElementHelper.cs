using AngleSharp.Dom;

namespace Bunit.Extensions.WaitForHelpers;

/// <summary>
/// Represents an async wait helper, that will wait for a specified time for an element to become available in the DOM.
/// </summary>
internal class WaitForElementHelper<TComponent> : WaitForElementHelper<TComponent, IElement>
	where TComponent : IComponent
{
	public WaitForElementHelper(IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan? timeout = null)
		: base(renderedComponent, cssSelector, timeout)
	{
	}
}

/// <summary>
/// Represents an async wait helper, that will wait for a specified time for an element of type <typeparamref name="TElement"/> to become available in the DOM.
/// </summary>
internal class WaitForElementHelper<TComponent, TElement> : WaitForHelper<TElement, TComponent>
	where TComponent : IComponent
	where TElement : class, IElement
{
	internal const string TimeoutBeforeFoundMessage = "The CSS selector and/or predicate did not result in a matching element before the timeout period passed.";

	/// <inheritdoc/>
	protected override string? TimeoutErrorMessage => TimeoutBeforeFoundMessage;

	/// <inheritdoc/>
	protected override bool StopWaitingOnCheckException => false;

	public WaitForElementHelper(IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan? timeout = null)
		: base(renderedComponent, () =>
		{
			var element = renderedComponent.Find<TComponent, TElement>(cssSelector);
			return (true, element);
		}, timeout)
	{
	}
}
