using Bunit.TestDoubles;

namespace Bunit;

public partial class TestContextTest : TestContext
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
	public async Task Test030()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = await Render(b =>
		{
			b.OpenComponent<ReceivesCascadinValue>(0);
			b.CloseComponent();
		});

		cut.FindComponent<ReceivesCascadinValue>()
			.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "Render<TComponent>() renders fragment inside RenderTreee")]
	public async Task Test031()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = await Render<ReceivesCascadinValue>(b =>
		{
			b.OpenComponent<ReceivesCascadinValue>(0);
			b.CloseComponent();
		});

		cut.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "RenderComponent<TComponent>(builder) renders TComponent inside RenderTreee")]
	public async Task Test032()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = await RenderComponent<ReceivesCascadinValue>(ps => ps.Add(p => p.Dummy, null));

		cut.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "RenderComponent<TComponent>(factories) renders TComponent inside RenderTreee")]
	public async Task Test033()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = await RenderComponent<ReceivesCascadinValue>(("Dummy", null));

		cut.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "Can raise events from markup rendered with TestContext")]
	public void Test040()
	{
		Should.NotThrowAsync(async () =>
		{
			var component = await RenderComponent<ClickCounter>();
			component.Find("button").Click();
		});
	}

	private class ReceivesCascadinValue : ComponentBase
	{
		[CascadingParameter] public string? Value { get; set; }

		[Parameter] public object? Dummy { get; set; }
	}
}
