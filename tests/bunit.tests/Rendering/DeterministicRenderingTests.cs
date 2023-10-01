
using System;
using Xunit;

namespace Bunit.Rendering;

/// <summary>
/// Deterministic renderer requirements and assumptions:
/// 
/// 1. Each waiting async operation should be executed one at the time and allow test (wait for) to inspect
///    state between each blocked operation.
///
/// 2. If a one or more timers are in play, an un-deterministic number of async operations
///    could be waiting to allowed to run. E.g. if a two or more components have been rendered which all have
///    timers that trigger a render, the order these timers are will be triggered is un-deterministic.
///    That means a any number of work items may be waiting and queued up in the dispatcher.
///    Besides recommending using things like TimeProvider we cannot make this part of user code deterministic.
///
/// 3. Should there be different tasks queues per root component?
///
/// 4. WaitFor should be invoked for each queued action. This should happen independent of whether a
///    component has rendered or had its markup changed (change from v1). This also enables WaitFors to be
///    triggered by none-rendering changes, e.g. NavMan.
/// 
/// 5. Queued actions should always be invoked in order.
///
/// 6. Multiple WaitFor should be allowed.
///
/// 7. Code running inside WaitFor lambdas cannot be blocking or be async.
/// 
/// </summary>
public class DeterministicRenderingTests : TestContext
{
	[Fact]
	public void Renderer_blocks_external_render_requests()
	{
		var cut = Render<RemoteRenderTriggerComponent>();

		var result = cut.Instance.TriggerRender(); // emulates something inside the component

		result.Status.ShouldBe(TaskStatus.WaitingForActivation);
		Renderer.DispatcherQueueLength.ShouldBe(1);
		cut.Instance.RendersCompleted.ShouldBe(1);
	}

	[Fact]
	public async Task Dispose_TestContext_unblocks_waiting_tasks_in_dispatcher()
	{
		var cut = Render<RemoteRenderTriggerComponent>();
		var completed = false;
		var result = cut.InvokeAsync(() => completed = true);

		Dispose();

		await result;
		completed.ShouldBeTrue();
	}

	[Fact]
	public async Task Second_Render_call_unblocks_waiting()
	{
		var c1 = Render<RemoteRenderTriggerComponent>();
		var result = c1.Instance.TriggerRender();

		var c2 = Render<RemoteRenderTriggerComponent>();

		await result;
		c1.Instance.RendersCompleted.ShouldBe(2);
		c2.Instance.RendersCompleted.ShouldBe(1);
	}

	[Fact]
	public async Task Renderer_blocks_multiple_external_render_requests()
	{
		var c1 = Render<RemoteRenderTriggerComponent>();
		Task[] results = [c1.Instance.TriggerRender(), c1.Instance.TriggerRender()];

		Render<RemoteRenderTriggerComponent>();

		await Task.WhenAll(results);
		c1.Instance.RendersCompleted.ShouldBe(3);
	}

	[Fact]
	public async Task WaitFor_allow_blocked_renders_to_proceed()
	{
		var cut = Render<RemoteRenderTriggerComponent>();
		var triggerTask = cut.Instance.TriggerRender();

		await cut.WaitForAssertionAsync(() => cut.Instance.RendersCompleted.ShouldBe(2));

		await triggerTask;
	}

	[Fact]
	public async Task WaitFor_waits_for_renders()
	{
		var cut = Render<RemoteRenderTriggerComponent>();
		var waitForTask = cut.WaitForAssertionAsync(() => cut.Instance.RendersCompleted.ShouldBe(2));

		var triggerTask = cut.Instance.TriggerRender();

		await waitForTask;
		await triggerTask;
	}

	[Fact]
	public async Task WaitFor_allow_waits_for_multiple_renders()
	{
		var cut = Render<RemoteRenderTriggerComponent>();
		var waitForTask = cut.WaitForAssertionAsync(() => cut.Instance.RendersCompleted.ShouldBe(3));

		await cut.Instance.TriggerRender();
		await cut.InvokeAsync(() => { /* do nothing work item that emulates other work queued up */ });
		await cut.Instance.TriggerRender();

		await waitForTask;
	}

	[Fact]
	public async Task WaitFor_only_unblocks_needed_waiting_workitems()
	{
		var cut = Render<RemoteRenderTriggerComponent>();
		var waitForTask = cut.WaitForAssertionAsync(() => cut.Instance.RendersCompleted.ShouldBe(2));
		await cut.Instance.TriggerRender();

		var t1 = cut.InvokeAsync(() => throw new Exception("do nothing work item that emulates other work queued up"));

		await waitForTask;
		t1.Status.ShouldBe(TaskStatus.WaitingForActivation);
		Renderer.DispatcherQueueLength.ShouldBe(1);
	}

	[Fact]
	public async Task Triggering_renders_from_WaitFor_allowed()
	{
		var cut = Render<RemoteRenderTriggerComponent>();

		await cut.WaitForAssertionAsync(() =>
		{
			_ = cut.Instance.TriggerRender();
			cut.RenderCount.ShouldBe(2);
		});
	}

	[Fact]
	public async Task Multiple_WaitFors_complete_simultaneously()
	{
		var cut = Render<RemoteRenderTriggerComponent>();
		var waitForTask1 = cut.WaitForAssertionAsync(() => cut.Instance.RendersCompleted.ShouldBe(2));
		var waitForTask2 = cut.WaitForAssertionAsync(() => cut.Instance.RendersCompleted.ShouldBe(2));

		await cut.Instance.TriggerRender();

		await waitForTask1;
		await waitForTask2;
	}

	[Fact]
	public async Task Multiple_WaitFors_complete_independently()
	{
		var cut = Render<RemoteRenderTriggerComponent>();
		var waitForTask1 = cut.WaitForAssertionAsync(() => cut.Instance.RendersCompleted.ShouldBe(2));
		var waitForTask2 = cut.WaitForAssertionAsync(() => cut.Instance.RendersCompleted.ShouldBe(3));

		await cut.Instance.TriggerRender();
		await waitForTask1;
		await cut.Instance.TriggerRender();

		await waitForTask2;
	}

	[Fact]
	public async Task All_WaitFor_unsubscribe_blocks_dispatcher_queue()
	{
		var cut = Render<RemoteRenderTriggerComponent>();
		var waitForTask1 = cut.WaitForAssertionAsync(() => cut.Instance.RendersCompleted.ShouldBe(2));
		var waitForTask2 = cut.WaitForAssertionAsync(() => cut.Instance.RendersCompleted.ShouldBe(2));
		await cut.Instance.TriggerRender();

		var t1 = cut.InvokeAsync(() => throw new Exception("do nothing work item that emulates other work queued up"));

		await waitForTask1;
		await waitForTask2;
		t1.Status.ShouldBe(TaskStatus.WaitingForActivation);
		Renderer.DispatcherQueueLength.ShouldBe(1);
	}

	[Theory, AutoData]
	public async Task WaitFor_breaks_when_queued_task_throws(Exception exceptedException)
	{
		var cut = Render<NoopComponent>();
		_ = cut.InvokeAsync(() => throw exceptedException);

		var waitFor = cut.WaitForStateAsync(() => cut.Instance.RendersCompleted == 2);

		var actualException = await waitFor.ShouldThrowAsync<Exception>();
		actualException.ShouldBe(exceptedException);
	}

	[Theory, AutoData]
	public async Task WaitFor_breaks_when_scheduled_task_throws(Exception exceptedException)
	{
		var cut = Render<NoopComponent>();
		var waitFor = cut.WaitForStateAsync(() => cut.Instance.RendersCompleted == 2);

		_ = cut.InvokeAsync(() => throw exceptedException);

		var actualException = await waitFor.ShouldThrowAsync<Exception>();
		actualException.ShouldBe(exceptedException);
	}

	[Fact]
	public async Task WaitFor_allows_each_queued_task_to_complete()
	{
		var cut = Render<RemoteRenderTriggerComponent>();
		var completion = new TaskCompletionSource();
		var t1 = cut.InvokeAsync(async () => { await completion.Task; await cut.Instance.TriggerRender(); });
		var t2 = cut.InvokeAsync(() => throw new Exception("Should not be called"));

		var waitFor = cut.WaitForStateAsync(() => cut.Instance.RendersCompleted == 2);
		completion.SetResult();

		await waitFor;
		t2.Status.ShouldBe(TaskStatus.WaitingForActivation);
	}

	[Fact]
	public void Disposing_Renderer_should_not_be_blocked()
	{
		var cut = Render<RemoteRenderTriggerComponent>();
		var completion = new TaskCompletionSource();
		var runningTask = cut.InvokeAsync(async () => { await completion.Task; await cut.Instance.TriggerRender(); });

		DisposeComponents();

		runningTask.Status.ShouldBe(TaskStatus.WaitingForActivation);
	}

	[Fact]
	public async Task Triggering_renders_in_parent_is_not_influenced_by_child_component()
	{
		var cut = Render<RemoteRenderTriggerComponent>(p => p.AddChildContent<NeverFinishedComponent>());
		var completion = new TaskCompletionSource();
		var runningTask = cut.InvokeAsync(async () =>
		{
			await completion.Task;
			await cut.Instance.TriggerRender();
		});

		await cut.WaitForStateAsync(() => cut.Instance.RendersCompleted == 2);

		runningTask.Status.ShouldBe(TaskStatus.RanToCompletion);
	}
}

file sealed class NoopComponent : ComponentBase
{
	public int RendersCompleted { get; private set; }

	protected override void OnAfterRender(bool firstRender) => RendersCompleted++;
}

file sealed class RemoteRenderTriggerComponent : ComponentBase
{
	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	public int RendersCompleted { get; private set; }

	public async Task TriggerRender()
		=> await InvokeAsync(StateHasChanged).ConfigureAwait(false); // this should force the invocation/waiting task to be on the thread pool

	protected override void OnAfterRender(bool firstRender)
		=> RendersCompleted++;
}

file sealed class NeverFinishedComponent : ComponentBase
{
	private readonly TaskCompletionSource tcs = new();

	protected override async Task OnInitializedAsync()
	{
		await tcs.Task;
	}
}

