using System.Diagnostics;
using Bunit.Rendering;
using Microsoft.Extensions.Logging;

namespace Bunit.Extensions.WaitForHelpers;

/// <summary>
/// Represents a helper class that can wait for a render notifications from a <see cref="IRenderedFragment"/> type,
/// until a specific timeout is reached.
/// </summary>
public abstract class WaitForHelper<T> : IDisposable
{
	private readonly CancellationTokenSource cts;
	private readonly TaskCompletionSource<T> checkPassedCompletionSource;
	private readonly Func<(bool CheckPassed, T Content)> completeChecker;
	private readonly IRenderedFragment renderedFragment;
	private readonly ILogger<WaitForHelper<T>> logger;
#pragma warning disable CA2213 // Handled by the container
	private readonly BunitRenderer renderer;
#pragma warning restore CA2213
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
		IRenderedFragment renderedFragment,
		Func<(bool CheckPassed, T Content)> completeChecker,
		TimeSpan? timeout = null)
	{
		this.renderedFragment = renderedFragment ?? throw new ArgumentNullException(nameof(renderedFragment));
		this.completeChecker = completeChecker ?? throw new ArgumentNullException(nameof(completeChecker));

		logger = renderedFragment.Services.CreateLogger<WaitForHelper<T>>();
		renderer = renderedFragment
			.Services
			.GetRequiredService<BunitRenderer>();
		checkPassedCompletionSource = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
		cts = new CancellationTokenSource(GetRuntimeTimeout(timeout));
		cts.Token.Register(() =>
		{
			logger.LogWaiterTimedOut(renderedFragment.ComponentId);
			checkPassedCompletionSource.TrySetException(
				new WaitForFailedException(
					TimeoutErrorMessage ?? string.Empty,
					checkCount,
					renderedFragment.RenderCount,
					renderer.RenderCount,
					capturedException));
		});
		WaitTask = CreateWaitTask();

		InitializeWaiting();
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

		isDisposed = true;
		cts.Dispose();
		checkPassedCompletionSource.TrySetCanceled();
		renderedFragment.OnAfterRender -= OnAfterRender;
		logger.LogWaiterDisposed(renderedFragment.ComponentId);
	}

	private void InitializeWaiting()
	{
		if (!WaitTask.IsCompleted)
		{
			renderedFragment.OnAfterRender += OnAfterRender;
			OnAfterRender(this, EventArgs.Empty);
		}
	}

	private Task<T> CreateWaitTask()
	{
		// Two to failure conditions, that the renderer captures an unhandled
		// exception from a component or itself, or that the timeout is reached,
		// are executed on the renderers scheduler, to ensure that OnAfterRender
		// and the continuations does not happen at the same time.
		var failureTask = renderer.Dispatcher.InvokeAsync(async () =>
		{
			var exception = await renderer.UnhandledException;
			return Task.FromException<T>(exception);
		}).Unwrap();

		return Task
			.WhenAny(checkPassedCompletionSource.Task, failureTask)
			.Unwrap();
	}

	private void OnAfterRender(object? sender, EventArgs args)
	{
		if (isDisposed || WaitTask.IsCompleted)
			return;

		try
		{
			logger.LogCheckingWaitCondition(renderedFragment.ComponentId);

			var checkResult = completeChecker();
			checkCount++;
			if (checkResult.CheckPassed)
			{
				checkPassedCompletionSource.TrySetResult(checkResult.Content);
				logger.LogCheckCompleted(renderedFragment.ComponentId);
				Dispose();
			}
			else
			{
				renderer.AllowOneRenderCycle();
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
				checkPassedCompletionSource.TrySetException(
					new WaitForFailedException(
						CheckThrowErrorMessage ?? string.Empty,
						checkCount,
						renderedFragment.RenderCount,
						renderer.RenderCount,
						capturedException));
				Dispose();
			}

			renderer.AllowOneRenderCycle();
		}
	}

	private static TimeSpan GetRuntimeTimeout(TimeSpan? timeout)
	{
		return Debugger.IsAttached
			? Timeout.InfiniteTimeSpan
			: timeout ?? TestContext.DefaultWaitTimeout;
	}
}
