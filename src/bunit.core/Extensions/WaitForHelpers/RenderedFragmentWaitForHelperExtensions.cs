using System;
using System.Diagnostics.CodeAnalysis;
using Bunit.Extensions.WaitForHelpers;

namespace Bunit
{
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
		public static void WaitForState(this IRenderedFragmentBase renderedFragment, Func<bool> statePredicate, TimeSpan? timeout = null)
		{
			using var waiter = new WaitForStateHelper(renderedFragment, statePredicate, timeout);
			try
			{
				waiter.WaitTask.GetAwaiter().GetResult();
			}
			catch (AggregateException e) when (e.InnerException is not null)
			{
				throw e.InnerException;
			}
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
		public static void WaitForAssertion(this IRenderedFragmentBase renderedFragment, Action assertion, TimeSpan? timeout = null)
		{
			using var waiter = new WaitForAssertionHelper(renderedFragment, assertion, timeout);
			try
			{
				waiter.WaitTask.GetAwaiter().GetResult();
			}
			catch (AggregateException e) when (e.InnerException is not null)
			{
				throw e.InnerException;
			}
		}
	}
}
