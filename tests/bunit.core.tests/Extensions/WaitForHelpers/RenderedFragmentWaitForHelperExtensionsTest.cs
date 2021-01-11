using System;
using Bunit.TestAssets.SampleComponents;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Bunit.Extensions.WaitForHelpers
{
	public class RenderedFragmentWaitForHelperExtensionsTest : TestContext
	{
		public RenderedFragmentWaitForHelperExtensionsTest(ITestOutputHelper testOutput)
		{
			Services.AddXunitLogger(testOutput);
		}

		[Fact(DisplayName = "WaitForAssertion can wait for multiple renders and changes to occur")]
		public void Test110()
		{
			// Initial state is stopped
			var cut = RenderComponent<TwoRendersTwoChanges>();
			var stateElement = cut.Find("#state");
			stateElement.TextContent.ShouldBe("Stopped");

			// Clicking 'tick' changes the state, and starts a task
			cut.Find("#tick").Click();
			cut.Find("#state").TextContent.ShouldBe("Started");

			// Clicking 'tock' completes the task, which updates the state
			// This click causes two renders, thus something is needed to await here.
			cut.Find("#tock").Click();
			cut.WaitForAssertion(() => cut.Find("#state").TextContent.ShouldBe("Stopped"));
		}

		[Fact(DisplayName = "WaitForAssertion throws assertion exception after timeout")]
		public void Test011()
		{
			var cut = RenderComponent<Simple1>();

			var expected = Should.Throw<WaitForFailedException>(() =>
				cut.WaitForAssertion(() => cut.Markup.ShouldBeEmpty(), TimeSpan.FromMilliseconds(10)));

			expected.Message.ShouldBe(WaitForAssertionHelper.TimeoutMessage);
			expected.InnerException.ShouldBeOfType<ShouldAssertException>();
		}

		[Fact(DisplayName = "WaitForState throws exception after timeout")]
		public void Test012()
		{
			var cut = RenderComponent<Simple1>();

			var expected = Should.Throw<WaitForFailedException>(() =>
				cut.WaitForState(() => string.IsNullOrEmpty(cut.Markup), TimeSpan.FromMilliseconds(100)));

			expected.Message.ShouldBe(WaitForStateHelper.TimeoutBeforePassMessage);
		}

		[Fact(DisplayName = "WaitForState throws exception if statePredicate throws on a later render")]
		public void Test013()
		{
			const string expectedInnerMessage = "INNER MESSAGE";
			var cut = RenderComponent<TwoRendersTwoChanges>();
			cut.Find("#tick").Click();
			cut.Find("#tock").Click();

			var expected = Should.Throw<WaitForFailedException>(() =>
				cut.WaitForState(() =>
				{
					if (cut.Find("#state").TextContent == "Stopped")
						throw new InvalidOperationException(expectedInnerMessage);
					return false;
				}));

			expected.Message.ShouldBe(WaitForStateHelper.ExceptionInPredicateMessage);
			expected.InnerException.ShouldBeOfType<InvalidOperationException>()
				.Message.ShouldBe(expectedInnerMessage);
		}

		[Fact(DisplayName = "WaitForState can wait for multiple renders and changes to occur")]
		public void Test100()
		{
			// Initial state is stopped
			var cut = RenderComponent<TwoRendersTwoChanges>();
			var stateElement = cut.Find("#state");
			stateElement.TextContent.ShouldBe("Stopped");

			// Clicking 'tick' changes the state, and starts a task
			cut.Find("#tick").Click();
			cut.Find("#state").TextContent.ShouldBe("Started");

			// Clicking 'tock' completes the task, which updates the state
			// This click causes two renders, thus something is needed to await here.
			cut.Find("#tock").Click();
			cut.WaitForState(() => cut.Find("#state").TextContent == "Stopped");

			cut.Find("#state").TextContent.ShouldBe("Stopped");
		}

		[Fact(DisplayName = "WaitForState can detect async changes to properties in the CUT")]
		public void Test200()
		{
			var cut = RenderComponent<AsyncRenderChangesProperty>();
			cut.Instance.Counter.ShouldBe(0);

			// Clicking 'tick' changes the counter, and starts a task
			cut.Find("#tick").Click();
			cut.Instance.Counter.ShouldBe(1);

			// Clicking 'tock' completes the task, which updates the counter
			// This click causes two renders, thus something is needed to await here.
			cut.Find("#tock").Click();
			cut.WaitForState(() => cut.Instance.Counter == 2);

			cut.Instance.Counter.ShouldBe(2);
		}
	}
}
