using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Bunit.Extensions.WaitForHelpers
{
	/// <summary>
	/// Represents a helper class that can wait for a render notifications from a <see cref="IRenderedFragmentBase"/> type,
	/// until a specific timeout is reached.
	/// </summary>
	public abstract class WaitForHelper : IDisposable
	{
		private readonly Timer _timer;
		private readonly TaskCompletionSource<object?> _completionSouce;
		private readonly Func<bool> _completeChecker;
		private readonly IRenderedFragmentBase _renderedFragment;
		private readonly ILogger _logger;
		private bool _isDisposed;
		private Exception? _capturedException;

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
		/// Gets whether to continue waiting if the wait condition checker throws.
		/// </summary>
		protected abstract bool StopWaitingOnCheckException { get; }

		/// <summary>
		/// Gets the task that will complete successfully if the check passed before the timeout was reached.
		/// The task will complete with an <see cref="WaitForFailedException"/> exception if the timeout was reached without the check passing.
		/// </summary>
		public Task WaitTask => _completionSouce.Task;

		/// <summary>
		/// Creates an instance of the <see cref="WaitForHelper"/> type.
		/// </summary>
		protected WaitForHelper(IRenderedFragmentBase renderedFragment, Func<bool> completeChecker, TimeSpan? timeout = null)
		{
			_renderedFragment = renderedFragment ?? throw new ArgumentNullException(nameof(renderedFragment));
			_completeChecker = completeChecker ?? throw new ArgumentNullException(nameof(completeChecker));
			_logger = renderedFragment.Services.CreateLogger<WaitForHelper>();
			_completionSouce = new TaskCompletionSource<object?>();
			_timer = new Timer(OnTimeout, this, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);

			_renderedFragment.OnAfterRender += OnAfterRender;
			OnAfterRender();
			StartTimer(timeout);
		}

		private void StartTimer(TimeSpan? timeout)
		{
			if (_isDisposed)
				return;

			lock (_completionSouce)
			{
				if (_isDisposed)
					return;

				_timer.Change(GetRuntimeTimeout(timeout), Timeout.InfiniteTimeSpan);
			}
		}

		private void OnAfterRender()
		{
			if (_isDisposed)
				return;

			lock (_completionSouce)
			{
				if (_isDisposed)
					return;

				try
				{
					_logger.LogDebug(new EventId(1, nameof(OnAfterRender)), $"Checking the wait condition for component {_renderedFragment.ComponentId}");
					if (_completeChecker())
					{
						_completionSouce.TrySetResult(null);
						_logger.LogDebug(new EventId(2, nameof(OnAfterRender)), $"The check completed successfully for component {_renderedFragment.ComponentId}");
						Dispose();
					}
					else
						_logger.LogDebug(new EventId(3, nameof(OnAfterRender)), $"The check failed for component {_renderedFragment.ComponentId}");
				}
				catch (Exception ex)
				{
					_capturedException = ex;
					_logger.LogDebug(new EventId(4, nameof(OnAfterRender)), $"The checker of component {_renderedFragment.ComponentId} throw an exception with message '{ex.Message}'");

					if (StopWaitingOnCheckException)
					{
						_completionSouce.TrySetException(new WaitForFailedException(CheckThrowErrorMessage, _capturedException));
						Dispose();
					}
				}
			}
		}

		private void OnTimeout(object? state)
		{
			if (_isDisposed)
				return;

			lock (_completionSouce)
			{
				if (_isDisposed)
					return;

				_logger.LogDebug(new EventId(5, nameof(OnTimeout)), $"The wait for helper for component {_renderedFragment.ComponentId} timed out");

				_completionSouce.TrySetException(new WaitForFailedException(TimeoutErrorMessage, _capturedException));

				Dispose();
			}
		}

		/// <summary>
		/// Disposes the wait helper and sets the <see cref="WaitTask"/> to canceled, if it is not
		/// already in one of the other completed states.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes of the wait task and related logic.
		/// </summary>
		/// <remarks>
		/// The disposing parameter should be false when called from a finalizer, and true when called from the
		/// <see cref="Dispose()"/> method. In other words, it is true when deterministically called and false when non-deterministically called.
		/// </remarks>
		/// <param name="disposing">Set to true if called from <see cref="Dispose()"/>, false if called from a finalizer.f</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed || !disposing)
				return;

			lock (_completionSouce)
			{
				if (_isDisposed)
					return;

				_isDisposed = true;
				_renderedFragment.OnAfterRender -= OnAfterRender;
				_timer.Dispose();
				_completionSouce.TrySetCanceled();
				_logger.LogDebug(new EventId(6, nameof(Dispose)), $"The state wait helper for component {_renderedFragment.ComponentId} disposed");
			}
		}

		private static TimeSpan GetRuntimeTimeout(TimeSpan? timeout)
		{
			return timeout ?? TimeSpan.FromSeconds(1);
		}
	}
}
