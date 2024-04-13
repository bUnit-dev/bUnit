using System.Diagnostics;
using Bunit.Rendering;
using Microsoft.Extensions.Logging;

namespace Bunit.Extensions.WaitForHelpers;

/// <summary>
/// Represents a helper class that can wait for a render notifications from a <see cref="IRenderedComponent{TComponent}"/> type,
/// until a specific timeout is reached.
/// </summary>
public abstract class WaitForHelper<T, TComponent> : IDisposable
	where TComponent : IComponent
{
	private readonly Timer timer;
	private readonly TaskCompletionSource<T> checkPassedCompletionSource;
	private readonly Func<(bool CheckPassed, T Content)> completeChecker;
	private readonly IRenderedComponent<TComponent> renderedComponent;
	private readonly ILogger<WaitForHelper<T, TComponent>> logger;
	private readonly BunitRenderer renderer;
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
	/// Initializes a new instance of the <see cref="WaitForHelper{T, TComponent}"/> class.
	/// </summary>
	protected WaitForHelper(
		IRenderedComponent<TComponent> renderedComponent,
		Func<(bool CheckPassed, T Content)> completeChecker,
		TimeSpan? timeout = null)
	{
		this.renderedComponent = renderedComponent ?? throw new ArgumentNullException(nameof(renderedComponent));
		this.completeChecker = completeChecker ?? throw new ArgumentNullException(nameof(completeChecker));

		logger = renderedComponent.Services.CreateLogger<WaitForHelper<T, TComponent>>();
		renderer = renderedComponent
			.Services
			.GetRequiredService<BunitContext>()
			.Renderer;
		checkPassedCompletionSource = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
		timer = new Timer(_ =>
		{
			logger.LogWaiterTimedOut(renderedComponent.ComponentId);
			checkPassedCompletionSource.TrySetException(
				new WaitForFailedException(
					TimeoutErrorMessage ?? string.Empty,
					checkCount,
					renderedComponent.RenderCount,
					renderer.RenderCount,
					capturedException));
		});
		WaitTask = CreateWaitTask();
		timer.Change(GetRuntimeTimeout(timeout), Timeout.InfiniteTimeSpan);

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
		timer.Dispose();
		checkPassedCompletionSource.TrySetCanceled();
		renderedComponent.OnAfterRender -= OnAfterRender;
		logger.LogWaiterDisposed(renderedComponent.ComponentId);
	}

	private void InitializeWaiting()
	{
		if (!WaitTask.IsCompleted)
		{
			// Subscribe inside the renderers synchronization context
			// to ensure no renders happens between the
			// initial OnAfterRender and subscribing.
			// This also ensures that checks performed during OnAfterRender,
			// which are usually not atomic, e.g. search the DOM tree,
			// can be performed without the DOM tree changing.
			renderedComponent.InvokeAsync(() =>
			{
				// Before subscribing to renderedComponent.OnAfterRender,
				// we need to make sure that the desired state has not already been reached.
				OnAfterRender(this, EventArgs.Empty);
				SubscribeToOnAfterRender();
			});
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
			logger.LogCheckingWaitCondition(renderedComponent.ComponentId);

			var checkResult = completeChecker();
			checkCount++;
			if (checkResult.CheckPassed)
			{
				checkPassedCompletionSource.TrySetResult(checkResult.Content);
				logger.LogCheckCompleted(renderedComponent.ComponentId);
				Dispose();
			}
			else
			{
				logger.LogCheckFailed(renderedComponent.ComponentId);
			}
		}
		catch (Exception ex)
		{
			checkCount++;
			capturedException = ex;
			logger.LogCheckThrow(renderedComponent.ComponentId, ex);

			if (StopWaitingOnCheckException)
			{
				checkPassedCompletionSource.TrySetException(
					new WaitForFailedException(
						CheckThrowErrorMessage ?? string.Empty,
						checkCount,
						renderedComponent.RenderCount,
						renderer.RenderCount,
						capturedException));
				Dispose();
			}
		}
	}

	private void SubscribeToOnAfterRender()
	{
		// There might not be a need to subscribe if the WaitTask has already
		// been completed, perhaps due to an unhandled exception from the
		// renderer or from the initial check by the checker.
		if (!isDisposed && !WaitTask.IsCompleted)
			renderedComponent.OnAfterRender += OnAfterRender;
	}

	private static TimeSpan GetRuntimeTimeout(TimeSpan? timeout)
	{
		return Debugger.IsAttached
			? Timeout.InfiniteTimeSpan
			: timeout ?? BunitContext.DefaultWaitTimeout;
	}
}
