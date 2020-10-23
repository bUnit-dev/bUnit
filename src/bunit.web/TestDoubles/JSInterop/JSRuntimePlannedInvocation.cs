using System;
using System.Collections.Generic;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a planned invocation of a JavaScript function with specific arguments.
	/// </summary>
	/// <typeparam name="TResult">The expect result type.</typeparam>
	public class JSRuntimePlannedInvocation<TResult> : JSRuntimePlannedInvocationBase<TResult>
	{
		private readonly Func<IReadOnlyList<object?>, bool> _invocationMatcher;

		/// <summary>
		/// The expected identifier for the function to invoke.
		/// </summary>
		public string Identifier { get; }

		internal JSRuntimePlannedInvocation(string identifier, Func<IReadOnlyList<object?>, bool> matcher)
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
	/// Represents a planned invocation of a JavaScript function which returns nothing, with specific arguments.
	/// </summary>
	public class JSRuntimePlannedInvocation : JSRuntimePlannedInvocation<object>
	{
		internal JSRuntimePlannedInvocation(string identifier, Func<IReadOnlyList<object?>, bool> matcher) : base(identifier, matcher)
		{ }

		/// <summary>
		/// Completes the current awaiting void invocation requests.
		/// </summary>
		public void SetVoidResult() => SetResultBase(default!);
	}

	/// <summary>
	/// Represents any planned invocation of a JavaScript function with a specific return type.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public class JSRuntimeCatchAllPlannedInvocation<TResult> : JSRuntimePlannedInvocationBase<TResult>
	{
		internal JSRuntimeCatchAllPlannedInvocation()
		{ }

		internal override bool Matches(JSRuntimeInvocation invocation) => true;

		/// <summary>
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result"></param>
		public void SetResult(TResult result) => SetResultBase(result);
	}

	/// <summary>
	/// Represents any planned invocation of a JavaScript function which returns nothing.
	/// </summary>
	public class JSRuntimeCatchAllPlannedInvocation : JSRuntimePlannedInvocationBase<object>
	{
		internal JSRuntimeCatchAllPlannedInvocation() { }

		internal override bool Matches(JSRuntimeInvocation invocation)
		{
			return true;
		}

		/// <summary>
		/// Completes the current awaiting void invocation request.
		/// </summary>
		public void SetVoid() => SetResultBase(default!);
	}
}
