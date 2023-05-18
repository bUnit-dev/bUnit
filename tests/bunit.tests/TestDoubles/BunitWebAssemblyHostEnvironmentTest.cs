namespace Bunit.TestDoubles;

public class BunitWebAssemblyHostEnvironmentTest : TestContext
{
	[Fact]
	public void ShouldSayHelloToDevelopers()
	{
		var hostEnvironment = Services.GetRequiredService<BunitWebAssemblyHostEnvironment>();
		hostEnvironment.SetEnvironmentToDevelopment();

		var cut = RenderComponent<SimpleUsingWebAssemblyHostEnvironment>();

		// Assert - inspects markup to verify the message
		cut.Find("p").MarkupMatches($"<p>Hello Developers. The base URL is: /</p>");
	}

	[Fact]
	public void ShouldUseCorrectBaseAddress()
	{
		var hostEnvironment = Services.GetRequiredService<BunitWebAssemblyHostEnvironment>();
		hostEnvironment.BaseAddress = "myBaseUrl/";
		var cut = RenderComponent<SimpleUsingWebAssemblyHostEnvironment>();

		// Assert - inspect markup to verify that the BaseAddress is used correctly.
		cut.Find("p").MarkupMatches($"<p>Hello World. The base URL is: myBaseUrl/</p>");
	}
}
