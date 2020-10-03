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
}
