using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bunit.TestDoubles.HttpClient
{
	/// <summary>
	/// This MessageHandler for HttpClient is used to provide users with helpful
	/// exceptions if they fail to provide a mock when required.
	/// </summary>
	public class PlaceholderHttpMessageHandler : HttpMessageHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="MissingMockHttpClientException"></exception>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			throw new MissingMockHttpClientException(request);
		}
	}

	/// <summary>
	/// Exception use to indicate that a mock HttpClient is required by a test
	/// but was not provided.
	/// </summary>
	public class MissingMockHttpClientException : Exception
	{
		/// <summary>
		/// The request that was sent via the http client
		/// </summary>
		public HttpRequestMessage Request { get; }

		/// <summary>
		///
		/// </summary>
		public MissingMockHttpClientException(HttpRequestMessage request)
			: base($"This test requires a HttpClient to be supplied, because the component under test invokes the HttpClient during the test. The request that was sent is contained within the '{nameof(Request)}' attribute of this exception. Guidance on mocking the HttpClient is available in the testing library's Wiki.")
		{
			Request = request;
		}
	}
}
