namespace Bunit.Extensions.WaitForHelpers;

/// <summary>
/// Represents an async wait helper, that will wait for a specified time for an assertion to pass.
/// </summary>
public class WaitForAssertionHelper : WaitForHelper<object?>
{
	internal const string TimeoutMessage = "The assertion did not pass within the timeout period.";

	/// <inheritdoc/>
	protected override string? TimeoutErrorMessage => TimeoutMessage;

	/// <inheritdoc/>
	protected override bool StopWaitingOnCheckException { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="WaitForAssertionHelper"/> class,
	/// which will until the provided <paramref name="assertion"/> passes (i.e. does not throw an
	/// exception), or the <paramref name="timeout"/> is reached (default is one second).
	///
	/// The <paramref name="assertion"/> is attempted initially, and then each time the <paramref name="renderedFragment"/> renders.
	/// </summary>
	/// <param name="renderedFragment">The rendered fragment to wait for renders from and assert against.</param>
	/// <param name="assertion">The verification or assertion to perform.</param>
	/// <param name="timeout">The maximum time to attempt the verification.</param>
	/// <remarks>
	/// If a debugger is attached the timeout is set to <see cref="Timeout.InfiniteTimeSpan" />, giving the possibility to debug without the timeout triggering.
	/// </remarks>
	public WaitForAssertionHelper(RenderedFragment renderedFragment, Action assertion, TimeSpan? timeout = null)
		: base(
			  renderedFragment,
			  () =>
			  {
				  assertion();
				  return (true, default);
			  },
			  timeout)
	{ }
}
