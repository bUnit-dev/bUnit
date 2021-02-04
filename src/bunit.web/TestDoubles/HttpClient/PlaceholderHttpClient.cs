using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// This PlaceholderHttpClient is used to provide users with helpful exceptions if they fail to provide a mock when required.
	/// </summary>
	internal sealed class PlaceholderHttpClient : HttpClient
	{
		private const string PlaceholderBaseAddress = "http://localhost";

		[SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Base class will dispose of handler.")]
		public PlaceholderHttpClient()
			: base(new PlaceholderHttpMessageHandler(), disposeHandler: true)
		{
			BaseAddress = new Uri(PlaceholderBaseAddress);
		}

		private sealed class PlaceholderHttpMessageHandler : HttpMessageHandler
		{
			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
				=> throw new MissingMockHttpClientException(request);
		}
	}
}
