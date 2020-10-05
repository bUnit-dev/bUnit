using System;
using System.Collections.Generic;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a planned invocation of a JavaScript function which returns nothing, with specific arguments.
	/// </summary>
	public class JSRuntimePlannedInvocation : JSRuntimePlannedInvocationBase<object>
	{
		internal JSRuntimePlannedInvocation(string identifier, Func<IReadOnlyList<object?>, bool> matcher) : base(identifier, matcher)
		{
		}

		/// <summary>
		/// Completes the current awaiting void invocation requests.
		/// </summary>
		public void SetVoidResult()
		{
			SetResultBase(default!);
		}
	}

	/// <summary>
	/// Represents a planned invocation of a JavaScript function with specific arguments.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public class JSRuntimePlannedInvocation<TResult> : JSRuntimePlannedInvocationBase<TResult>
	{
		internal JSRuntimePlannedInvocation(string identifier, Func<IReadOnlyList<object?>, bool> matcher) : base(identifier, matcher)
		{
		}

		/// <summary>
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result"></param>
		public void SetResult(TResult result) => SetResultBase(result);
	}
}
