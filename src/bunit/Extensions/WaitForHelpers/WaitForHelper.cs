using System.Diagnostics;
using Bunit.Rendering;
using Microsoft.Extensions.Logging;

namespace Bunit.Extensions.WaitForHelpers;

/// <summary>
/// Represents a helper class that can wait for a render notifications from a <see cref="RenderedFragment"/> type,
/// until a specific timeout is reached.
/// </summary>
public abstract class WaitForHelper<T> : IDisposable
{
	private readonly TaskCompletionSource<T> completion;
	private readonly Func<(bool CheckPassed, T Content)> completeChecker;
	private readonly RenderedFragment renderedFragment;
	private readonly ILogger<WaitForHelper<T>> logger;
#pragma warning disable CA2213 // not owned by this type
	private readonly BunitRenderer renderer;
#pragma warning restore CA2213
	private readonly TimeProvider timeProvider;
	private ITimer? timeoutTimer;
	private bool isDisposed;
	private int checkCount;
	private Exception? capturedException;

	/// <summary>
	/// Gets the error message passed to the user when the wait for helper times out.
	/// </summary>
	protected virtual string? TimeoutErrorMessage { get; }

	/// <summary>
	/// Gets the error message passed to the user when the wait for checker throws an exception.
	/// Only used if <see cref="StopWaitingOnCheckException"/> is true.
	/// </summary>
	protected virtual string? CheckThrowErrorMessage { get; }

	/// <summary>
	/// Gets a value indicating whether to continue waiting if the wait condition checker throws.
	/// </summary>
	protected abstract bool StopWaitingOnCheckException { get; }

	/// <summary>
	/// Gets the task that will complete successfully if the check passed before the timeout was reached.
	/// The task will complete with an <see cref="WaitForFailedException"/> exception if the timeout was reached without the check passing.
	/// </summary>
	public Task<T> WaitTask { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="WaitForHelper{T}"/> class.
	/// </summary>
	protected WaitForHelper(
		RenderedFragment renderedFragment,
		Func<(bool CheckPassed, T Content)> completeChecker,
		TimeSpan? timeout = null)
	{
		this.renderedFragment = renderedFragment ?? throw new ArgumentNullException(nameof(renderedFragment));
		this.completeChecker = completeChecker ?? throw new ArgumentNullException(nameof(completeChecker));

		logger = renderedFragment.Services.CreateLogger<WaitForHelper<T>>();
		renderer = renderedFragment
			.Services
			.GetRequiredService<BunitRenderer>();
		timeProvider = renderedFragment.Services.GetService<TimeProvider>() ?? TimeProvider.System;

		completion = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

		WaitTask = completion.Task;

		InitializeWaiting(GetRuntimeTimeout(timeout));
	}

	/// <summary>
	/// Disposes the wait helper and cancels the any ongoing waiting, if it is not
	/// already in one of the other completed states.
	/// </summary>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Disposes of the wait task and related logic.
	/// </summary>
	/// <remarks>
	/// The disposing parameter should be false when called from a finalizer, and true when called from the
	/// <see cref="Dispose()"/> method. In other words, it is true when deterministically called and false when non-deterministically called.
	/// </remarks>
	/// <param name="disposing">Set to true if called from <see cref="Dispose()"/>, false if called from a finalizer.f.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (isDisposed || !disposing)
			return;

		lock (completion)
		{
			isDisposed = true;
			renderer.BunitDispatcher.Unsubscribe(
				UnblockWaitingTask,
				OnAfterDispatchCompleted,
				OnDispatchException);

			timeoutTimer?.Dispose();
			completion.TrySetCanceled();
			logger.LogWaiterDisposed(renderedFragment.ComponentId);
		}
	}

	private void InitializeWaiting(TimeSpan timeout)
	{
		if (CheckWaitCondition())
			return;

		renderer.BunitDispatcher.Subscribe(
			UnblockWaitingTask,
			OnAfterDispatchCompleted,
			OnDispatchException);

		if (timeout > TimeSpan.Zero)
		{
			timeoutTimer = timeProvider.CreateTimer(static state =>
			{
				var helper = (WaitForHelper<T>)state!;
				helper.OnTimeout();
			}, this, timeout, Timeout.InfiniteTimeSpan);
		}
	}

	private bool UnblockWaitingTask()
	{
		if (isDisposed || WaitTask.IsCompleted)
			return false;

		lock (completion)
		{
			return !isDisposed && !WaitTask.IsCompleted;
		}
	}

	private void OnAfterDispatchCompleted()
	{
		CheckWaitCondition();
	}

	private void OnDispatchException(Exception exception)
	{
		if (isDisposed || WaitTask.IsCompleted)
			return;

		lock (completion)
		{
			if (isDisposed || WaitTask.IsCompleted)
				return;

			completion.TrySetException(exception);
		}
	}

	private void OnTimeout()
	{
		if (isDisposed || WaitTask.IsCompleted)
			return;

		lock (completion)
		{
			if (isDisposed || WaitTask.IsCompleted)
				return;

			completion.TrySetException(
				new WaitForFailedException(
					TimeoutErrorMessage ?? string.Empty,
					checkCount,
					renderedFragment.RenderCount,
					renderer.RenderCount,
					capturedException));
		}
	}

	private bool CheckWaitCondition()
	{
		if (isDisposed || WaitTask.IsCompleted)
			return true;

		lock (completion)
		{
			if (isDisposed || WaitTask.IsCompleted)
				return true;

			try
			{
				logger.LogCheckingWaitCondition(renderedFragment.ComponentId);

				var checkResult = completeChecker();
				checkCount++;
				if (checkResult.CheckPassed)
				{
					completion.TrySetResult(checkResult.Content);
					logger.LogCheckCompleted(renderedFragment.ComponentId);
					Dispose();
					return true;
				}
				else
				{
					logger.LogCheckFailed(renderedFragment.ComponentId);
				}
			}
			catch (Exception ex)
			{
				checkCount++;
				capturedException = ex;
				logger.LogCheckThrow(renderedFragment.ComponentId, ex);

				if (StopWaitingOnCheckException)
				{
					completion.TrySetException(
						new WaitForFailedException(
							CheckThrowErrorMessage ?? string.Empty,
							checkCount,
							renderedFragment.RenderCount,
							renderer.RenderCount,
							capturedException));
					Dispose();
					return true;
				}
			}

			return false;
		}
	}

	private static TimeSpan GetRuntimeTimeout(TimeSpan? timeout)
	{
		return Debugger.IsAttached
			? Timeout.InfiniteTimeSpan
			: timeout ?? TestContext.DefaultWaitTimeout;
	}
}
