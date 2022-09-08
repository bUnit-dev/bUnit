using Bunit.TestDoubles.WebAssemblyHostEnvironment;

namespace Bunit.TestDoubles;

public class FakeWebAssemblyHostEnvironmentTest : TestContext
{
	[Fact]
	public void ShouldSayHelloToDevelopers()
	{
		using var ctx = new TestContext();
		var hostEnvironment = ctx.Services.GetRequiredService<FakeWebAssemblyHostEnvironment>();
		hostEnvironment.SetEnvironmentToDevelopment();

		var cut = ctx.RenderComponent<SimpleUsingWebAssemblyHostEnvironment>();

		// Assert - inspects markup to verify the message
		cut.Find("p").MarkupMatches($"<p>Hello Developers. The base URL is: /</p>");
	}

	[Fact]
	public void ShouldUseCorrectBaseAddress()
	{
		using var ctx = new TestContext();
		var hostEnvironment = ctx.Services.GetRequiredService<FakeWebAssemblyHostEnvironment>();
		hostEnvironment.BaseAddress = "myBaseUrl/";
		var cut = ctx.RenderComponent<SimpleUsingWebAssemblyHostEnvironment>();

		// Assert - inspect markup to verify that the BaseAddress is used correctly.
		cut.Find("p").MarkupMatches($"<p>Hello World. The base URL is: myBaseUrl/</p>");
	}
}
