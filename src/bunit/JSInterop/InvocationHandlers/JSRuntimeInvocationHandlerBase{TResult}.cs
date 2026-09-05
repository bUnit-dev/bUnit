using System.Collections.Concurrent;

namespace Bunit;

// Invocation tracking mirrors ASP.NET Core's JSRuntime: no per-invocation state lives in instance
// fields. Each call gets its own TaskCompletionSource in a ConcurrentDictionary keyed by an
// Interlocked id, and the timeout closes over that entry alone, so an elapsing timeout can never
// race a concurrently set result. See https://github.com/dotnet/aspnetcore/blob/main/src/JSInterop/Microsoft.JSInterop/src/JSRuntime.cs
/// <summary>
/// Represents an invocation handler for <see cref="JSRuntimeInvocation"/> instances.
/// </summary>
public abstract class JSRuntimeInvocationHandlerBase<TResult> : IDisposable
{
	private readonly InvocationMatcher invocationMatcher;
	private readonly ConcurrentDictionary<long, PendingInvocation> pendingInvocations = new();
	private long nextInvocationId;
	private Task<TResult>? outcome;
	private bool disposed;
	private BunitJSInterop? owner;

	/// <summary>
	/// Gets a value indicating whether this handler is set up to handle calls to <c>InvokeVoidAsync(string, object[])</c>.
	/// </summary>
	public virtual bool IsVoidResultHandler { get; }

	/// <summary>
	/// Gets a value indicating whether this handler is considered a catch all handler for invocations with <typeparamref name="TResult"/> as the return type.
	/// </summary>
	public bool IsCatchAllHandler { get; }

	/// <summary>
	/// Gets the invocations that this <see cref="JSRuntimeInvocationHandler{TResult}"/> has matched with.
	/// </summary>
	public JSRuntimeInvocationDictionary Invocations { get; } = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="JSRuntimeInvocationHandlerBase{TResult}"/> class.
	/// </summary>
	/// <param name="matcher">An invocation matcher used to determine if the handler should handle an invocation.</param>
	/// <param name="isCatchAllHandler">Set to true if this handler is a catch all handler, that should only be used if there are no other non-catch all handlers available.</param>
	protected JSRuntimeInvocationHandlerBase(InvocationMatcher matcher, bool isCatchAllHandler)
	{
		invocationMatcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
		IsCatchAllHandler = isCatchAllHandler;
	}

	/// <summary>
	/// Marks the <see cref="Task{TResult}"/> that invocations will receive as canceled.
	/// </summary>
	protected void SetCanceledBase()
		=> CompleteAll(Task.FromCanceled<TResult>(new CancellationToken(canceled: true)));

	/// <summary>
	/// Sets the <typeparamref name="TException"/> exception that invocations will receive.
	/// </summary>
	/// <param name="exception">The type of exception to pass to the callers.</param>
	protected void SetExceptionBase<TException>(TException exception)
		where TException : Exception
		=> CompleteAll(Task.FromException<TResult>(exception));

	/// <summary>
	/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
	/// </summary>
	/// <param name="result">The type of result to pass to the callers.</param>
	protected void SetResultBase(TResult result)
		=> CompleteAll(Task.FromResult(result));

	/// <summary>
	/// Call this to have the this handler handle the <paramref name="invocation"/>.
	/// </summary>
	/// <remarks>
	/// Note to implementors: Always call the <see cref="JSRuntimeInvocationHandlerBase{TResult}.HandleAsync(JSRuntimeInvocation)"/>
	/// method when overriding it in a sub class. It will make sure the invocation is correctly registered in the <see cref="Invocations"/> dictionary.
	/// </remarks>
	/// <param name="invocation">Invocation to handle.</param>
	protected internal virtual Task<TResult> HandleAsync(JSRuntimeInvocation invocation)
	{
		Invocations.RegisterInvocation(invocation);

		if (Volatile.Read(ref outcome) is { } configured)
			return configured;

		var timeout = DefaultWaitTimeout;
		if (timeout <= TimeSpan.Zero)
		{
			throw new JSRuntimeInvocationNotSetException(invocation);
		}

		var id = Interlocked.Increment(ref nextInvocationId);
		var pending = new PendingInvocation(id, invocation);
		pendingInvocations[id] = pending;

		if (Volatile.Read(ref outcome) is { } raced && pendingInvocations.TryRemove(id, out _))
		{
			Transfer(raced, pending.CompletionSource);
		}
		else
		{
			pending.StartTimeout(OnTimeoutElapsed, timeout);
		}

		return pending.CompletionSource.Task;
	}

	/// <summary>
	/// Checks whether this invocation handler can handle the <paramref name="invocation"/>.
	/// </summary>
	/// <param name="invocation">Invocation to check.</param>
	/// <returns>True if the handler can handle the invocation, false otherwise.</returns>
	internal bool CanHandle(JSRuntimeInvocation invocation) => invocationMatcher(invocation);

	/// <summary>
	/// Attaches this handler to its owning <see cref="BunitJSInterop"/>.
	/// </summary>
	internal void AttachTo(BunitJSInterop jsInterop) => owner = jsInterop;

	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	/// <inheritdoc/>
	protected virtual void Dispose(bool disposing)
	{
		if (!disposed && disposing)
		{
			foreach (var id in pendingInvocations.Keys)
			{
				if (pendingInvocations.TryRemove(id, out var pending))
					pending.Dispose();
			}

			disposed = true;
		}
	}

	private void CompleteAll(Task<TResult> next)
	{
		Volatile.Write(ref outcome, next);

		foreach (var id in pendingInvocations.Keys)
		{
			if (pendingInvocations.TryRemove(id, out var pending))
			{
				pending.Dispose();
				Transfer(next, pending.CompletionSource);
			}
		}
	}

	private TimeSpan DefaultWaitTimeout => owner?.DefaultWaitTimeout ?? TimeSpan.FromSeconds(1);

	private void OnTimeoutElapsed(object? state)
	{
		if (state is not PendingInvocation pending || !pendingInvocations.TryRemove(pending.Id, out _))
			return;

		pending.Dispose();
		pending.CompletionSource.TrySetException(new JSRuntimeInvocationNotSetException(pending.Invocation));
	}

	private static void Transfer(Task<TResult> from, TaskCompletionSource<TResult> to)
	{
		if (from.IsCanceled)
			to.TrySetCanceled();
		else if (from.Exception is { } exception)
			to.TrySetException(exception.InnerExceptions);
		else
			to.TrySetResult(from.Result);
	}

	private sealed class PendingInvocation : IDisposable
	{
		private Timer? timeoutTimer;

		public long Id { get; }

		public JSRuntimeInvocation Invocation { get; }

		public TaskCompletionSource<TResult> CompletionSource { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

		public PendingInvocation(long id, JSRuntimeInvocation invocation)
		{
			Id = id;
			Invocation = invocation;
		}

		public void StartTimeout(TimerCallback callback, TimeSpan timeout)
		{
			timeoutTimer = new Timer(callback, this, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
			timeoutTimer.Change(timeout, Timeout.InfiniteTimeSpan);
		}

		public void Dispose() => timeoutTimer?.Dispose();
	}
}
