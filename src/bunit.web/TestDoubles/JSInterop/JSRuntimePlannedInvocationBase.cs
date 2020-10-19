using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a planned invocation of a JavaScript function with specific arguments.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public abstract class JSRuntimePlannedInvocationBase<TResult>
	{
		private readonly List<JSRuntimeInvocation> _invocations;

		internal Func<IReadOnlyList<object?>, bool> InvocationMatcher { get; }

		private TaskCompletionSource<TResult> _completionSource;

		/// <summary>
		/// Gets the invocations that this <see cref="JSRuntimePlannedInvocation{TResult}"/> has matched with.
		/// </summary>
		public IReadOnlyList<JSRuntimeInvocation> Invocations => _invocations.AsReadOnly();

		/// <summary>
		/// Creates an instance of a <see cref="JSRuntimePlannedInvocationBase{TResult}"/>.
		/// </summary>
		protected JSRuntimePlannedInvocationBase(Func<IReadOnlyList<object?>, bool> matcher)
		{
			_invocations = new List<JSRuntimeInvocation>();
			InvocationMatcher = matcher;
			_completionSource = new TaskCompletionSource<TResult>();
		}

		/// <summary>
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result"></param>
		protected void SetResultBase(TResult result)
		{
			if (_completionSource.Task.IsCompleted)
				_completionSource = new TaskCompletionSource<TResult>();

			_completionSource.SetResult(result);
		}

		/// <summary>
		/// Sets the <typeparamref name="TException"/> exception that invocations will receive.
		/// </summary>
		/// <param name="exception"></param>
		public void SetException<TException>(TException exception)
			where TException : Exception
		{
			if (_completionSource.Task.IsCompleted)
				_completionSource = new TaskCompletionSource<TResult>();

			_completionSource.SetException(exception);
		}

		/// <summary>
		/// Marks the <see cref="Task{TResult}"/> that invocations will receive as canceled.
		/// </summary>
		public void SetCanceled()
		{
			if (_completionSource.Task.IsCompleted)
				_completionSource = new TaskCompletionSource<TResult>();

			_completionSource.SetCanceled();
		}

		internal Task<TResult> RegisterInvocation(JSRuntimeInvocation invocation)
		{
			_invocations.Add(invocation);
			return _completionSource.Task;
		}

		internal abstract bool Matches(JSRuntimeInvocation invocation);
	}
}
