using Xunit.Abstractions;
using Xunit;
using System;
using Bunit.TestAssets.SampleComponents;
using Shouldly;

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
	}
}
