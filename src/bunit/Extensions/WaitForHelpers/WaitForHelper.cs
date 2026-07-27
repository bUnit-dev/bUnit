using System.Diagnostics;
using Bunit.Rendering;
using Microsoft.Extensions.Logging;

namespace Bunit.Extensions.WaitForHelpers;

/// <summary>
/// Represents a helper class that can wait for a render notifications from a <see cref="IRenderedComponent{TComponent}"/> type,
/// until a specific timeout is reached.
/// </summary>
internal abstract class WaitForHelper<T, TComponent> : IDisposable
	where TComponent : IComponent
{
	private readonly TaskCompletionSource<T> checkPassedCompletionSource;
	private readonly Func<ValueTask<(bool CheckPassed, T Content)>> completeChecker;
	private readonly IRenderedComponent<TComponent> renderedComponent;
	private readonly ILogger<WaitForHelper<T, TComponent>> logger;
	private readonly BunitRenderer renderer;
	private readonly Timer? timer;
	private bool isDisposed;
	private bool isChecking;
	private bool isDirty;
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
	/// Initializes a new instance of the <see cref="WaitForHelper{T, TComponent}"/> class
	/// with a synchronous check. The check is wrapped into a completed <see cref="ValueTask{TResult}"/>,
	/// so it is still evaluated synchronously (with no render interleaving) on the renderer's dispatcher.
	/// </summary>
	protected WaitForHelper(
		IRenderedComponent<TComponent> renderedComponent,
		Func<(bool CheckPassed, T Content)> completeChecker,
		TimeSpan? timeout = null)
		: this(renderedComponent, WrapSynchronousChecker(completeChecker), timeout)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="WaitForHelper{T, TComponent}"/> class.
	/// </summary>
	/// <remarks>
	/// The <paramref name="completeChecker"/> is awaited on the renderer's dispatcher after each render.
	/// A synchronous check (see the other constructor) wraps into an already-completed <see cref="ValueTask{TResult}"/>
	/// and therefore resumes synchronously without yielding the dispatcher; a genuinely asynchronous check yields the
	/// dispatcher at each <c>await</c>, so a render may interleave and the check is not guaranteed an atomic view of the DOM.
	/// </remarks>
	protected WaitForHelper(
		IRenderedComponent<TComponent> renderedComponent,
		Func<ValueTask<(bool CheckPassed, T Content)>> completeChecker,
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

		// Create the wait task and run the initial check
		// and subscribe to the OnAfterRender event.
		// This must happen before the timer is started,
		// as the check happens inside the renderers synchronization context,
		// and that may be blocked longer than the timeout on overloaded systems,
		// resulting in the timer completing before a single check has
		// has a chance to complete.
		WaitTask = CreateWaitTask();
		CheckAndInitializeWaiting();

		// If the initial check did not complete successfully,
		// start the timer and recheck after every render until the timer expires.
		if (!WaitTask.IsCompleted)
		{
			timer = new Timer(
				static (state) =>
				{
					var @this = (WaitForHelper<T, TComponent>)state!;
					@this.logger.LogWaiterTimedOut(@this.renderedComponent.ComponentId);
					@this.checkPassedCompletionSource.TrySetException(
						new WaitForFailedException(
							@this.TimeoutErrorMessage ?? string.Empty,
							@this.checkCount,
							@this.renderedComponent.RenderCount,
							@this.renderer.RenderCount,
							@this.capturedException));
				},
				this,
				GetRuntimeTimeout(timeout),
				Timeout.InfiniteTimeSpan);
		}
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
		timer?.Dispose();
		checkPassedCompletionSource.TrySetCanceled();
		renderedComponent.OnAfterRender -= OnAfterRender;
		logger.LogWaiterDisposed(renderedComponent.ComponentId);
	}

	private void CheckAndInitializeWaiting()
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
				// Before subscribing to renderedFragment.OnAfterRender,
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

		// The check may be asynchronous, so it cannot be awaited inside this synchronous
		// event handler. Kick it off without blocking the dispatcher. A synchronous check
		// completes inline (see RunCheckAsync). If a check is already in flight, mark the
		// state dirty so the running check re-evaluates once it completes, ensuring the
		// final render is never missed.
		if (isChecking)
		{
			isDirty = true;
			return;
		}

		isChecking = true;
		_ = RunCheckAsync();
	}

	private async Task RunCheckAsync()
	{
		// Runs on the renderer's dispatcher. A synchronous check wraps into an already-completed
		// ValueTask, so the await below resumes inline without yielding - preserving the atomic,
		// no-render-interleaving behaviour synchronous checks rely on. A genuinely asynchronous
		// check yields the dispatcher; the default await captures and resumes on the dispatcher's
		// synchronization context, so isChecking/isDirty are only ever touched on the single
		// dispatcher thread and need no locking.
		try
		{
			do
			{
				isDirty = false;

				if (isDisposed || WaitTask.IsCompleted)
					return;

				try
				{
					logger.LogCheckingWaitCondition(renderedComponent.ComponentId);

					var checkResult = await completeChecker();
					checkCount++;

					if (isDisposed || WaitTask.IsCompleted)
						return;

					if (checkResult.CheckPassed)
					{
						checkPassedCompletionSource.TrySetResult(checkResult.Content);
						logger.LogCheckCompleted(renderedComponent.ComponentId);
						Dispose();
						return;
					}

					logger.LogCheckFailed(renderedComponent.ComponentId);
				}
				catch (Exception ex)
				{
					checkCount++;
					capturedException = ex;
					logger.LogCheckThrow(renderedComponent.ComponentId, ex);

					if (StopWaitingOnCheckException)
					{
						if (!isDisposed && !WaitTask.IsCompleted)
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

						return;
					}
				}
			}
			while (isDirty && !isDisposed && !WaitTask.IsCompleted);
		}
		finally
		{
			isChecking = false;
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

	private static Func<ValueTask<(bool CheckPassed, T Content)>> WrapSynchronousChecker(Func<(bool CheckPassed, T Content)> completeChecker)
	{
		ArgumentNullException.ThrowIfNull(completeChecker);

		// Wrap the synchronous check in an already-completed ValueTask so the single check
		// pipeline can await it. Awaiting a completed ValueTask resumes inline, so the check
		// still runs synchronously on the dispatcher with no render interleaving.
		return () => new ValueTask<(bool CheckPassed, T Content)>(completeChecker());
	}
}
