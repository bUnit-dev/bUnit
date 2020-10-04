using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bunit.TestDoubles.HttpClient
{
	/// <summary>
	/// This PlaceholderHttpClient is used to provide users with helpful exceptions if they fail to provide a mock when required.
	/// </summary>
	internal class PlaceholderHttpClient : System.Net.Http.HttpClient
	{
		/// <summary>
		/// Creates an instance of <see cref="PlaceholderHttpClient"/>
		/// with a <see cref="PlaceholderHttpMessageHandler"/> message handler
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
		public PlaceholderHttpClient()
			: base(new PlaceholderHttpMessageHandler())
		{
			BaseAddress = new Uri("http://localhost");
		}

		/// <summary>
		/// This MessageHandler for HttpClient is used to provide users with helpful
		/// exceptions if they fail to provide a mock when required.
		/// </summary>
		private class PlaceholderHttpMessageHandler : HttpMessageHandler
		{
			/// <summary>
			///
			/// </summary>
			/// <param name="request"></param>
			/// <param name="cancellationToken"></param>
			/// <returns></returns>
			/// <exception cref="MissingMockHttpClientException"></exception>
			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
				=> throw new MissingMockHttpClientException(request);
		}
	}
}
