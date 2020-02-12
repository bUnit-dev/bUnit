using System;
using System.Diagnostics.CodeAnalysis;
using Bunit.Mocking.JSInterop;
using Xunit.Sdk;

namespace Xunit.Sdk
{
    /// <summary>
    /// Represents a number of unexpected invocation to a <see cref="MockJsRuntimeInvokeHandler"/>.
    /// </summary>
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
    public class JsInvokeCountExpectedException : AssertActualExpectedException
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
        /// Creates an instance of the <see cref="JsInvokeCountExpectedException"/>.
        /// </summary>
        public JsInvokeCountExpectedException(string identifier, int expectedCount, int actualCount, string assertMethod, string? userMessage = null)
            : base(expectedCount, actualCount, CreateMessage(assertMethod, identifier, userMessage), "Expected number of calls", "Actual number of calls")
        {
            ExpectedInvocationCount = expectedCount;
            ActualInvocationCount = actualCount;
            Identifier = identifier;
        }

        private static string CreateMessage(string assertMethod, string identifier, string? userMessage = null)
        {
            var result = $"{assertMethod} failed: ";
            result += userMessage is null
                    ? $"\"{identifier}\" was not called the expected number of times."
                    : userMessage;
            return result;
        }
    }
}
