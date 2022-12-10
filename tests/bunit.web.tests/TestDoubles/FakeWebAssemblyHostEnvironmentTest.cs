namespace Bunit.TestDoubles;

public class FakeWebAssemblyHostEnvironmentTest : TestContext
{
	[UIFact]
	public void ShouldSayHelloToDevelopers()
	{
		var hostEnvironment = Services.GetRequiredService<FakeWebAssemblyHostEnvironment>();
		hostEnvironment.SetEnvironmentToDevelopment();

		var cut = RenderComponent<SimpleUsingWebAssemblyHostEnvironment>();

		// Assert - inspects markup to verify the message
		cut.Find("p").MarkupMatches($"<p>Hello Developers. The base URL is: /</p>");
	}

	[UIFact]
	public void ShouldUseCorrectBaseAddress()
	{
		var hostEnvironment = Services.GetRequiredService<FakeWebAssemblyHostEnvironment>();
		hostEnvironment.BaseAddress = "myBaseUrl/";
		var cut = RenderComponent<SimpleUsingWebAssemblyHostEnvironment>();

		// Assert - inspect markup to verify that the BaseAddress is used correctly.
		cut.Find("p").MarkupMatches($"<p>Hello World. The base URL is: myBaseUrl/</p>");
	}
}
