using System;

namespace Bunit.Extensions.WaitForHelpers
{
	/// <summary>
	/// Represents an async wait helper, that will wait for a specified time for a state predicate to pass.
	/// </summary>
	public class WaitForStateHelper : WaitForHelper
	{
		internal const string TimeoutBeforePassMessage = "The state predicate did not pass before the timeout period passed.";
		internal const string ExceptionInPredicateMessage = "The state predicate throw an unhandled exception.";

		/// <inheritdoc/>
		protected override string? TimeoutErrorMessage { get; } = TimeoutBeforePassMessage;

		/// <inheritdoc/>
		protected override string? CheckThrowErrorMessage { get; } = ExceptionInPredicateMessage;

		/// <inheritdoc/>
		protected override bool StopWaitingOnCheckException { get; } = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="WaitForStateHelper"/> class,
		/// which will wait until the provided <paramref name="statePredicate"/> action returns true,
		/// or the <paramref name="timeout"/> is reached (default is one second).
		/// </summary>
		/// <remarks>
		/// The <paramref name="statePredicate"/> is evaluated initially, and then each time the <paramref name="renderedFragment"/> renders.
		/// </remarks>
		/// <param name="renderedFragment">The render fragment or component to attempt to verify state against.</param>
		/// <param name="statePredicate">The predicate to invoke after each render, which must returns <c>true</c> when the desired state has been reached.</param>
		/// <param name="timeout">The maximum time to wait for the desired state.</param>
		/// <exception cref="WaitForFailedException">Thrown if the <paramref name="statePredicate"/> throw an exception during invocation, or if the timeout has been reached. See the inner exception for details.</exception>
		public WaitForStateHelper(IRenderedFragmentBase renderedFragment, Func<bool> statePredicate, TimeSpan? timeout = null)
			: base(renderedFragment, statePredicate, timeout)
		{
		}
	}
}
