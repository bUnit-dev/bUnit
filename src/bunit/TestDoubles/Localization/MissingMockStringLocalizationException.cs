namespace Bunit.TestDoubles;

/// <summary>
/// Exception use to indicate that a IStringLocalizer is required by a test
/// but was not provided.
/// </summary>
public sealed class MissingMockStringLocalizationException : Exception
{
	/// <summary>
	/// Gets the arguments that were passed into the localizer.
	/// </summary>
	public IReadOnlyList<object?> Arguments { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MissingMockStringLocalizationException"/> class
	/// with the method name and arguments used in the invocation.
	/// </summary>
	/// <param name="methodName">The method that was called on the localizer.</param>
	/// <param name="arguments">The arguments that were passed in.</param>
	public MissingMockStringLocalizationException(string methodName, params object?[] arguments)
		: base($"This test requires a IStringLocalizer to be supplied, because the component under test invokes the IStringLocalizer during the test. The method that was called was '{methodName}', the parameters are container within the '{nameof(Arguments)}' property of this exception.")
	{
		Arguments = arguments;
	}
}
