namespace Bunit.Extensions.WaitForHelpers;

/// <summary>
/// Represents an exception thrown when the <see cref="WaitForHelper{T}"/> does not complete successfully.
/// </summary>
[Serializable]
public sealed class WaitForFailedException : Exception
{
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

	private WaitForFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context) { }
}
