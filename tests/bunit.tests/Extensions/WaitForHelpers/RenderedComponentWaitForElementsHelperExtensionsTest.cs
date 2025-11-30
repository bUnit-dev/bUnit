using System.Globalization;
using AngleSharp.Html.Dom;

namespace Bunit.Extensions.WaitForHelpers;

public class RenderedComponentWaitForElementsHelperExtensionsTest : BunitContext
{
	private readonly static TimeSpan WaitForTestTimeout = TimeSpan.FromMilliseconds(5);

	public RenderedComponentWaitForElementsHelperExtensionsTest(ITestOutputHelper testOutput)
	{
		BunitContext.DefaultWaitTimeout = TimeSpan.FromSeconds(10);
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

	[Fact(DisplayName = "WaitForElements with specific count N throws exception after timeout when cssSelector does not result in N matching elements")]
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

	[Fact(DisplayName = "WaitForElement<TElement> waits until element of specified type matching cssSelector appears")]
	[Trait("Category", "sync")]
	public void Test026()
	{
		var expectedMarkup = "<input type='text' id='myInput' />";
		var cut = Render<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elm = cut.WaitForElement<DelayRenderFragment, IHtmlInputElement>("#myInput");

		elm.ShouldNotBeNull();
		elm.Type.ShouldBe("text");
	}

	[Fact(DisplayName = "WaitForElement<TElement> throws exception when element type does not match")]
	[Trait("Category", "sync")]
	public void Test027()
	{
		var cut = Render<DelayRenderFragment>(ps => ps.AddChildContent("<div id='myDiv'></div>"));

		var expected = Should.Throw<WaitForFailedException>(() =>
			cut.WaitForElement<DelayRenderFragment, IHtmlInputElement>("#myDiv", WaitForTestTimeout));

		expected.InnerException.ShouldBeOfType<ElementNotFoundException>();
	}

	[Fact(DisplayName = "WaitForElements<TElement> waits until elements of specified type matching cssSelector appear")]
	[Trait("Category", "sync")]
	public void Test028()
	{
		var expectedMarkup = "<input type='text' /><div></div><input type='checkbox' />";
		var cut = Render<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elms = cut.WaitForElements<DelayRenderFragment, IHtmlInputElement>("main input");

		elms.Count.ShouldBe(2);
		elms[0].ShouldBeAssignableTo<IHtmlInputElement>();
		elms[1].ShouldBeAssignableTo<IHtmlInputElement>();
	}

	[Fact(DisplayName = "WaitForElements<TElement> with count waits until exactly N elements of specified type appear")]
	[Trait("Category", "sync")]
	public void Test029()
	{
		var expectedMarkup = "<input type='text' /><input type='checkbox' /><input type='password' />";
		var cut = Render<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

		var elms = cut.WaitForElements<DelayRenderFragment, IHtmlInputElement>("main input", matchElementCount: 3);

		elms.Count.ShouldBe(3);
	}
}
