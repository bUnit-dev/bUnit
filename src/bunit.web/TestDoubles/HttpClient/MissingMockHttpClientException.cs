using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Exception use to indicate that a mock HttpClient is required by a test
	/// but was not provided.
	/// </summary>
	[Serializable]
	public sealed class MissingMockHttpClientException : Exception
	{
		[NonSerialized]
		private readonly HttpRequestMessage? _request;

		/// <summary>
		/// The request that was sent via the http client
		/// </summary>
		public HttpRequestMessage? Request => _request;
		/// <summary>
		/// Creates a new instance of the <see cref="MissingMockHttpClientException"/>
		/// with the request that would have been handled
		/// </summary>
		/// <param name="request">The request being handled by the client</param>
		public MissingMockHttpClientException(HttpRequestMessage request)
			: base($"This test requires a HttpClient to be supplied, because the component under test invokes the HttpClient during the test. The request that was sent is contained within the '{nameof(Request)}' attribute of this exception. Guidance on mocking the HttpClient is available on bUnit's website.")
		{
			_request = request;
			HelpLink = "https://bunit.egilhansen.com/docs/test-doubles/mocking-httpclient";
		}

		private MissingMockHttpClientException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext) { }
	}
}
