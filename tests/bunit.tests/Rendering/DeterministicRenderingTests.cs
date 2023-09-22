
namespace Bunit.Rendering;

public class DeterministicRenderingTests : TestContext
{
	[Fact]
	public void Renderer_blocks_external_render_requests()
	{
		var cut = Render<RemoteRenderTriggerComponent>();

		var result = cut.Instance.TriggerRender(); // emulates something inside the component

		result.Status.ShouldBe(TaskStatus.WaitingForActivation);
		cut.Instance.RendersCompleted.ShouldBe(1);
	}

	[Fact]
	public async Task Dispose_unblocks_waiting_tasks_in_dispatcher()
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

	[Fact(Skip = "TDOO")]
	public async Task WaitFor_allow_blocked_renders_to_proceed()
	{
		var cut = Render<RemoteRenderTriggerComponent>();
		var triggerTask = cut.Instance.TriggerRender();

		await cut.WaitForAssertionAsync(() => cut.Instance.RendersCompleted.ShouldBe(2));

		triggerTask.Status.ShouldBe(TaskStatus.RanToCompletion);
	}

	private sealed class RemoteRenderTriggerComponent : ComponentBase
	{
		public int RendersCompleted { get; private set; }

		public async Task TriggerRender()
			=> await InvokeAsync(StateHasChanged)
			.ConfigureAwait(false); // this should force the invocation/waiting task to be on the thread pool

		protected override void OnAfterRender(bool firstRender)
		{
			RendersCompleted++;
		}
	}
}
