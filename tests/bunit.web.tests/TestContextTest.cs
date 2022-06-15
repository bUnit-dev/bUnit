using Bunit.TestDoubles;

namespace Bunit;

public class TestContextTest : TestContext
{
	[Fact(DisplayName = "The test service provider should register a placeholder HttpClient which throws exceptions")]
	public void Test024()
	{
		Should.Throw<MissingMockHttpClientException>(() => RenderComponent<SimpleWithHttpClient>());
	}

	[Fact(DisplayName = "The test service provider should register a placeholder IStringLocalizer which throws exceptions")]
	public void Test026()
	{
		Should.Throw<MissingMockStringLocalizationException>(() => RenderComponent<SimpleUsingLocalizer>());
	}

	[Fact(DisplayName = "Render() renders fragment inside RenderTree")]
	public void Test030()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = Render(b =>
		{
			b.OpenComponent<ReceivesCascadingValue>(0);
			b.CloseComponent();
		});

		cut.FindComponent<ReceivesCascadingValue>()
			.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "Render<TComponent>() renders fragment inside RenderTreee")]
	public void Test031()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = Render<ReceivesCascadingValue>(b =>
		{
			b.OpenComponent<ReceivesCascadingValue>(0);
			b.CloseComponent();
		});

		cut.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "RenderComponent<TComponent>(builder) renders TComponent inside RenderTreee")]
	public void Test032()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = RenderComponent<ReceivesCascadingValue>(ps => ps.Add(p => p.Dummy, null));

		cut.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "RenderComponent<TComponent>(factories) renders TComponent inside RenderTreee")]
	public void Test033()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = RenderComponent<ReceivesCascadingValue>(("Dummy", null));

		cut.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "Can raise events from markup rendered with TestContext")]
	public void Test040()
	{
		Should.NotThrow(() => RenderComponent<ClickCounter>().Find("button").Click());
	}

	private class ReceivesCascadingValue : ComponentBase
	{
		[CascadingParameter] public string? Value { get; set; }

		[Parameter] public object? Dummy { get; set; }
	}
}
