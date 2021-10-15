using Xunit.Abstractions;
using Xunit;
using System;
using Bunit.TestAssets.SampleComponents;
using Shouldly;
using System.Globalization;

namespace Bunit.Extensions.WaitForHelpers
{
	public class RenderedFragmentWaitForElementsHelperExtensionsTest : TestContext
	{
		public RenderedFragmentWaitForElementsHelperExtensionsTest(ITestOutputHelper testOutput)
		{
			Services.AddXunitLogger(testOutput);
		}

		[Fact(DisplayName = "WaitForElement waits until cssSelector returns at a element")]
		public void Test001()
		{
			var expectedMarkup = "<p>child content</p>";
			var cut = RenderComponent<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

			var elm = cut.WaitForElement("main > p");

			elm.MarkupMatches(expectedMarkup);
		}

		[Fact(DisplayName = "WaitForElement throws exception after timeout when cssSelector does not result in matching element")]
		public void Test002()
		{
			var cut = RenderComponent<DelayRenderFragment>();

			var expected = Should.Throw<WaitForFailedException>(() =>
				cut.WaitForElement("#notHereElm", TimeSpan.FromMilliseconds(10)));

			expected.Message.ShouldBe(WaitForElementHelper.TimeoutBeforeFoundMessage);
			expected.InnerException.ShouldBeOfType<ElementNotFoundException>();
		}

		[Fact(DisplayName = "WaitForElements waits until cssSelector returns at least one element")]
		public void Test021()
		{
			var expectedMarkup = "<p>child content</p>";
			var cut = RenderComponent<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

			var elms = cut.WaitForElements("main > p");

			elms.MarkupMatches(expectedMarkup);
		}

		[Fact(DisplayName = "WaitForElements throws exception after timeout when cssSelector does not result in matching elements")]
		public void Test022()
		{
			var cut = RenderComponent<DelayRenderFragment>();

			var expected = Should.Throw<WaitForFailedException>(() =>
				cut.WaitForElements("#notHereElm", TimeSpan.FromMilliseconds(30)));

			expected.Message.ShouldBe(WaitForElementsHelper.TimeoutBeforeFoundMessage);
			expected.InnerException.ShouldBeNull();
		}

		[Fact(DisplayName = "WaitForElements with specific count N throws exception after timeout when cssSelector does not result in N matching elements")]
		public void Test023()
		{
			var cut = RenderComponent<DelayRenderFragment>();

			var expected = Should.Throw<WaitForFailedException>(() =>
				cut.WaitForElements("#notHereElm", 2, TimeSpan.FromMilliseconds(30)));

			expected.Message.ShouldBe(string.Format(CultureInfo.InvariantCulture, WaitForElementsHelper.TimeoutBeforeFoundWithCountMessage, 2));
			expected.InnerException.ShouldBeNull();
		}

		[Fact(DisplayName = "WaitForElements with specific count N waits until cssSelector returns at exact N elements")]
		public void Test024()
		{
			var expectedMarkup = "<p>child content</p><p>child content</p><p>child content</p>";
			var cut = RenderComponent<DelayRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

			var elms = cut.WaitForElements("main > p", matchElementCount: 3);

			elms.MarkupMatches(expectedMarkup);
		}

		[Fact(DisplayName = "WaitForElements with specific count 0 waits until cssSelector returns at exact zero elements")]
		public void Test025()
		{
			var expectedMarkup = "<p>child content</p>";
			var cut = RenderComponent<DelayRemovedRenderFragment>(ps => ps.AddChildContent(expectedMarkup));

			var elms = cut.WaitForElements("main > p", matchElementCount: 0);

			elms.ShouldBeEmpty();
		}
	}
}
