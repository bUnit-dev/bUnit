using System;
using System.Threading.Tasks;

namespace Bunit.JSInterop.InvocationHandlers
{
	/// <summary>
	/// Represents a handler for an invocation of a JavaScript function with specific arguments
	/// and returns <typeparamref name="TResult"/>.
	/// </summary>
	/// <typeparam name="TResult">The expect result type.</typeparam>
	public class JSRuntimeInvocationHandler<TResult> : JSRuntimeInvocationHandlerBase<TResult>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="JSRuntimeInvocationHandler{TResult}"/> class.
		/// </summary>
		/// <param name="identifier">Identifier it matches. Set to "*" to match all identifiers.</param>
		/// <param name="matcher">An invocation matcher used to determine if the handler should handle an invocation.</param>
		protected internal JSRuntimeInvocationHandler(string identifier, InvocationMatcher matcher)
			: base(identifier, matcher)
		{ }

		/// <summary>
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result">The result to pass to callers.</param>
		/// <returns>This handler to allow calls to be chained.</returns>
		public JSRuntimeInvocationHandler<TResult> SetResult(TResult result)
		{
			SetResultBase(result);
			return this;
		}

		/// <summary>
		/// Marks the <see cref="Task{TResult}"/> that invocations will receive as canceled.
		/// </summary>
		/// <returns>This handler to allow calls to be chained.</returns>
		public JSRuntimeInvocationHandler<TResult> SetCanceled()
		{
			SetCanceledBase();
			return this;
		}

		/// <summary>
		/// Sets the <typeparamref name="TException"/> exception that invocations will receive.
		/// </summary>
		/// <param name="exception">The exception to set.</param>
		/// <returns>This handler to allow calls to be chained.</returns>
		public JSRuntimeInvocationHandler<TResult> SetException<TException>(TException exception)
			where TException : Exception
		{
			SetExceptionBase(exception);
			return this;
		}
	}
}
