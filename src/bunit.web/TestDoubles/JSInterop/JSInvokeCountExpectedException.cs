using System;
using Bunit.TestDoubles.JSInterop;

namespace Bunit
{
	/// <summary>
	/// Represents a number of unexpected invocation to a <see cref="MockJSRuntimeInvokeHandler"/>.
	/// </summary>
	public class JSInvokeCountExpectedException : Exception
	{
		/// <summary>
		/// Gets the expected invocation count.
		/// </summary>
		public int ExpectedInvocationCount { get; }

		/// <summary>
		/// Gets the actual invocation count.
		/// </summary>
		public int ActualInvocationCount { get; }

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		public string Identifier { get; }

		/// <summary>
		/// Creates an instance of the <see cref="JSInvokeCountExpectedException"/>.
		/// </summary>
		public JSInvokeCountExpectedException(string identifier, int expectedCount, int actualCount, string assertMethod, string? userMessage = null)
			: base(CreateMessage(identifier, expectedCount, actualCount, assertMethod, userMessage))
		{
			ExpectedInvocationCount = expectedCount;
			ActualInvocationCount = actualCount;
			Identifier = identifier;
		}

		private static string CreateMessage(string identifier, int expectedCount, int actualCount, string assertMethod, string? userMessage = null)
		{
			var result = $"{assertMethod} failed: ";
			result += userMessage is null
					? $"\"{identifier}\" was not called the expected number of times."
					: userMessage;

			result += Environment.NewLine;
			result += Environment.NewLine;
			result += $"Expected number of calls: {expectedCount}{Environment.NewLine}";
			result += $"Actual number of calls:   {actualCount}{Environment.NewLine}";

			return result;
		}
	}
}
