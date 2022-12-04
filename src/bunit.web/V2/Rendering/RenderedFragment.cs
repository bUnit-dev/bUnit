using AngleSharp.Dom;
using Microsoft.Extensions.Logging;

namespace Bunit.V2.Rendering;

public partial class RenderedFragment
{
	private readonly ILogger<RenderedFragment> logger;
	private Exception? onAfterRenderActionException;
	private Action? onAfterRenderAction;
	private TaskCompletionSource? onAfterRenderTask;

	public string Markup { get; internal set; }

	public INodeList Nodes { get; internal set; }

	public RenderedFragment(ILogger<RenderedFragment> logger)
	{
		this.logger = logger;
		Markup = string.Empty;
		Nodes = EmptyNodeList.Empty;
	}

	public async Task OnAfterRenderAsync(Action afterRenderAction, TimeSpan? timeout = null)
	{
		onAfterRenderAction = afterRenderAction;
		onAfterRenderTask = new TaskCompletionSource();
		TryAfterRenderAction();

		try
		{
			await onAfterRenderTask
				.Task
				.WaitAsync(timeout ?? TimeSpan.FromSeconds(1));

			onAfterRenderActionException = null;
			onAfterRenderAction = null;
			onAfterRenderTask = null;
		}
		catch (TimeoutException)
		{
			throw new TimeoutException("The after render action did not complete successfully before the timeout was reached.", onAfterRenderActionException);
		}
	}

	private void TryAfterRenderAction()
	{
		if (onAfterRenderAction is null || onAfterRenderTask is null)
		{
			return;
		}

		LogTryOnAfterAction(logger);

		try
		{
			onAfterRenderAction.Invoke();
			LogOnAfterActionCompleted(logger);
			onAfterRenderTask.SetResult();
		}
		catch (Exception ex)
		{
			onAfterRenderActionException = ex;
			LogOnAfterActionFailed(logger);
		}
	}

	internal void NotifyRenderComplete()
	{
		LogRenderedFragmentUpdated(logger);
		TryAfterRenderAction();
	}
}
