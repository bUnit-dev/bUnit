using System.Globalization;

namespace Bunit.Extensions.WaitForHelpers;

public class RenderedComponentWaitForElementsHelperExtensionsBunit : BunitContext
{
	private readonly static TimeSpan WaitForTestTimeout = TimeSpan.FromMilliseconds(5);

	public RenderedComponentWaitForElementsHelperExtensionsBunit(ITestOutputHelper testOutput)
	{
		BunitContext.DefaultWaitTimeout = TimeSpan.FromSeconds(30);
		Services.AddXunitLogger(testOutput);
	}

	[Fact(DisplayName = "WaitForElement waits until cssSelector returns at a element")]
	[Trait("Category", "sync")]
	public void Test001()
	{
		var expectedMarkup = "<p>child content</p>";
		var cut = Render<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elm = cut.WaitForElement("main > p");

		elm.MarkupMatches(expectedMarkup);
	}

	[Fact(DisplayName = "WaitForElement throws exception after timeout when cssSelector does not result in matching element")]
	[Trait("Category", "sync")]
	public void Test002()
	{
		var cut = Render<DelayRenderFragment>();

		var expected = Should.Throw<WaitForFailedException>(() =>
			cut.WaitForElement("#notHereElm", WaitForTestTimeout));

		expected.Message.ShouldStartWith(WaitForElementHelper<DelayRenderFragment>.TimeoutBeforeFoundMessage);
	}

	[Fact(DisplayName = "WaitForElements waits until cssSelector returns at least one element")]
	[Trait("Category", "sync")]
	public void Test021()
	{
		var expectedMarkup = "<p>child content</p>";
		var cut = Render<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elms = cut.WaitForElements("main > p");

		elms.MarkupMatches(expectedMarkup);
	}

	[Fact(DisplayName = "WaitForElements throws exception after timeout when cssSelector does not result in matching elements")]
	[Trait("Category", "sync")]
	public void Test022()
	{
		var cut = Render<DelayRenderFragment>();

		var expected = Should.Throw<WaitForFailedException>(() =>
			cut.WaitForElements("#notHereElm", WaitForTestTimeout));

		expected.Message.ShouldStartWith(WaitForElementsHelper<DelayRenderFragment>.TimeoutBeforeFoundMessage);
		expected.InnerException.ShouldBeNull();
	}

	[Fact(DisplayName = "WaitForElements with specific count N throws exception after timeout when cssSelector does not result in N matching elements", Skip = "Need to figure out how to make this deterministic.")]
	[Trait("Category", "sync")]
	public void Test023()
	{
		var cut = Render<DelayRenderFragment>();

		var expected = Should.Throw<WaitForFailedException>(() =>
			cut.WaitForElements("#notHereElm", 2, WaitForTestTimeout));

		expected.Message.ShouldStartWith(string.Format(CultureInfo.InvariantCulture, WaitForElementsHelper<DelayRenderFragment>.TimeoutBeforeFoundWithCountMessage, 2));
		expected.InnerException.ShouldBeNull();
	}

	[Fact(DisplayName = "WaitForElements with specific count N waits until cssSelector returns at exact N elements")]
	[Trait("Category", "sync")]
	public void Test024()
	{
		var expectedMarkup = "<p>child content</p><p>child content</p><p>child content</p>";
		var cut = Render<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elms = cut.WaitForElements("main > p", matchElementCount: 3);

		elms.MarkupMatches(expectedMarkup);
	}

	[Fact(DisplayName = "WaitForElements with specific count 0 waits until cssSelector returns at exact zero elements")]
	[Trait("Category", "sync")]
	public void Test025()
	{
		var expectedMarkup = "<p>child content</p>";
		var cut = Render<DelayRemovedRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elms = cut.WaitForElements("main > p", matchElementCount: 0);

		elms.ShouldBeEmpty();
	}
}
