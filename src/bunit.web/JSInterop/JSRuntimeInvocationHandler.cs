using System;
using System.Collections.Generic;

namespace Bunit
{
	/// <summary>
	/// Represents a handler for an invocation of a JavaScript function with specific arguments
	/// and returns <typeparamref name="TResult"/>.
	/// </summary>
	/// <typeparam name="TResult">The expect result type.</typeparam>
	public class JSRuntimeInvocationHandler<TResult> : JSRuntimeInvocationHandlerBase<TResult>
	{
		private readonly Func<IReadOnlyList<object?>, bool> _invocationMatcher;

		/// <summary>
		/// The expected identifier for the function to invoke.
		/// </summary>
		public string Identifier { get; }

		internal JSRuntimeInvocationHandler(string identifier, Func<IReadOnlyList<object?>, bool> matcher)
		{
			Identifier = identifier;
			_invocationMatcher = matcher;
		}

		/// <summary>
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result"></param>
		public void SetResult(TResult result) => SetResultBase(result);

		internal override bool Matches(JSRuntimeInvocation invocation)
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
		internal JSRuntimeInvocationHandler(string identifier, Func<IReadOnlyList<object?>, bool> matcher) : base(identifier, matcher)
		{ }

		/// <summary>
		/// Completes the current awaiting void invocation requests.
		/// </summary>
		public void SetVoidResult() => SetResultBase(default!);
	}
}
