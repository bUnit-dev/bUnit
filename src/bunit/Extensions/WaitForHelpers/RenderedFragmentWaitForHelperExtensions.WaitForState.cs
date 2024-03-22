using System.Runtime.ExceptionServices;
using Bunit.Asserting;
using Bunit.Extensions.WaitForHelpers;

namespace Bunit;

/// <summary>
/// Helper methods dealing with async rendering during testing.
/// </summary>
public static partial class RenderedComponentWaitForHelperExtensions
{
		/// <summary>
		/// Wait until the provided <paramref name="statePredicate"/> action returns true,
		/// or the <paramref name="timeout"/> is reached (default is one second).
		///
		/// The <paramref name="statePredicate"/> is evaluated initially, and then each time
		/// the <paramref name="renderedComponent"/> renders.
		/// </summary>
		/// <param name="renderedComponent">The render fragment or component to attempt to verify state against.</param>
		/// <param name="statePredicate">The predicate to invoke after each render, which must returns <c>true</c> when the desired state has been reached.</param>
		/// <param name="timeout">The maximum time to wait for the desired state.</param>
		/// <exception cref="WaitForFailedException">Thrown if the <paramref name="statePredicate"/> throw an exception during invocation, or if the timeout has been reached. See the inner exception for details.</exception>
		/// <remarks>
		/// If a debugger is attached the timeout is set to <see cref="Timeout.InfiniteTimeSpan" />, giving the possibility to debug without the timeout triggering.
		/// </remarks>
		public static void WaitForState<TComponent>(this IRenderedComponent<TComponent> renderedComponent, Func<bool> statePredicate, TimeSpan? timeout = null)
			where TComponent : IComponent
		{
			using var waiter = new WaitForStateHelper<TComponent>(renderedComponent, statePredicate, timeout);

			try
			{
				waiter.WaitTask.GetAwaiter().GetResult();
			}
			catch (AggregateException e) when (e.InnerExceptions.Count == 1)
			{
				ExceptionDispatchInfo.Capture(e.InnerExceptions[0]).Throw();
			}
		}

		/// <summary>
		/// Wait until the provided <paramref name="statePredicate"/> action returns true,
		/// or the <paramref name="timeout"/> is reached (default is one second).
		///
		/// The <paramref name="statePredicate"/> is evaluated initially, and then each time
		/// the <paramref name="renderedComponent"/> renders.
		/// </summary>
		/// <param name="renderedComponent">The render fragment or component to attempt to verify state against.</param>
		/// <param name="statePredicate">The predicate to invoke after each render, which must returns <c>true</c> when the desired state has been reached.</param>
		/// <param name="timeout">The maximum time to wait for the desired state.</param>
		/// <exception cref="WaitForFailedException">Thrown if the <paramref name="statePredicate"/> throw an exception during invocation, or if the timeout has been reached. See the inner exception for details.</exception>
		internal static async Task WaitForStateAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, Func<bool> statePredicate, TimeSpan? timeout = null)
			where TComponent : IComponent
		{
			using var waiter = new WaitForStateHelper<TComponent>(renderedComponent, statePredicate, timeout);

			await waiter.WaitTask;
		}

		/// <summary>
		/// Wait until the provided <paramref name="assertion"/> passes (i.e. does not throw an
		/// exception), or the <paramref name="timeout"/> is reached (default is one second).
		///
		/// The <paramref name="assertion"/> is attempted initially, and then each time the <paramref name="renderedComponent"/> renders.
		/// </summary>
		/// <param name="renderedComponent">The rendered fragment to wait for renders from and assert against.</param>
		/// <param name="assertion">The verification or assertion to perform.</param>
		/// <param name="timeout">The maximum time to attempt the verification.</param>
		/// <exception cref="WaitForFailedException">Thrown if the timeout has been reached. See the inner exception to see the captured assertion exception.</exception>
		[AssertionMethod]
		public static void WaitForAssertion<TComponent>(this IRenderedComponent<TComponent> renderedComponent, Action assertion, TimeSpan? timeout = null)
			where TComponent : IComponent
		{
			using var waiter = new WaitForAssertionHelper<TComponent>(renderedComponent, assertion, timeout);

			try
			{
				waiter.WaitTask.GetAwaiter().GetResult();
			}
			catch (AggregateException e) when (e.InnerExceptions.Count == 1)
			{
				ExceptionDispatchInfo.Capture(e.InnerExceptions[0]).Throw();
			}
		}

		/// <summary>
		/// Wait until the provided <paramref name="assertion"/> passes (i.e. does not throw an
		/// exception), or the <paramref name="timeout"/> is reached (default is one second).
		///
		/// The <paramref name="assertion"/> is attempted initially, and then each time the <paramref name="renderedComponent"/> renders.
		/// </summary>
		/// <param name="renderedComponent">The rendered fragment to wait for renders from and assert against.</param>
		/// <param name="assertion">The verification or assertion to perform.</param>
		/// <param name="timeout">The maximum time to attempt the verification.</param>
		/// <exception cref="WaitForFailedException">Thrown if the timeout has been reached. See the inner exception to see the captured assertion exception.</exception>
		[AssertionMethod]
		internal static async Task WaitForAssertionAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, Action assertion, TimeSpan? timeout = null)
			where TComponent : IComponent
		{
			using var waiter = new WaitForAssertionHelper<TComponent>(renderedComponent, assertion, timeout);

			await waiter.WaitTask;
		}
}
