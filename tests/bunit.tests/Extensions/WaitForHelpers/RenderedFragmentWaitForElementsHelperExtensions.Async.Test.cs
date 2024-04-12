using System.Globalization;

namespace Bunit.Extensions.WaitForHelpers;

public class RenderedFragmentWaitForElementsHelperExtensionsAsyncTest : TestContext
{
	private readonly static TimeSpan WaitForTestTimeout = TimeSpan.FromMilliseconds(5);

	public RenderedFragmentWaitForElementsHelperExtensionsAsyncTest(ITestOutputHelper testOutput)
	{
		TestContext.DefaultWaitTimeout = TimeSpan.FromSeconds(30);
		Services.AddXunitLogger(testOutput);
	}

	[Fact(DisplayName = "WaitForElement waits until cssSelector returns at a element")]
	[Trait("Category", "async")]
	public async Task Test001()
	{
		var expectedMarkup = "<p>child content</p>";
		var cut = Render<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elm = await cut.WaitForElementAsync("main > p");

		elm.MarkupMatches(expectedMarkup);
	}

	[Fact(DisplayName = "WaitForElement throws exception after timeout when cssSelector does not result in matching element")]
	[Trait("Category", "async")]
	public async Task Test002()
	{
		var cut = Render<DelayRenderFragment>();

		var expected = await Should.ThrowAsync<WaitForFailedException>(async () =>
			await cut.WaitForElementAsync("#notHereElm", WaitForTestTimeout));

		expected.Message.ShouldStartWith(WaitForElementHelper.TimeoutBeforeFoundMessage);
	}

	[Fact(DisplayName = "WaitForElements waits until cssSelector returns at least one element")]
	[Trait("Category", "async")]
	public async Task Test021()
	{
		var expectedMarkup = "<p>child content</p>";
		var cut = Render<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elms = await cut.WaitForElementsAsync("main > p");

		elms.MarkupMatches(expectedMarkup);
	}

	[Fact(DisplayName = "WaitForElements throws exception after timeout when cssSelector does not result in matching elements")]
	[Trait("Category", "async")]
	public async Task Test022()
	{
		var cut = Render<DelayRenderFragment>();

		var expected = await Should.ThrowAsync<WaitForFailedException>(async () =>
			await cut.WaitForElementsAsync("#notHereElm", WaitForTestTimeout));

		expected.Message.ShouldStartWith(WaitForElementsHelper.TimeoutBeforeFoundMessage);
		expected.InnerException.ShouldBeNull();
	}

	[Fact(DisplayName = "WaitForElements with specific count N throws exception after timeout when cssSelector does not result in N matching elements",
		Skip = "Need to figure out how to make this deterministic.")]
	[Trait("Category", "async")]
	public async Task Test023()
	{
		var cut = Render<DelayRenderFragment>();

		var expected = await Should.ThrowAsync<WaitForFailedException>(async () =>
			await cut.WaitForElementsAsync("#notHereElm", 2, WaitForTestTimeout));

		expected.Message.ShouldStartWith(string.Format(CultureInfo.InvariantCulture, WaitForElementsHelper.TimeoutBeforeFoundWithCountMessage, 2));
		expected.InnerException.ShouldBeNull();
	}

	[Fact(DisplayName = "WaitForElements with specific count N waits until cssSelector returns at exact N elements")]
	[Trait("Category", "async")]
	public async Task Test024()
	{
		var expectedMarkup = "<p>child content</p><p>child content</p><p>child content</p>";
		var cut = Render<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elms = await cut.WaitForElementsAsync("main > p", matchElementCount: 3);

		elms.MarkupMatches(expectedMarkup);
	}

	[Fact(DisplayName = "WaitForElements with specific count 0 waits until cssSelector returns at exact zero elements")]
	[Trait("Category", "async")]
	public async Task Test025()
	{
		var expectedMarkup = "<p>child content</p>";
		var cut = Render<DelayRemovedRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elms = await cut.WaitForElementsAsync("main > p", matchElementCount: 0);

		elms.ShouldBeEmpty();
	}
}
