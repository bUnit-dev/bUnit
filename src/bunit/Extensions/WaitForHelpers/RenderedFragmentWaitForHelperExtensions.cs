using System.Runtime.ExceptionServices;
using AngleSharp.Dom;
using Bunit.Asserting;
using Bunit.Extensions.WaitForHelpers;

namespace Bunit;

/// <summary>
/// Helper methods dealing with async rendering during testing.
/// </summary>
public static class RenderedFragmentWaitForHelperExtensions
{
	/// <summary>
	/// Wait until the provided <paramref name="statePredicate"/> action returns true,
	/// or the <paramref name="timeout"/> is reached (default is one second).
	///
	/// The <paramref name="statePredicate"/> is evaluated initially, and then each time
	/// the <paramref name="renderedFragment"/> renders.
	/// </summary>
	/// <param name="renderedFragment">The render fragment or component to attempt to verify state against.</param>
	/// <param name="statePredicate">The predicate to invoke after each render, which must returns <c>true</c> when the desired state has been reached.</param>
	/// <param name="timeout">The maximum time to wait for the desired state.</param>
	/// <exception cref="WaitForFailedException">Thrown if the <paramref name="statePredicate"/> throw an exception during invocation, or if the timeout has been reached. See the inner exception for details.</exception>
	public static async Task WaitForStateAsync(this IRenderedFragment renderedFragment, Func<bool> statePredicate, TimeSpan? timeout = null)
	{
		using var waiter = new WaitForStateHelper(renderedFragment, statePredicate, timeout);

		await waiter.WaitTask;
	}

	/// <summary>
	/// Wait until the provided <paramref name="assertion"/> passes (i.e. does not throw an
	/// exception), or the <paramref name="timeout"/> is reached (default is one second).
	///
	/// The <paramref name="assertion"/> is attempted initially, and then each time the <paramref name="renderedFragment"/> renders.
	/// </summary>
	/// <param name="renderedFragment">The rendered fragment to wait for renders from and assert against.</param>
	/// <param name="assertion">The verification or assertion to perform.</param>
	/// <param name="timeout">The maximum time to attempt the verification.</param>
	/// <exception cref="WaitForFailedException">Thrown if the timeout has been reached. See the inner exception to see the captured assertion exception.</exception>
	[AssertionMethod]
	public static async Task WaitForAssertionAsync(this IRenderedFragment renderedFragment, Action assertion, TimeSpan? timeout = null)
	{
		using var waiter = new WaitForAssertionHelper(renderedFragment, assertion, timeout);

		await waiter.WaitTask;
	}

	/// <summary>
	/// Wait until an element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedFragment"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedFragment">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <see cref="IElement"/>.</returns>
	public static Task<IElement> WaitForElementAsync(this IRenderedFragment renderedFragment, string cssSelector)
		=> WaitForElementCoreAsync(renderedFragment, cssSelector, timeout: null);

	/// <summary>
	/// Wait until an element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedFragment"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedFragment">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <param name="timeout">The maximum time to wait for the element to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <see cref="IElement"/>.</returns>
	internal static Task<IElement> WaitForElementAsync(this IRenderedFragment renderedFragment, string cssSelector, TimeSpan timeout)
		=> WaitForElementCoreAsync(renderedFragment, cssSelector, timeout: timeout);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedFragment"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedFragment">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IRefreshableElementCollection{IElement}"/>.</returns>
	internal static Task<IRefreshableElementCollection<IElement>> WaitForElementsAsync(this IRenderedFragment renderedFragment, string cssSelector, int matchElementCount)
		=> WaitForElementsCoreAsync(renderedFragment, cssSelector, matchElementCount: matchElementCount, timeout: null);

	/// <summary>
	/// Wait until at least one element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedFragment"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedFragment">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IRefreshableElementCollection{IElement}"/>.</returns>
	internal static Task<IRefreshableElementCollection<IElement>> WaitForElementsAsync(this IRenderedFragment renderedFragment, string cssSelector, TimeSpan timeout)
		=> WaitForElementsCoreAsync(renderedFragment, cssSelector, matchElementCount: null, timeout: timeout);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedFragment"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedFragment">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IRefreshableElementCollection{IElement}"/>.</returns>
	internal static Task<IRefreshableElementCollection<IElement>> WaitForElementsAsync(this IRenderedFragment renderedFragment, string cssSelector, int matchElementCount, TimeSpan timeout)
		=> WaitForElementsCoreAsync(renderedFragment, cssSelector, matchElementCount: matchElementCount, timeout: timeout);

	/// <summary>
	/// Wait until at least one element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedFragment"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedFragment">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IRefreshableElementCollection{IElement}"/>.</returns>
	public static Task<IRefreshableElementCollection<IElement>> WaitForElementsAsync(this IRenderedFragment renderedFragment, string cssSelector)
		=> WaitForElementsCoreAsync(renderedFragment, cssSelector, matchElementCount: null, timeout: null);

	private static async Task<IElement> WaitForElementCoreAsync(this IRenderedFragment renderedFragment, string cssSelector, TimeSpan? timeout)
	{
		using var waiter = new WaitForElementHelper(renderedFragment, cssSelector, timeout);

		return await waiter.WaitTask;
	}

	private static async Task<IRefreshableElementCollection<IElement>> WaitForElementsCoreAsync(
		this IRenderedFragment renderedFragment,
		string cssSelector,
		int? matchElementCount,
		TimeSpan? timeout)
	{
		using var waiter = new WaitForElementsHelper(renderedFragment, cssSelector, matchElementCount, timeout);

		return await waiter.WaitTask;
	}
}
