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
		/// Creates an instance of a <see cref="JSRuntimeInvocationHandler{TResult}"/> type.
		/// </summary>
		protected internal JSRuntimeInvocationHandler(string identifier, InvocationMatcher matcher) : base(identifier, matcher) { }

		/// <summary>
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result"></param>
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

	/// <summary>
	/// Represents a handler for an invocation of a JavaScript function which returns nothing, with specific arguments.
	/// </summary>
	public class JSRuntimeInvocationHandler : JSRuntimeInvocationHandlerBase<object>
	{
		/// <inheritdoc/>
		public override sealed bool IsVoidResultHandler { get; } = true;

		/// <summary>
		/// Creates an instance of a <see cref="JSRuntimeInvocationHandler"/> type.
		/// </summary>
		protected internal JSRuntimeInvocationHandler(string identifier, InvocationMatcher matcher) : base(identifier, matcher) { }

		/// <summary>
		/// Completes the current awaiting void invocation requests.
		/// </summary>
		/// <returns>This handler to allow calls to be chained.</returns>
		public JSRuntimeInvocationHandler SetVoidResult()
		{
			SetResultBase(default!);
			return this;
		}

		/// <summary>
		/// Marks the <see cref="Task"/> that invocations will receive as canceled.
		/// </summary>
		/// <returns>This handler to allow calls to be chained.</returns>
		public JSRuntimeInvocationHandler SetCanceled()
		{
			SetCanceledBase();
			return this;
		}

		/// <summary>
		/// Sets the <typeparamref name="TException"/> exception that invocations will receive.
		/// </summary>
		/// <param name="exception">The exception to set.</param>
		/// <returns>This handler to allow calls to be chained.</returns>
		public JSRuntimeInvocationHandler SetException<TException>(TException exception)
			where TException : Exception
		{
			SetExceptionBase(exception);
			return this;
		}
	}
}
