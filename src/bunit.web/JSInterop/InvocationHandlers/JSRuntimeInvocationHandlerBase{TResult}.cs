using System;
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

		private readonly InvocationMatcher invocationMatcher;
		private TaskCompletionSource<TResult> completionSource;

		/// <summary>
		/// Gets a value indicating whether this handler is set up to handle calls to <c>InvokeVoidAsync(string, object[])</c>.
		/// </summary>
		public virtual bool IsVoidResultHandler { get; }

		/// <summary>
		/// Gets a value indicating whether this handler will match any invocations that expect a <typeparamref name="TResult"/> as the return type.
		/// </summary>
		public bool IsCatchAllHandler { get; }

		/// <summary>
		/// Gets the expected identifier for the function to invoke.
		/// </summary>
		public string Identifier { get; }

		/// <summary>
		/// Gets the invocations that this <see cref="JSRuntimeInvocationHandler{TResult}"/> has matched with.
		/// </summary>
		public JSRuntimeInvocationDictionary Invocations { get; } = new();

		/// <summary>
		/// Initializes a new instance of the <see cref="JSRuntimeInvocationHandlerBase{TResult}"/> class.
		/// </summary>
		/// <param name="identifier">Identifier it matches. Set to "*" to match all identifiers.</param>
		/// <param name="matcher">An invocation matcher used to determine if the handler should handle an invocation.</param>
		protected JSRuntimeInvocationHandlerBase(string identifier, InvocationMatcher matcher)
		{
			if (string.IsNullOrWhiteSpace(identifier))
				throw new ArgumentException($"'{nameof(identifier)}' cannot be null or whitespace", nameof(identifier));

			Identifier = identifier;
			IsCatchAllHandler = string.Equals(identifier, CatchAllIdentifier, StringComparison.Ordinal);
			invocationMatcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
			completionSource = new TaskCompletionSource<TResult>();
		}

		/// <summary>
		/// Marks the <see cref="Task{TResult}"/> that invocations will receive as canceled.
		/// </summary>
		protected void SetCanceledBase()
		{
			if (completionSource.Task.IsCompleted)
				completionSource = new TaskCompletionSource<TResult>();

			completionSource.SetCanceled();
		}

		/// <summary>
		/// Sets the <typeparamref name="TException"/> exception that invocations will receive.
		/// </summary>
		/// <param name="exception">The type of exception to pass to the callers.</param>
		protected void SetExceptionBase<TException>(TException exception)
			where TException : Exception
		{
			if (completionSource.Task.IsCompleted)
				completionSource = new TaskCompletionSource<TResult>();

			completionSource.SetException(exception);
		}

		/// <summary>
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result">The type of result to pass to the callers.</param>
		protected void SetResultBase(TResult result)
		{
			if (completionSource.Task.IsCompleted)
				completionSource = new TaskCompletionSource<TResult>();

			completionSource.SetResult(result);
		}

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
			return completionSource.Task;
		}

		/// <summary>
		/// Checks whether this invocation handler can handle the <paramref name="invocation"/>.
		/// </summary>
		/// <param name="invocation">Invocation to check.</param>
		/// <returns>True if the handler can handle the invocation, false otherwise.</returns>
		internal bool CanHandle(JSRuntimeInvocation invocation) => (IsCatchAllHandler || MatchesIdentifier(invocation)) && invocationMatcher(invocation);

		private bool MatchesIdentifier(JSRuntimeInvocation invocation) => Identifier.Equals(invocation.Identifier, StringComparison.Ordinal);
	}
}
