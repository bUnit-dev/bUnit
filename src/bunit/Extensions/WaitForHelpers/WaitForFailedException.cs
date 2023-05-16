namespace Bunit.Extensions.WaitForHelpers;

/// <summary>
/// Represents an exception thrown when the <see cref="WaitForHelper{T}"/> does not complete successfully.
/// </summary>
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
		: base(errorMessage + $" Check count: {checkCount}. Component render count: {componentRenderCount}. Total render count: {totalRenderCount}.", innerException)
	{
	}
}
