namespace Bunit.Rendering;

internal class BunitRendererSynchronizationContextDispatcher : Dispatcher
{
	private readonly BunitRendererSynchronizationContext context;

	public BunitRendererSynchronizationContextDispatcher()
	{
		context = new BunitRendererSynchronizationContext();
		context.UnhandledException += (sender, e) =>
		{
			OnUnhandledException(e);
		};
	}

	public override bool CheckAccess() => SynchronizationContext.Current == context;

	public override Task InvokeAsync(Action workItem)
	{
		if (CheckAccess())
		{
			workItem();
			return Task.CompletedTask;
		}

		return context.InvokeAsync(workItem);
	}

	public override Task InvokeAsync(Func<Task> workItem)
	{
		if (CheckAccess())
		{
			return workItem();
		}

		return context.InvokeAsync(workItem);
	}

	public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
	{
		if (CheckAccess())
		{
			return Task.FromResult(workItem());
		}

		return context.InvokeAsync<TResult>(workItem);
	}

	public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
	{
		if (CheckAccess())
		{
			return workItem();
		}

		return context.InvokeAsync<TResult>(workItem);
	}
}
