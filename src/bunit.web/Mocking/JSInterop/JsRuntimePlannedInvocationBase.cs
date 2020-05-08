using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bunit.Mocking.JSInterop
{
	/// <summary>
	/// Represents a planned invocation of a JavaScript function with specific arguments.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public abstract class JsRuntimePlannedInvocationBase<TResult>
	{
		private readonly List<JsRuntimeInvocation> _invocations;

		private Func<IReadOnlyList<object>, bool> InvocationMatcher { get; }

		private TaskCompletionSource<TResult> _completionSource;

		/// <summary>
		/// The expected identifier for the function to invoke.
		/// </summary>
		public string Identifier { get; }

		/// <summary>
		/// Gets the invocations that this <see cref="JsRuntimePlannedInvocation{TResult}"/> has matched with.
		/// </summary>
		public IReadOnlyList<JsRuntimeInvocation> Invocations => _invocations.AsReadOnly();

		/// <summary>
		/// Creates an instance of a <see cref="JsRuntimePlannedInvocationBase{TResult}"/>.
		/// </summary>
		protected JsRuntimePlannedInvocationBase(string identifier, Func<IReadOnlyList<object>, bool> matcher)
		{
			Identifier = identifier;
			_invocations = new List<JsRuntimeInvocation>();
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

		internal Task<TResult> RegisterInvocation(JsRuntimeInvocation invocation)
		{
			_invocations.Add(invocation);
			return _completionSource.Task;
		}

		internal bool Matches(JsRuntimeInvocation invocation)
		{
			return Identifier.Equals(invocation.Identifier, StringComparison.Ordinal)
				&& InvocationMatcher(invocation.Arguments);
		}
	}
}
