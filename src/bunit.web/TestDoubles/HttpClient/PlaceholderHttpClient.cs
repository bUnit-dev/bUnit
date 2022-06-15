namespace Bunit.TestDoubles;

/// <summary>
/// This PlaceholderHttpClient is used to provide users with helpful exceptions if they fail to provide a mock when required.
/// </summary>
internal sealed class PlaceholderHttpClient : HttpClient
{
	private const string PlaceholderBaseAddress = "http://localhost";

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
