namespace Bunit.TestDoubles;

/// <summary>
/// This PlaceholderHttpClient is used to provide users with helpful exceptions if they fail to provide a mock when required.
/// </summary>
internal sealed class PlaceholderHttpClient : HttpClient
{
	private const string PlaceholderBaseAddress = "http://localhost";

	public PlaceholderHttpClient()
#pragma warning disable CA2000 // Will be disposed by HttpClient
		: base(new PlaceholderHttpMessageHandler(), disposeHandler: true)
#pragma warning restore CA2000
	{
		BaseAddress = new Uri(PlaceholderBaseAddress);
	}

	private sealed class PlaceholderHttpMessageHandler : HttpMessageHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			=> throw new MissingMockHttpClientException(request);
	}
}
