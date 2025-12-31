namespace Bunit.TestIds;

/// <summary>
/// Represents a failure to find an element in the searched target
/// using the specified test id.
/// </summary>
public sealed class TestIdNotFoundException : Exception
{
	/// <summary>
	/// Gets the test id used to search with.
	/// </summary>
	public string? TestId { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="TestIdNotFoundException"/> class.
	/// </summary>
	/// <param name="testId">The test id that was searched for.</param>
	public TestIdNotFoundException(string? testId = null)
		: base($"Unable to find an element with the Test ID '{testId}'.")
	{
		TestId = testId;
	}
}
