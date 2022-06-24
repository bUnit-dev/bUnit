using System.Globalization;
using Xunit.Abstractions;

namespace Bunit.Extensions.WaitForHelpers;

public class RenderedFragmentWaitForElementsHelperExtensionsTest : TestContext
{
	public RenderedFragmentWaitForElementsHelperExtensionsTest(ITestOutputHelper testOutput)
	{
		Services.AddXunitLogger(testOutput);
	}

	[Fact(DisplayName = "WaitForElement waits until cssSelector returns at a element")]
	public async Task Test001()
	{
		var expectedMarkup = "<p>child content</p>";
		var cut = await RenderComponent<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elm = await cut.WaitForElement("main > p");

		elm.MarkupMatches(expectedMarkup);
	}

	[Fact(DisplayName = "WaitForElement throws exception after timeout when cssSelector does not result in matching element")]
	public async Task Test002()
	{
		var cut = await RenderComponent<DelayRenderFragment>();

		var expected = await Should.ThrowAsync<WaitForFailedException>(() =>
			cut.WaitForElement("#notHereElm", TimeSpan.FromMilliseconds(10)));

		expected.Message.ShouldBe(WaitForElementHelper.TimeoutBeforeFoundMessage);
	}

	[Fact(DisplayName = "WaitForElements waits until cssSelector returns at least one element")]
	public async Task Test021()
	{
		var expectedMarkup = "<p>child content</p>";
		var cut = await RenderComponent<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elms = await cut.WaitForElements("main > p");

		elms.MarkupMatches(expectedMarkup);
	}

	[Fact(DisplayName = "WaitForElements throws exception after timeout when cssSelector does not result in matching elements")]
	public async Task Test022()
	{
		var cut = await RenderComponent<DelayRenderFragment>();

		var expected = Should.Throw<WaitForFailedException>(() =>
			cut.WaitForElements("#notHereElm", TimeSpan.FromMilliseconds(30)));

		expected.Message.ShouldBe(WaitForElementsHelper.TimeoutBeforeFoundMessage);
		expected.InnerException.ShouldBeNull();
	}

	[Fact(DisplayName = "WaitForElements with specific count N throws exception after timeout when cssSelector does not result in N matching elements")]
	public async Task Test023()
	{
		var cut = await RenderComponent<DelayRenderFragment>();

		var expected = await Should.ThrowAsync<WaitForFailedException>(() =>
			cut.WaitForElements("#notHereElm", 2, TimeSpan.FromMilliseconds(30)));

		expected.Message.ShouldBe(string.Format(CultureInfo.InvariantCulture, WaitForElementsHelper.TimeoutBeforeFoundWithCountMessage, 2));
		expected.InnerException.ShouldBeNull();
	}

	[Fact(DisplayName = "WaitForElements with specific count N waits until cssSelector returns at exact N elements")]
	public async Task Test024()
	{
		var expectedMarkup = "<p>child content</p><p>child content</p><p>child content</p>";
		var cut = await RenderComponent<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elms = await cut.WaitForElements("main > p", matchElementCount: 3);

		elms.MarkupMatches(expectedMarkup);
	}

	[Fact(DisplayName = "WaitForElements with specific count 0 waits until cssSelector returns at exact zero elements")]
	public async Task Test025()
	{
		var expectedMarkup = "<p>child content</p>";
		var cut = await RenderComponent<DelayRemovedRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elms = await cut.WaitForElements("main > p", matchElementCount: 0);

		elms.ShouldBeEmpty();
	}
}
