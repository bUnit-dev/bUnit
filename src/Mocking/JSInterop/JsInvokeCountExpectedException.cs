using System;
using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;

namespace Xunit.Sdk
{
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
    public class JsInvokeCountExpectedException : AssertActualExpectedException
    {
        public JsInvokeCountExpectedException(string identifier, int expectedCount, int actualCount, string assertMethod, string? userMessage = null)
            : base(expectedCount, actualCount, CreateMessage(assertMethod, identifier, userMessage), "Expected number of calls", "Actual number of calls")
        {
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
