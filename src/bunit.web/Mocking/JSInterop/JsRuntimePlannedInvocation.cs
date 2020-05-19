using System;
using System.Collections.Generic;

namespace Bunit.Mocking.JSInterop
{
	/// <summary>
	/// Represents a planned invocation of a JavaScript function which returns nothing, with specific arguments.
	/// </summary>
	public class JsRuntimePlannedInvocation : JsRuntimePlannedInvocationBase<object>
	{
		internal JsRuntimePlannedInvocation(string identifier, Func<IReadOnlyList<object>, bool> matcher) : base(identifier, matcher)
		{
		}

		/// <summary>
		/// Completes the current awaiting void invocation requests.
		/// </summary>
		public void SetVoidResult()
		{
			base.SetResultBase(default!);
		}
	}

	/// <summary>
	/// Represents a planned invocation of a JavaScript function with specific arguments.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public class JsRuntimePlannedInvocation<TResult> : JsRuntimePlannedInvocationBase<TResult>
	{
		internal JsRuntimePlannedInvocation(string identifier, Func<IReadOnlyList<object>, bool> matcher) : base(identifier, matcher)
		{
		}

		/// <summary>
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result"></param>
		public void SetResult(TResult result) => base.SetResultBase(result);
	}
}
