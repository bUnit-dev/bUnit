using AngleSharp.Dom;

namespace Bunit.Extensions.WaitForHelpers;

public class RenderedFragmentWaitForHelperExtensionsTest : TestContext
{
	public RenderedFragmentWaitForHelperExtensionsTest(ITestOutputHelper testOutput)
	{
		Services.AddXunitLogger(testOutput);
	}

	[Fact(DisplayName = "WaitForAssertion can wait for multiple renders and changes to occur")]
	public async Task Test110()
	{
		// Initial state is stopped
		var cut = Render<TwoRendersTwoChanges>();
		var stateElement = cut.Find("#state");
		stateElement.TextContent.ShouldBe("Stopped");

		// Clicking 'tick' changes the state, and starts a task
		cut.Find("#tick").Click();
		cut.Find("#state").TextContent.ShouldBe("Started");

		// Clicking 'tock' completes the task, which updates the state
		// This click causes two renders, thus something is needed to await here.
		cut.Find("#tock").Click();
		await cut.WaitForAssertionAsync(() => cut.Find("#state").TextContent.ShouldBe("Stopped"));
	}

	[Fact(DisplayName = "WaitForAssertion throws assertion exception after timeout")]
	public async Task Test011()
	{
		var cut = Render<Simple1>();

		var expected = await Should.ThrowAsync<WaitForFailedException>(async () =>
			await cut.WaitForAssertionAsync(() => cut.Markup.ShouldBeEmpty(), TimeSpan.FromMilliseconds(10)));

		expected.Message.ShouldStartWith(WaitForAssertionHelper.TimeoutMessage);
	}

	[Fact(DisplayName = "WaitForState throws exception after timeout")]
	public async Task Test012()
	{
		var cut = Render<Simple1>();

		var expected = await Should.ThrowAsync<WaitForFailedException>(async () =>
			await cut.WaitForStateAsync(() => string.IsNullOrEmpty(cut.Markup), TimeSpan.FromMilliseconds(100)));

		expected.Message.ShouldStartWith(WaitForStateHelper.TimeoutBeforePassMessage);
	}

	[Fact(DisplayName = "WaitForState throws exception if statePredicate throws on a later render")]
	public async Task Test013()
	{
		const string expectedInnerMessage = "INNER MESSAGE";
		var cut = Render<TwoRendersTwoChanges>();
		cut.Find("#tick").Click();
		cut.Find("#tock").Click();

		var expected = await Should.ThrowAsync<WaitForFailedException>(async () =>
			await cut.WaitForStateAsync(() =>
			{
				if (cut.Find("#state").TextContent == "Stopped")
					throw new InvalidOperationException(expectedInnerMessage);
				return false;
			}));

		expected.Message.ShouldStartWith(WaitForStateHelper.ExceptionInPredicateMessage);
		expected.InnerException.ShouldBeOfType<InvalidOperationException>()
			.Message.ShouldBe(expectedInnerMessage);
	}

	[Fact(DisplayName = "WaitForState can wait for multiple renders and changes to occur")]
	public async Task Test100()
	{
		// Initial state is stopped
		var cut = Render<TwoRendersTwoChanges>();

		// Clicking 'tick' changes the state, and starts a task
		cut.Find("#tick").Click();
		cut.Find("#state").TextContent.ShouldBe("Started");

		// Clicking 'tock' completes the task, which updates the state
		// This click causes two renders, thus something is needed to await here.
		cut.Find("#tock").Click();
		await cut.WaitForStateAsync(() =>
		{
			var elm = cut.Nodes.QuerySelector("#state");
			return elm?.TextContent == "Stopped";
		});
	}

	[Fact(DisplayName = "WaitForState can detect async changes to properties in the CUT")]
	public async Task Test200()
	{
		var cut = Render<AsyncRenderChangesProperty>();
		cut.Instance.Counter.ShouldBe(0);

		// Clicking 'tick' changes the counter, and starts a task
		cut.Find("#tick").Click();
		cut.Instance.Counter.ShouldBe(1);

		// Clicking 'tock' completes the task, which updates the counter
		// This click causes two renders, thus something is needed to await here.
		cut.Find("#tock").Click();
		await cut.WaitForStateAsync(() => cut.Instance.Counter == 2);

		cut.Instance.Counter.ShouldBe(2);
	}

	[Fact(DisplayName = "WaitForAssertion rethrows unhandled exception from a components async operation's methods")]
	public async Task Test300()
	{
		var cut = Render<ThrowsAfterAsyncOperation>();

		// Adding additional wait time to deal with tests sometimes failing for timeout on Windows.
		await Should.ThrowAsync<ThrowsAfterAsyncOperation.ThrowsAfterAsyncOperationException>(
			async () => await cut.WaitForAssertionAsync(() => false.ShouldBeTrue(), TimeSpan.FromSeconds(5)));
	}

	[Fact(DisplayName = "WaitForState rethrows unhandled exception from components async operation's methods")]
	public async Task Test301()
	{
		var cut = Render<ThrowsAfterAsyncOperation>();

		// Adding additional wait time to deal with tests sometimes failing for timeout on Windows.
		await Should.ThrowAsync<ThrowsAfterAsyncOperation.ThrowsAfterAsyncOperationException>(
			async () => await cut.WaitForStateAsync(() => false, TimeSpan.FromSeconds(5)));
	}

	private sealed class ThrowsAfterAsyncOperation : ComponentBase
	{
		protected override async Task OnInitializedAsync()
		{
			await InvokeAsync(async () =>
			{
				await Task.Delay(100);
				throw new ThrowsAfterAsyncOperationException();
			});
		}

		internal sealed class ThrowsAfterAsyncOperationException : Exception { }
	}
}
