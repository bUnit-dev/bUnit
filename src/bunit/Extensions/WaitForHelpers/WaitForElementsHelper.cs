using System.Globalization;
using System.Text;
using AngleSharp.Dom;
using Bunit.Web.AngleSharp;

namespace Bunit.Extensions.WaitForHelpers;

/// <summary>
/// Represents an async wait helper, that will wait for a specified time for element(s) to become available in the DOM.
/// </summary>
internal class WaitForElementsHelper<TComponent> : WaitForHelper<IHtmlCollection<IElement>, TComponent>
	where TComponent : IComponent
{
	internal const string TimeoutBeforeFoundMessage = "The CSS selector did not result in any matching element(s) before the timeout period passed.";
	internal static readonly CompositeFormat TimeoutBeforeFoundWithCountMessage = CompositeFormat.Parse("The CSS selector did not result in exactly {0} matching element(s) before the timeout period passed.");
	private readonly int? matchElementCount;

	/// <inheritdoc/>
	protected override string? TimeoutErrorMessage => matchElementCount is null
		? TimeoutBeforeFoundMessage
		: string.Format(CultureInfo.InvariantCulture, TimeoutBeforeFoundWithCountMessage, matchElementCount);

	/// <inheritdoc/>
	protected override bool StopWaitingOnCheckException => true;

	public WaitForElementsHelper(IRenderedComponent<TComponent> renderedComponent, string cssSelector, int? matchElementCount, TimeSpan? timeout = null)
		: base(renderedComponent, () =>
		{
			var elements = FindAllElements(renderedComponent, cssSelector);

			var checkPassed = matchElementCount is null
				? elements.Length > 0
				: elements.Length == matchElementCount;

			return (checkPassed, elements);
		}, timeout)
	{
		this.matchElementCount = matchElementCount;
	}

	private static IHtmlCollection<IElement> FindAllElements(IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		=> renderedComponent.Nodes.QuerySelectorAll(cssSelector);
}
