using AngleSharp.Dom;

namespace Bunit.Extensions.WaitForHelpers;

/// <summary>
/// Represents an async wait helper, that will wait for a specified time for an element to become available in the DOM.
/// </summary>
internal class WaitForElementHelper<TComponent> : WaitForHelper<IElement, TComponent>
	where TComponent : IComponent
{
	internal const string TimeoutBeforeFoundMessage = "The CSS selector and/or predicate did not result in a matching element before the timeout period passed.";

	/// <inheritdoc/>
	protected override string? TimeoutErrorMessage => TimeoutBeforeFoundMessage;

	/// <inheritdoc/>
	protected override bool StopWaitingOnCheckException => false;

	public WaitForElementHelper(IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan? timeout = null)
		: base(renderedComponent, () =>
		{
			var element = renderedComponent.Find(cssSelector);
			return (true, element);
		}, timeout)
	{
	}
}
