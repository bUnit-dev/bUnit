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
	internal class PlaceholderHttpClient : System.Net.Http.HttpClient
	{
		[SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
		public PlaceholderHttpClient() : base(new PlaceholderHttpMessageHandler())
		{
			BaseAddress = new Uri("http://localhost");
		}

		private class PlaceholderHttpMessageHandler : HttpMessageHandler
		{
			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
				=> throw new MissingMockHttpClientException(request);
		}
	}
}
