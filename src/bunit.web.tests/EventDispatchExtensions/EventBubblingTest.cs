using Bunit.TestAssets.SampleComponents;

using Shouldly;

using Xunit;

namespace Bunit.EventDispatchExtensions
{
	public class EventBubblingTest : ComponentTestFixture
	{
		[Fact(DisplayName = "When clicking on an element with an event handler, " +
							"event handlers higher up the DOM tree is also triggered", Skip = "fix with #119")]
		public void Test001()
		{
			var cut = RenderComponent<ClickEventBubbling>();

			cut.Find("span").Click();

			cut.Instance.SpanClickCount.ShouldBe(1);
			cut.Instance.HeaderClickCount.ShouldBe(1);
		}

		[Fact(DisplayName = "When clicking on an element without an event handler attached, " +
							"event handlers higher up the DOM tree is triggered", Skip = "fix with #119")]
		public void Test002()
		{
			var cut = RenderComponent<ClickEventBubbling>();

			cut.Find("button").Click();

			cut.Instance.SpanClickCount.ShouldBe(0);
			cut.Instance.HeaderClickCount.ShouldBe(1);
		}
	}
}
