namespace Bunit.TestDoubles;

/// <summary>
/// Exception used to indicate that the bunit authorization services are required by a test
/// but provided in BunitContext.Services.
/// </summary>
[Serializable]
public sealed class MissingBunitAuthorizationException : Exception
{
	/// <summary>
	/// Gets the invoking service name.
	/// </summary>
	public string ServiceName { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MissingBunitAuthorizationException"/> class
	/// with the arguments used in the invocation.
	/// </summary>
	/// <param name="serviceName">The service being used.</param>
	public MissingBunitAuthorizationException(string serviceName)
		: base($"This test requires {serviceName} to be supplied, because the component under test uses authentication/authorization during the test. You can fix this by calling BunitContext.Services.AddAuthorization with appropriate values. More information can be found in the documentation.")
	{
		ServiceName = serviceName;
		HelpLink = "https://bunit.egilhansen.com/docs/test-doubles/bunit-auth";
	}

	private MissingBunitAuthorizationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
		ServiceName = serializationInfo?.GetString(nameof(ServiceName)) ?? string.Empty;
	}
}
