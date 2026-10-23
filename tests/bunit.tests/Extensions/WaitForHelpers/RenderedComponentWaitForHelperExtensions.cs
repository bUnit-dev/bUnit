using System.Runtime.CompilerServices;
using AngleSharp.Dom;

namespace Bunit.Extensions.WaitForHelpers;

public class RenderedComponentWaitForHelperExtensions : BunitContext
{
	public RenderedComponentWaitForHelperExtensions(ITestOutputHelper testOutput)
	{
		Services.AddXunitLogger(testOutput);
	}

	[Fact(DisplayName = "WaitForAssertion can wait for multiple renders and changes to occur")]
	public void Test110()
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
		cut.WaitForAssertion(() => cut.Find("#state").TextContent.ShouldBe("Stopped"));
	}

	[Fact(DisplayName = "WaitForAssertion throws assertion exception after timeout")]
	public void Test011()
	{
		var cut = Render<Simple1>();

		var expected = Should.Throw<WaitForFailedException>(() =>
			cut.WaitForAssertion(() => cut.Markup.ShouldBeEmpty(), TimeSpan.FromMilliseconds(10)));

		expected.Message.ShouldStartWith(WaitForAssertionHelper<Simple1>.TimeoutMessage);
	}

	[Fact(DisplayName = "WaitForState throws exception after timeout")]
	public void Test012()
	{
		var cut = Render<Simple1>();

		var expected = Should.Throw<WaitForFailedException>(() =>
			cut.WaitForState(() => string.IsNullOrEmpty(cut.Markup), TimeSpan.FromMilliseconds(100)));

		expected.Message.ShouldStartWith(WaitForStateHelper<Simple1>.TimeoutBeforePassMessage);
	}

	[Fact(DisplayName = "WaitForState throws exception if statePredicate throws on a later render")]
	public void Test013()
	{
		const string expectedInnerMessage = "INNER MESSAGE";
		var cut = Render<TwoRendersTwoChanges>();
		cut.Find("#tick").Click();
		cut.Find("#tock").Click();

		var expected = Should.Throw<WaitForFailedException>(() =>
			cut.WaitForState(() =>
			{
				if (cut.Find("#state").TextContent == "Stopped")
					throw new InvalidOperationException(expectedInnerMessage);
				return false;
			}));

		expected.Message.ShouldStartWith(WaitForStateHelper<Simple1>.ExceptionInPredicateMessage);
		expected.InnerException.ShouldBeOfType<InvalidOperationException>()
			.Message.ShouldBe(expectedInnerMessage);
	}

	[Fact(DisplayName = "WaitForState can wait for multiple renders and changes to occur")]
	public void Test100()
	{
		// Initial state is stopped
		var cut = Render<TwoRendersTwoChanges>();

		// Clicking 'tick' changes the state, and starts a task
		cut.Find("#tick").Click();
		cut.Find("#state").TextContent.ShouldBe("Started");

		// Clicking 'tock' completes the task, which updates the state
		// This click causes two renders, thus something is needed to await here.
		cut.Find("#tock").Click();
		cut.WaitForState(() =>
		{
			var elm = cut.Nodes.QuerySelector("#state");
			return elm?.TextContent == "Stopped";
		});
	}

	[Fact(DisplayName = "WaitForState can detect async changes to properties in the CUT")]
	public void Test200()
	{
		var cut = Render<AsyncRenderChangesProperty>();
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

	[Fact(DisplayName = "WaitForAssertion rethrows unhandled exception from a components async operation's methods")]
	public void Test300()
	{
		var cut = Render<ThrowsAfterAsyncOperation>();

		// Adding additional wait time to deal with tests sometimes failing for timeout on Windows.
		Should.Throw<ThrowsAfterAsyncOperation.ThrowsAfterAsyncOperationException>(
			() => cut.WaitForAssertion(() => false.ShouldBeTrue(), TimeSpan.FromSeconds(5)));
	}

	[Fact(DisplayName = "WaitForState rethrows unhandled exception from components async operation's methods")]
	public void Test301()
	{
		var cut = Render<ThrowsAfterAsyncOperation>();

		// Adding additional wait time to deal with tests sometimes failing for timeout on Windows.
		Should.Throw<ThrowsAfterAsyncOperation.ThrowsAfterAsyncOperationException>(
			() => cut.WaitForState(() => false, TimeSpan.FromSeconds(5)));
	}

	[Fact(DisplayName = "WaitForStateAsync with an async predicate can wait for multiple renders and changes to occur")]
	public async Task Test400()
	{
		var cut = Render<AsyncRenderChangesProperty>();
		cut.Instance.Counter.ShouldBe(0);

		// Clicking 'tick' changes the counter, and starts a task
		cut.Find("#tick").Click();
		cut.Instance.Counter.ShouldBe(1);

		// Clicking 'tock' completes the task, which updates the counter
		// This click causes two renders, thus something is needed to await here.
		cut.Find("#tock").Click();
		await cut.WaitForStateAsync(async () =>
		{
			await Task.Yield();
			return cut.Instance.Counter == 2;
		});

		cut.Instance.Counter.ShouldBe(2);
	}

	[Fact(DisplayName = "WaitForAssertionAsync with an async assertion can wait for multiple renders and changes to occur")]
	public async Task Test401()
	{
		// Initial state is stopped
		var cut = Render<TwoRendersTwoChanges>();
		cut.Find("#state").TextContent.ShouldBe("Stopped");

		// Clicking 'tick' changes the state, and starts a task
		cut.Find("#tick").Click();
		cut.Find("#state").TextContent.ShouldBe("Started");

		// Clicking 'tock' completes the task, which updates the state
		// This click causes two renders, thus something is needed to await here.
		cut.Find("#tock").Click();
		await cut.WaitForAssertionAsync(async () =>
		{
			await Task.Yield();
			cut.Find("#state").TextContent.ShouldBe("Stopped");
		});
	}

	[Fact(DisplayName = "WaitForStateAsync throws WaitForFailedException after timeout when the async predicate never passes")]
	public async Task Test402()
	{
		var cut = Render<Simple1>();

		var expected = await Should.ThrowAsync<WaitForFailedException>(async () =>
			await cut.WaitForStateAsync(async () =>
			{
				await Task.Yield();
				return false;
			}, TimeSpan.FromMilliseconds(50)));

		expected.Message.ShouldStartWith(WaitForStateHelper<Simple1>.TimeoutBeforePassMessage);
	}

	[Fact(DisplayName = "WaitForStateAsync throws WaitForFailedException if the async predicate throws")]
	public async Task Test403()
	{
		const string expectedInnerMessage = "INNER ASYNC MESSAGE";
		var cut = Render<Simple1>();

		var expected = await Should.ThrowAsync<WaitForFailedException>(async () =>
			await cut.WaitForStateAsync(async () =>
			{
				await Task.Yield();
				throw new InvalidOperationException(expectedInnerMessage);
			}));

		expected.Message.ShouldStartWith(WaitForStateHelper<Simple1>.ExceptionInPredicateMessage);
		expected.InnerException.ShouldBeOfType<InvalidOperationException>()
			.Message.ShouldBe(expectedInnerMessage);
	}

	[Fact(DisplayName = "WaitForStateAsync does not deadlock when the async predicate awaits work on the renderer dispatcher")]
	public async Task Test405()
	{
		var cut = Render<AsyncRenderChangesProperty>();

		// Clicking 'tick' then 'tock' causes the counter to reach 2 across two renders.
		cut.Find("#tick").Click();
		cut.Find("#tock").Click();

		// The predicate awaits work routed through the renderer's dispatcher. A naive
		// sync-over-async implementation (e.g. blocking the dispatcher with .GetResult())
		// would deadlock here; awaiting on the dispatcher does not.
		await cut.WaitForStateAsync(async () =>
		{
			var counter = await cut.InvokeAsync(() => cut.Instance.Counter);
			return counter == 2;
		});

		cut.Instance.Counter.ShouldBe(2);
	}

	[Fact(DisplayName = "WaitForAssertionAsync throws WaitForFailedException after timeout when the async assertion never passes")]
	public async Task Test404()
	{
		var cut = Render<Simple1>();

		var expected = await Should.ThrowAsync<WaitForFailedException>(async () =>
			await cut.WaitForAssertionAsync(async () =>
			{
				await Task.Yield();
				cut.Markup.ShouldBeEmpty();
			}, TimeSpan.FromMilliseconds(50)));

		expected.Message.ShouldStartWith(WaitForAssertionHelper<Simple1>.TimeoutMessage);
	}

	[Fact(DisplayName = "WaitForAssertion does not keep the renderer alive after the first check passes")]
	public async Task Test500()
	{
		// The first call is slow while it is being JIT compiled, which is enough for the wait task
		// to complete before the helper decides whether to start its timer, so it is not measured.
		RenderAndWaitForAssertionThatPassesAtOnce();

		var renderers = Enumerable.Range(0, 10)
			.Select(_ => RenderAndWaitForAssertionThatPassesAtOnce())
			.ToList();

		// Disposing a context queues work that still references the renderer: cancelling its
		// unhandled exception task wakes the helper's failure continuation on the thread pool, and
		// the renderer's own base disposal goes through the dispatcher. On a busy test host that
		// can still be queued when the first collection runs, so the collection is retried for a
		// short while. A timer left running roots the renderer for ten minutes, so no number of
		// retries makes the leaking case pass.
		var alive = renderers.Count;
		for (var attempt = 0; attempt < 20 && alive > 0; attempt++)
		{
			if (attempt > 0)
				await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);

#pragma warning disable S1215 // A full collection is what proves the renderers are no longer rooted.
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
#pragma warning restore S1215

			alive = renderers.Count(r => r.IsAlive);
		}

		alive.ShouldBe(0, "a wait whose first check passed should leave no timer running");
	}

	// Kept out of the test method so that nothing on its stack frame keeps the context alive.
	[MethodImpl(MethodImplOptions.NoInlining)]
	private static WeakReference RenderAndWaitForAssertionThatPassesAtOnce()
	{
		using var context = new BunitContext();
		var cut = context.Render<Simple1>();

		// A timeout far longer than the test: a timer left running roots the helper,
		// and through it the renderer, until it fires.
		cut.WaitForAssertion(() => { }, TimeSpan.FromMinutes(10));

		return new WeakReference(context.Renderer);
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
