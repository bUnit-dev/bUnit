using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Bunit;

namespace Bunit.TestDoubles.JSInterop
{
	/// <summary>
	/// Exception use to indicate that an unplanned invocation was
	/// received by the <see cref="MockJSRuntimeInvokeHandler"/> running in <see cref="JSRuntimeMockMode.Strict"/>.
	/// </summary>
	[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
	public class UnplannedJSInvocationException : Exception
	{
		/// <summary>
		/// Gets the unplanned invocation.
		/// </summary>
		public JSRuntimeInvocation Invocation { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="UnplannedJSInvocationException"/>
		/// with the provided <see cref="Invocation"/> attached.
		/// </summary>
		/// <param name="invocation">The unplanned invocation.</param>
		public UnplannedJSInvocationException(JSRuntimeInvocation invocation)
			: base($"The invocation of '{invocation.Identifier}' {PrintArguments(invocation.Arguments)} was not expected.")
		{
			Invocation = invocation;
		}

		private static string PrintArguments(IReadOnlyList<object> arguments)
		{
			if (arguments.Count == 0)
				return "without arguments";
			else if (arguments.Count == 1)
				return $"with the argument [{arguments[0].ToString()}]";
			else
				return $"with arguments [{string.Join(", ", arguments.Select(x => x.ToString()))}]";
		}
	}
}
