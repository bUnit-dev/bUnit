using System;
using System.Collections.Generic;

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
		public void SetResult(TResult result) => SetResultBase(result);
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
		public void SetVoidResult() => SetResultBase(default!);
	}
}
