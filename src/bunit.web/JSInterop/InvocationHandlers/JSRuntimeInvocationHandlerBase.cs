using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bunit.JSInterop.InvocationHandlers
{
	/// <summary>
	/// Represents an invocation handler for <see cref="JSRuntimeInvocation"/> instances.
	/// </summary>
	public abstract class JSRuntimeInvocationHandlerBase<TResult>
	{
		/// <summary>
		/// The identifier string used to indicate a catch all handler.
		/// </summary>
		protected internal const string CatchAllIdentifier = "*";

		private readonly InvocationMatcher _invocationMatcher;
		private TaskCompletionSource<TResult> _completionSource;

		/// <summary>
		/// Gets whether this handler is set up to handle calls to <c>InvokeVoidAsync(string, object[])</c>.
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
		public JSRuntimeInvocationDictionary Invocations { get; } = new();

		/// <summary>
		/// Creates an instance of the <see cref="JSRuntimeInvocationHandlerBase{TResult}"/>.
		/// </summary>
		/// <param name="identifier">Identifier it matches. Set to "*" to match all identifiers.</param>
		/// <param name="matcher"></param>
		protected JSRuntimeInvocationHandlerBase(string identifier, InvocationMatcher matcher)
		{
			Identifier = identifier;
			IsCatchAllHandler = identifier == CatchAllIdentifier;
			_invocationMatcher = matcher;
			_completionSource = new TaskCompletionSource<TResult>();
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
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result"></param>
		public void SetResultBase(TResult result)
		{
			if (_completionSource.Task.IsCompleted)
				_completionSource = new TaskCompletionSource<TResult>();

			_completionSource.SetResult(result);
		}

		/// <summary>
		/// Call this to have the this handler handle the <paramref name="invocation"/>.
		/// </summary>
		/// <remarks>
		/// Note to implementors: Always call the <see cref="JSRuntimeInvocationHandlerBase{TResult}.Handle(JSRuntimeInvocation)"/>
		/// method when overriding it in a sub class. It will make sure the invocation is correctly registered in the <see cref="Invocations"/> dictionary.
		/// </remarks>
		/// <param name="invocation">Invocation to handle.</param>
		protected internal virtual Task<TResult> Handle(JSRuntimeInvocation invocation)
		{
			Invocations.RegisterInvocation(invocation);
			return _completionSource.Task;
		}

		/// <summary>
		/// Checks whether this invocation handler can handle the <paramref name="invocation"/>.
		/// </summary>
		/// <param name="invocation">Invocation to check.</param>
		/// <returns>True if the handler can handle the invocation, false otherwise.</returns>
		internal bool CanHandle(JSRuntimeInvocation invocation) => (IsCatchAllHandler || MatchesIdentifier(invocation)) && _invocationMatcher(invocation);

		private bool MatchesIdentifier(JSRuntimeInvocation invocation) => Identifier.Equals(invocation.Identifier, StringComparison.Ordinal);
	}
}
