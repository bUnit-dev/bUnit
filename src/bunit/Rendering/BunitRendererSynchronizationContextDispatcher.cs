using System.Diagnostics;

namespace Bunit.Rendering;

internal class BunitRendererSynchronizationContextDispatcher : Dispatcher
{
	private readonly object bulkheadsLock = new();
	private readonly BunitRendererSynchronizationContext context;
	private readonly Queue<TaskCompletionSource> workItemBulkheads = new();
	private Func<bool>? unblockWaitingTask;
	private Action? onAfterDispatchCompleted;
	private Action<Exception>? onDispatchException;

	private Task CreateBulkhead()
	{
		lock (bulkheadsLock)
		{
			if (unblockWaitingTask is not null && unblockWaitingTask())
			{
				return Task.CompletedTask;
			}
			else
			{
				var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
				workItemBulkheads.Enqueue(tcs);
				return tcs.Task;
			}
		}
	}

	public int WaitingTasks => workItemBulkheads.Count;

	public BunitRendererSynchronizationContextDispatcher()
	{
		// pass in a dummy bulkhead to ensure the sync context doesn't executes
		// workitems immediately if its taskQueue is completed.
		context = new BunitRendererSynchronizationContext(CreateBulkhead());
		context.UnhandledException += (sender, e) =>
		{
			if (e.ExceptionObject is Exception ex)
				onDispatchException?.Invoke(ex);

			OnUnhandledException(e);
		};
	}

	public void Subscribe(
		Func<bool> unblockWaitingTask,
		Action onAfterDispatchCompleted,
		Action<Exception> onDispatchException)
	{
		lock (bulkheadsLock)
		{
			this.unblockWaitingTask += unblockWaitingTask;
			this.onAfterDispatchCompleted += onAfterDispatchCompleted;
			this.onDispatchException += onDispatchException;

			// proceed to dequeue any existing bulkheads until.
			while (this.unblockWaitingTask() && workItemBulkheads.TryDequeue(out var tcs))
			{
				tcs.SetResult();
			}
		}
	}

	public void Unsubscribe(
		Func<bool> unblockWaitingTask,
		Action onAfterDispatchCompleted,
		Action<Exception> onDispatchException)
	{
		lock (bulkheadsLock)
		{
			this.unblockWaitingTask -= unblockWaitingTask;
			this.onAfterDispatchCompleted -= onAfterDispatchCompleted;
			this.onDispatchException -= onDispatchException;
		}
	}

	public Task ReleaseAllAndInvoke(Action workItem)
	{
		lock (bulkheadsLock)
		{
			var result = InvokeAsync(workItem);

			// because the lock prevents additional bulkheads from
			// being added to the queue, we can safely assume that
			// the workItem passed to this method is the last item
			// in the queue and we can release all bulkheads.
			while (workItemBulkheads.TryDequeue(out var tcs))
			{
				tcs.SetResult();
			}

			return result;
		}
	}

	public Task<TResult> ReleaseAllAndInvoke<TResult>(Func<TResult> workItem)
	{
		lock (bulkheadsLock)
		{
			var result = InvokeAsync(workItem);

			while (workItemBulkheads.TryDequeue(out var tcs))
			{
				tcs.SetResult();
			}

			return result;
		}
	}

	public override bool CheckAccess() => SynchronizationContext.Current == context;

	public override Task InvokeAsync(Action workItem)
	{
		lock (bulkheadsLock)
		{
			var bulkheadTask = CreateBulkhead();
			return context.InvokeAsync(async () =>
			{
				await bulkheadTask;

				try
				{
					workItem.Invoke();
					onAfterDispatchCompleted?.Invoke();
				}
				catch (Exception e)
				{
					onDispatchException?.Invoke(e);
					throw;
				}
			});
		}
	}

	public override Task InvokeAsync(Func<Task> workItem)
	{
		lock (bulkheadsLock)
		{
			var bulkheadTask = CreateBulkhead();

			return context.InvokeAsync(async () =>
			{
				await bulkheadTask;

				try
				{
					await workItem();
					onAfterDispatchCompleted?.Invoke();
				}
				catch (Exception e)
				{
					onDispatchException?.Invoke(e);
					throw;
				}
			});
		}
	}

	public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
	{
		lock (bulkheadsLock)
		{
			var bulkheadTask = CreateBulkhead();
			return context.InvokeAsync(async () =>
			{
				await bulkheadTask;

				try
				{
					var result = workItem();
					onAfterDispatchCompleted?.Invoke();
					return result;
				}
				catch (Exception e)
				{
					onDispatchException?.Invoke(e);
					throw;
				}
			});
		}
	}

	public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
	{
		lock (bulkheadsLock)
		{
			var bulkheadTask = CreateBulkhead();

			return context.InvokeAsync(async () =>
			{
				await bulkheadTask;

				try
				{
					var result = await workItem();
					onAfterDispatchCompleted?.Invoke();
					return result;
				}
				catch (Exception e)
				{
					onDispatchException?.Invoke(e);
					throw;
				}
			});
		}
	}
}
