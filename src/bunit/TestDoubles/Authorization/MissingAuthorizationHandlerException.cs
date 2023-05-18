namespace Bunit.TestDoubles;

/// <summary>
/// Exception used to indicate that the an authorization services are required by a test
/// but are not provided in the container.
/// </summary>
public sealed class MissingAuthorizationHandlerException : Exception
{
	/// <summary>
	/// Gets the invoking service name.
	/// </summary>
	public string ServiceName { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MissingAuthorizationHandlerException"/> class
	/// with the arguments used in the invocation.
	/// </summary>
	/// <param name="serviceName">The service being used.</param>
	public MissingAuthorizationHandlerException(string serviceName)
		: base($"This test requires {serviceName} to be supplied, because the component under test uses authentication/authorization during the test. You can fix this by calling TestContext.Services.AddAuthorization with appropriate values. More information can be found in the documentation.")
	{
		ServiceName = serviceName;
		HelpLink = "https://bunit.egilhansen.com/docs/test-doubles/faking-auth";
	}
}
