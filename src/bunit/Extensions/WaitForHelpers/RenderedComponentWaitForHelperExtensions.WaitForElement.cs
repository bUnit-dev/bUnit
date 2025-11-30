using System.Runtime.ExceptionServices;
using AngleSharp.Dom;
using Bunit.Extensions.WaitForHelpers;

namespace Bunit;

/// <summary>
/// Helper methods dealing with async rendering during testing.
/// </summary>
public static partial class RenderedComponentWaitForHelperExtensions
{
	/// <summary>
	/// Wait until an element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <see cref="IElement"/>.</returns>
	public static IElement WaitForElement<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent => WaitForElementCore(renderedComponent, cssSelector, timeout: null);

	/// <summary>
	/// Wait until an element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <param name="timeout">The maximum time to wait for the element to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <see cref="IElement"/>.</returns>
	public static IElement WaitForElement<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan timeout)
		where TComponent : IComponent
		=> WaitForElementCore(renderedComponent, cssSelector, timeout: timeout);

	/// <summary>
	/// Wait until an element of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of element to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <typeparamref name="TElement"/>.</returns>
	public static TElement WaitForElement<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementCore<TComponent, TElement>(renderedComponent, cssSelector, timeout: null);

	/// <summary>
	/// Wait until an element of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of element to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <param name="timeout">The maximum time to wait for the element to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <typeparamref name="TElement"/>.</returns>
	public static TElement WaitForElement<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan timeout)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementCore<TComponent, TElement>(renderedComponent, cssSelector, timeout: timeout);

	/// <summary>
	/// Wait until at least one element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{IElement}"/>.</returns>
	public static IReadOnlyList<IElement> WaitForElements<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent => WaitForElementsCore(renderedComponent, cssSelector, matchElementCount: null, timeout: null);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{IElement}"/>.</returns>
	public static IReadOnlyList<IElement> WaitForElements<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, int matchElementCount)
		where TComponent : IComponent => WaitForElementsCore(renderedComponent, cssSelector, matchElementCount: matchElementCount, timeout: null);

	/// <summary>
	/// Wait until at least one element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{IElement}"/>.</returns>
	public static IReadOnlyList<IElement> WaitForElements<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan timeout)
		where TComponent : IComponent
		=> WaitForElementsCore(renderedComponent, cssSelector, matchElementCount: null, timeout: timeout);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{IElement}"/>.</returns>
	public static IReadOnlyList<IElement> WaitForElements<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, int matchElementCount, TimeSpan timeout)
		where TComponent : IComponent
		=> WaitForElementsCore(renderedComponent, cssSelector, matchElementCount: matchElementCount, timeout: timeout);

	/// <summary>
	/// Wait until at least one element of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of elements to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{TElement}"/>.</returns>
	public static IReadOnlyList<TElement> WaitForElements<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementsCore<TComponent, TElement>(renderedComponent, cssSelector, matchElementCount: null, timeout: null);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of elements to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{TElement}"/>.</returns>
	public static IReadOnlyList<TElement> WaitForElements<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, int matchElementCount)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementsCore<TComponent, TElement>(renderedComponent, cssSelector, matchElementCount: matchElementCount, timeout: null);

	/// <summary>
	/// Wait until at least one element of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of elements to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{TElement}"/>.</returns>
	public static IReadOnlyList<TElement> WaitForElements<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan timeout)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementsCore<TComponent, TElement>(renderedComponent, cssSelector, matchElementCount: null, timeout: timeout);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of elements to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{TElement}"/>.</returns>
	public static IReadOnlyList<TElement> WaitForElements<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, int matchElementCount, TimeSpan timeout)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementsCore<TComponent, TElement>(renderedComponent, cssSelector, matchElementCount: matchElementCount, timeout: timeout);

	/// <summary>
	/// Wait until an element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <see cref="IElement"/>.</returns>
	public static Task<IElement> WaitForElementAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent => WaitForElementCoreAsync(renderedComponent, cssSelector, timeout: null);

	/// <summary>
	/// Wait until an element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <param name="timeout">The maximum time to wait for the element to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <see cref="IElement"/>.</returns>
	public static Task<IElement> WaitForElementAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan timeout)
		where TComponent : IComponent
		=> WaitForElementCoreAsync(renderedComponent, cssSelector, timeout: timeout);

	/// <summary>
	/// Wait until an element of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of element to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <typeparamref name="TElement"/>.</returns>
	public static Task<TElement> WaitForElementAsync<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementCoreAsync<TComponent, TElement>(renderedComponent, cssSelector, timeout: null);

	/// <summary>
	/// Wait until an element of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of element to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
	/// <param name="timeout">The maximum time to wait for the element to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
	/// <returns>The <typeparamref name="TElement"/>.</returns>
	public static Task<TElement> WaitForElementAsync<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan timeout)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementCoreAsync<TComponent, TElement>(renderedComponent, cssSelector, timeout: timeout);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{IElement}"/>.</returns>
	public static Task<IReadOnlyList<IElement>> WaitForElementsAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, int matchElementCount)
		where TComponent : IComponent
		=> WaitForElementsCoreAsync(renderedComponent, cssSelector, matchElementCount: matchElementCount, timeout: null);

	/// <summary>
	/// Wait until at least one element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{IElement}"/>.</returns>
	public static Task<IReadOnlyList<IElement>> WaitForElementsAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan timeout)
		where TComponent : IComponent
		=> WaitForElementsCoreAsync(renderedComponent, cssSelector, matchElementCount: null, timeout: timeout);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{IElement}"/>.</returns>
	public static Task<IReadOnlyList<IElement>> WaitForElementsAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, int matchElementCount, TimeSpan timeout)
		where TComponent : IComponent
		=> WaitForElementsCoreAsync(renderedComponent, cssSelector, matchElementCount: matchElementCount, timeout: timeout);

	/// <summary>
	/// Wait until at least one element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{IElement}"/>.</returns>
	public static Task<IReadOnlyList<IElement>> WaitForElementsAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent
		=> WaitForElementsCoreAsync<TComponent, IElement>(renderedComponent, cssSelector, matchElementCount: null, timeout: null);
	/// <summary>
	/// Wait until at least one element of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of elements to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{TElement}"/>.</returns>
	public static Task<IReadOnlyList<TElement>> WaitForElementsAsync<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementsCoreAsync<TComponent, TElement>(renderedComponent, cssSelector, matchElementCount: null, timeout: null);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the timeout is reached (default is one second).
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of elements to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{TElement}"/>.</returns>
	public static Task<IReadOnlyList<TElement>> WaitForElementsAsync<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, int matchElementCount)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementsCoreAsync<TComponent, TElement>(renderedComponent, cssSelector, matchElementCount: matchElementCount, timeout: null);

	/// <summary>
	/// Wait until at least one element of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of elements to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{TElement}"/>.</returns>
	public static Task<IReadOnlyList<TElement>> WaitForElementsAsync<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan timeout)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementsCoreAsync<TComponent, TElement>(renderedComponent, cssSelector, matchElementCount: null, timeout: timeout);

	/// <summary>
	/// Wait until exactly <paramref name="matchElementCount"/> element(s) of type <typeparamref name="TElement"/> matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedComponent"/>,
	/// or the <paramref name="timeout"/> is reached.
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test.</typeparam>
	/// <typeparam name="TElement">The type of elements to wait for (e.g., IHtmlInputElement).</typeparam>
	/// <param name="renderedComponent">The render fragment or component find the matching element in.</param>
	/// <param name="cssSelector">The CSS selector to use to search for elements.</param>
	/// <param name="matchElementCount">The exact number of elements to that the <paramref name="cssSelector"/> should match.</param>
	/// <param name="timeout">The maximum time to wait for elements to appear.</param>
	/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout.</exception>
	/// <returns>The <see cref="IReadOnlyList{TElement}"/>.</returns>
	public static Task<IReadOnlyList<TElement>> WaitForElementsAsync<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, int matchElementCount, TimeSpan timeout)
		where TComponent : IComponent
		where TElement : class, IElement
		=> WaitForElementsCoreAsync<TComponent, TElement>(renderedComponent, cssSelector, matchElementCount: matchElementCount, timeout: timeout);

	private static IElement WaitForElementCore<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan? timeout)
		where TComponent : IComponent
		=> WaitForElementCore<TComponent, IElement>(renderedComponent, cssSelector, timeout);

	private static TElement WaitForElementCore<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan? timeout)
		where TComponent : IComponent
		where TElement : class, IElement
	{
		using var waiter = new WaitForElementHelper<TComponent, TElement>(renderedComponent, cssSelector, timeout);

		try
		{
			return waiter.WaitTask.ConfigureAwait(false).GetAwaiter().GetResult();
		}
		catch (AggregateException e) when (e.InnerExceptions.Count == 1)
		{
			ExceptionDispatchInfo.Capture(e.InnerExceptions[0]).Throw();

			// Unreachable code
			throw;
		}
	}

	private static Task<IElement> WaitForElementCoreAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan? timeout)
		where TComponent : IComponent
		=> WaitForElementCoreAsync<TComponent, IElement>(renderedComponent, cssSelector, timeout);

	private static async Task<TElement> WaitForElementCoreAsync<TComponent, TElement>(this IRenderedComponent<TComponent> renderedComponent, string cssSelector, TimeSpan? timeout)
		where TComponent : IComponent
		where TElement : class, IElement
	{
		using var waiter = new WaitForElementHelper<TComponent, TElement>(renderedComponent, cssSelector, timeout);

		return await waiter.WaitTask;
	}

	private static IReadOnlyList<IElement> WaitForElementsCore<TComponent>(
		this IRenderedComponent<TComponent> renderedComponent,
		string cssSelector,
		int? matchElementCount,
		TimeSpan? timeout)
		where TComponent : IComponent
		=> WaitForElementsCore<TComponent, IElement>(renderedComponent, cssSelector, matchElementCount, timeout);

	private static IReadOnlyList<TElement> WaitForElementsCore<TComponent, TElement>(
		this IRenderedComponent<TComponent> renderedComponent,
		string cssSelector,
		int? matchElementCount,
		TimeSpan? timeout)
		where TComponent : IComponent
		where TElement : class, IElement
	{
		using var waiter = new WaitForElementsHelper<TComponent, TElement>(renderedComponent, cssSelector, matchElementCount, timeout);

		try
		{
			return waiter.WaitTask.ConfigureAwait(false).GetAwaiter().GetResult();
		}
		catch (AggregateException e) when (e.InnerExceptions.Count == 1)
		{
			ExceptionDispatchInfo.Capture(e.InnerExceptions[0]).Throw();

			// Unreachable code
			throw;
		}
	}

	private static Task<IReadOnlyList<IElement>> WaitForElementsCoreAsync<TComponent>(
		this IRenderedComponent<TComponent> renderedComponent,
		string cssSelector,
		int? matchElementCount,
		TimeSpan? timeout)
		where TComponent : IComponent
		=> WaitForElementsCoreAsync<TComponent, IElement>(renderedComponent, cssSelector, matchElementCount, timeout);

	private static async Task<IReadOnlyList<TElement>> WaitForElementsCoreAsync<TComponent, TElement>(
		this IRenderedComponent<TComponent> renderedComponent,
		string cssSelector,
		int? matchElementCount,
		TimeSpan? timeout)
		where TComponent : IComponent
		where TElement : class, IElement
	{
		using var waiter = new WaitForElementsHelper<TComponent, TElement>(renderedComponent, cssSelector, matchElementCount, timeout);

		return await waiter.WaitTask;
	}
}
