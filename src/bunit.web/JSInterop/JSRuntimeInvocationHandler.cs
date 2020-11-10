using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bunit
{
	/// <summary>
	/// Represents a handler for an invocation of a JavaScript function with specific arguments
	/// and returns <typeparamref name="TResult"/>.
	/// </summary>
	/// <typeparam name="TResult">The expect result type.</typeparam>
	public class JSRuntimeInvocationHandler<TResult>
	{
		private readonly Func<IReadOnlyList<object?>, bool> _invocationMatcher;
		internal const string CatchAllIdentifier = "*";

		private readonly List<JSRuntimeInvocation> _invocations;

		private TaskCompletionSource<TResult> _completionSource;

		/// <summary>
		/// Gets whether this handler is set up to handle calls to <c>InvokeVoidAsync=(string, object[])</c>.
		/// </summary>
		public virtual bool IsVoidResultHandler { get; } = false;

		/// <summary>
		/// Gets whether this handler will match any invocations that expect a <typeparamref name="TResult"/> as the return type.
		/// </summary>
		public bool IsCatchAllHandler { get; }

		/// <summary>
		/// The expected identifier for the function to invoke.
		/// </summary>
		public string Identifier { get; }

		/// <summary>
		/// Gets the invocations that this <see cref="JSRuntimeInvocationHandler{TResult}"/> has matched with.
		/// </summary>
		public IReadOnlyList<JSRuntimeInvocation> Invocations => _invocations.AsReadOnly();

		/// <summary>
		/// Creates an instance of a <see cref="JSRuntimeInvocationHandler{TResult}"/>.
		/// </summary>
		internal JSRuntimeInvocationHandler(string identifier, Func<IReadOnlyList<object?>, bool> matcher)
		{
			Identifier = identifier;
			IsCatchAllHandler = identifier == CatchAllIdentifier;
			_invocationMatcher = matcher;
			_invocations = new List<JSRuntimeInvocation>();
			_completionSource = new TaskCompletionSource<TResult>();
		}

		/// <summary>
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result"></param>
		public void SetResult(TResult result)
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

		internal bool Matches(JSRuntimeInvocation invocation)
		{
			return Identifier.Equals(invocation.Identifier, StringComparison.Ordinal)
				&& _invocationMatcher(invocation.Arguments);
		}
	}

	/// <summary>
	/// Represents a handler for an invocation of a JavaScript function which returns nothing, with specific arguments.
	/// </summary>
	public class JSRuntimeInvocationHandler : JSRuntimeInvocationHandler<object>
	{
		/// <inheritdoc/>
		public override bool IsVoidResultHandler { get; } = true;

		internal JSRuntimeInvocationHandler(string identifier, Func<IReadOnlyList<object?>, bool> matcher) : base(identifier, matcher)
		{ }

		/// <summary>
		/// Completes the current awaiting void invocation requests.
		/// </summary>
		public void SetVoidResult() => SetResult(default!);
	}
}
