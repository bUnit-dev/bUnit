using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// This PlaceholderHttpClient is used to provide users with helpful exceptions if they fail to provide a mock when required.
	/// </summary>
	internal class PlaceholderHttpClient : HttpClient
	{
		private const string PlaceholderBaseAddress = "http://localhost";

		public PlaceholderHttpClient() : base(new PlaceholderHttpMessageHandler())
		{
			BaseAddress = new Uri(PlaceholderBaseAddress);
		}

		private class PlaceholderHttpMessageHandler : HttpMessageHandler
		{
			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
				=> throw new MissingMockHttpClientException(request);
		}
	}
}
