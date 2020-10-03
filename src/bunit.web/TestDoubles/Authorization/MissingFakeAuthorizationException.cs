using System;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// Exception used to indicate that the fake authorization services are required by a test
	/// but provided in TestContext.Services.
	/// </summary>
	public sealed class MissingFakeAuthorizationException : Exception
	{
		/// <summary>
		/// Creates a new instance of the <see cref="MissingFakeAuthorizationException"/>
		/// with the arguments used in the invocation.
		/// </summary>
		/// <param name="serviceName">The service being used.</param>
		public MissingFakeAuthorizationException(string serviceName)
			: base($"This test requires {serviceName} to be supplied, because the component under test uses authentication/authorization during the test. You can fix this by calling TestContext.Services.AddAuthorization with appropriate values. More information can be found in the documentation.")
		{
			ServiceName = serviceName;
			HelpLink = "https://bunit.egilhansen.com/docs/test-doubles/faking-auth";
		}

		/// <summary>
		/// Gets the invoking service name.
		/// </summary>
		public string ServiceName { get; }
	}
}
