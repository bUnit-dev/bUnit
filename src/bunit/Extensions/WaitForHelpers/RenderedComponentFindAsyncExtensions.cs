using AngleSharp.Dom;
using Bunit.Extensions.WaitForHelpers;

namespace Bunit;

/// <summary>
/// Helper methods that deal with asynchronous retrieval of elements from a rendered component.
/// </summary>
public static class RenderedComponentFindAsyncExtensions
{
	/// <summary>
	/// Wait until an element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <see cref="IElement"/>.</returns>
	public static Task<IElement> FindAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent => FindCoreAsync(renderedComponent, cssSelector, timeout: null);

	/// <summary>
	/// Wait until an element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <param name="timeout">The maximum time to wait for the element to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <see cref="IElement"/>.</returns>
	public static Task<IElement> FindAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan timeout)
		where TComponent : IComponent
		=> FindCoreAsync(renderedComponent, cssSelector, timeout: timeout);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	public static Task<IReadOnlyList<IElement>> FindAllAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, int matchElementCount)
		where TComponent : IComponent
		=> FindAllCoreAsync(renderedComponent, cssSelector, matchElementCount: matchElementCount, timeout: null);

	/// <summary>
	/// Wait until at least one element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	public static Task<IReadOnlyList<IElement>> FindAllAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan timeout)
		where TComponent : IComponent
		=> FindAllCoreAsync(renderedComponent, cssSelector, matchElementCount: null, timeout: timeout);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	public static Task<IReadOnlyList<IElement>> FindAllAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, int matchElementCount, TimeSpan timeout)
		where TComponent : IComponent
		=> FindAllCoreAsync(renderedComponent, cssSelector, matchElementCount: matchElementCount, timeout: timeout);

	/// <summary>
	/// Wait until at least one element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	public static Task<IReadOnlyList<IElement>> FindAllAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent
		=> FindAllCoreAsync(renderedComponent, cssSelector, matchElementCount: null, timeout: null);

	private static async Task<IElement> FindCoreAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan? timeout)
		where TComponent : IComponent
	{
		using var waiter = new WaitForElementHelper<TComponent>(renderedComponent, cssSelector, timeout);

		return await waiter.WaitTask;
	}


	private static async Task<IReadOnlyList<IElement>> FindAllCoreAsync<TComponent>(
		this IRenderedComponent<TComponent> renderedComponent,
		string cssSelector,
		int? matchElementCount,
		TimeSpan? timeout)
		where TComponent : IComponent
	{
		using var waiter = new WaitForElementsHelper<TComponent>(renderedComponent, cssSelector, matchElementCount, timeout);

		return (await waiter.WaitTask).ToArray();
	}
}
