using System;
using System.Runtime.ExceptionServices;
using AngleSharp.Dom;
using Bunit.Extensions.WaitForHelpers;

namespace Bunit
{
	/// <summary>
	/// Helper methods dealing with async rendering during testing.
	/// </summary>
	public static class RenderedFragmentWaitForHelperExtensions
	{
		/// <summary>
		/// Wait until an element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedFragment"/>,
		/// or the timeout is reached (default is one second).
		/// </summary>
		/// <param name="renderedFragment">The render fragment or component find the matching element in.</param>
		/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
		/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
		/// <returns>The <see cref="IElement"/>.</returns>
		public static IElement WaitForElement(this IRenderedFragment renderedFragment, string cssSelector)
			=> WaitForElementCore(renderedFragment, cssSelector, timeout: null);

		/// <summary>
		/// Wait until an element matching the <paramref name="cssSelector"/> exists in the <paramref name="renderedFragment"/>,
		/// or the <paramref name="timeout"/> is reached.
		/// </summary>
		/// <param name="renderedFragment">The render fragment or component find the matching element in.</param>
		/// <param name="cssSelector">The CSS selector to use to search for the element.</param>
		/// <param name="timeout">The maximum time to wait for the element to appear.</param>
		/// <exception cref="WaitForFailedException">Thrown if no elements is found matching the <paramref name="cssSelector"/> within the default timeout. See the inner exception for details.</exception>
		/// <returns>The <see cref="IElement"/>.</returns>
		public static IElement WaitForElement(this IRenderedFragment renderedFragment, string cssSelector, TimeSpan timeout)
			=> WaitForElementCore(renderedFragment, cssSelector, timeout: timeout);

		private static IElement WaitForElementCore(this IRenderedFragment renderedFragment, string cssSelector, TimeSpan? timeout)
		{
			using var waiter = new WaitForElementHelper(renderedFragment, cssSelector, timeout);

			try
			{
				return waiter.WaitTask.GetAwaiter().GetResult();
			}
			catch (Exception e)
			{
				if (e is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1)
				{
					ExceptionDispatchInfo.Capture(aggregateException.InnerExceptions[0]).Throw();
				}
				else
				{
					ExceptionDispatchInfo.Capture(e).Throw();
				}

				// Unreachable code. Only here because compiler does not know that ExceptionDispatchInfo throws an exception
				throw;
			}
		}
	}
}
