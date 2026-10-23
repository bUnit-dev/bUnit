using Bunit.Asserting.XUnitExceptions;

namespace Bunit.Extensions.WaitForHelpers;

/// <summary>
/// Represents an exception thrown when the <see cref="WaitForHelper{T, TComponent}"/> does not complete successfully.
/// </summary>
public sealed class WaitForFailedException : Exception, ITestTimeoutException
{
	internal const string CheckNotStartedMessage = """
		The wait condition was never checked, because the renderer's dispatcher did not run the check before the timeout.
		This points to the test setup rather than the component under test: the thread pool may be starved,
		e.g. by many tests blocking threads with synchronous WaitFor* or Click calls in parallel, or other work is blocking the dispatcher.

		Consider one of the following:
		- Increase the timeout per call, e.g. WaitForElement("div", TimeSpan.FromSeconds(15)),
		  or globally via BunitContext.DefaultWaitTimeout.
		- Prefer the async WaitFor*Async overloads, which do not block a thread pool thread while waiting.
		- Raise the thread pool minimum once for the test assembly, e.g.:

		    internal static class ThreadPoolSetup
		    {
		        [System.Runtime.CompilerServices.ModuleInitializer]
		        internal static void RaiseMinThreads()
		        {
		            ThreadPool.GetMinThreads(out var workers, out var io);
		            ThreadPool.SetMinThreads(Math.Max(workers, Environment.ProcessorCount * 4), io);
		        }
		    }
		""";

	internal const string CheckNotCompletedMessage =
		"The wait condition check started but did not complete before the timeout. " +
		"The asynchronous check is most likely awaiting something that never completes, or that takes longer than the timeout. " +
		"This usually indicates a problem in the test itself.";

	/// <summary>
	/// Initializes a new instance of the <see cref="WaitForFailedException"/> class.
	/// </summary>
	public WaitForFailedException(string? errorMessage, Exception? innerException = null)
		: base(errorMessage ?? string.Empty, innerException)
	{
	}

	internal WaitForFailedException(string errorMessage, int checkCount, int componentRenderCount, int totalRenderCount, Exception? innerException = null)
		: base(CreateMessage(errorMessage, checkCount, componentRenderCount, totalRenderCount), innerException)
	{
	}

	private static string CreateMessage(
		string errorMessage,
		int checkCount,
		int componentRenderCount,
		int totalRenderCount)
	{
		return $"""
			{errorMessage}

			If this test does not fail consistently, the reason may be that
			the wait timeout is too short, and the runtime did not have enough
			time to complete the necessary number of renders of the component under test.
			This can happen on highly utilized or slower hardware, for example.

			To determine if this is the cause, compare the check and render count(s) below
			and see if they match what is expected. If they do not,
			consider increasing the timeout, either at the individual
			method call level, e.g. WaitForElement("div", TimeSpan.FromSeconds(15)),
			or via the static TestContext.DefaultWaitTimeout property.

			Check count: {checkCount}.
			Component render count: {componentRenderCount}.
			Total render count across all components: {totalRenderCount}.
			""";
	}
}
