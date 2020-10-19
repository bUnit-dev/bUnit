using System;
using System.Collections.Generic;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a planned invocation of an identified JavaScript function with specific arguments.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public abstract class JSRuntimePlannedInvocationWithIdentifierBase<TResult> : JSRuntimePlannedInvocationBase<TResult>
	{

		/// <summary>
		/// The expected identifier for the function to invoke.
		/// </summary>
		public string Identifier { get; }

		internal JSRuntimePlannedInvocationWithIdentifierBase(string identifier, Func<IReadOnlyList<object?>, bool> matcher) : base(matcher)
		{
			Identifier = identifier;
		}

		internal override bool Matches(JSRuntimeInvocation invocation)
		{
			return Identifier.Equals(invocation.Identifier, StringComparison.Ordinal)
				&& InvocationMatcher(invocation.Arguments);
		}
	}


	/// <summary>
	/// Represents a planned invocation of a JavaScript function which returns nothing, with specific arguments.
	/// </summary>
	public class JSRuntimePlannedInvocation : JSRuntimePlannedInvocationWithIdentifierBase<object>
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
	public class JSRuntimePlannedInvocation<TResult> : JSRuntimePlannedInvocationWithIdentifierBase<TResult>
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

	/// <summary>
	/// Represents any planned invocation of a JavaScript function with a specific return type.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public class JSRuntimeCatchAllPlannedInvocation<TResult> : JSRuntimePlannedInvocationBase<TResult>
	{
		internal JSRuntimeCatchAllPlannedInvocation() : base((args) => true)
		{
		}

		internal override bool Matches(JSRuntimeInvocation invocation)
		{
			return true;
		}

		/// <summary>
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result"></param>
		public void SetResult(TResult result) => SetResultBase(result);
	}
}
