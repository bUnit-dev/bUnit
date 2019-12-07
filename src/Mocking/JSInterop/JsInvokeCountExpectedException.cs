using System;
using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing.Mocking.JsInterop
{
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
    public class JsInvokeCountExpectedException : AssertActualExpectedException
    {
        public JsInvokeCountExpectedException(string identifier, int expectedCount, int actualCount, string assertMethod, string? userMessage = null)
            : base(expectedCount, actualCount, CreateMessage(assertMethod, identifier, userMessage), "Expected number of calls", "Actual number of calls")
        {
        }

        public static void ThrowJsInvokeCountExpectedException(string identifier, int expectedCount, int actualCount, string assertMethod, string? userMessage = null)
        {
            // This throws an AssertActualExpectedException because the output in the test runner is much prettier. The full type is not prefixed to the message.
            throw new AssertActualExpectedException(expectedCount, actualCount, CreateMessage(assertMethod, identifier, userMessage), "Expected number of calls", "Actual number of calls");
        }

        // This doesnt seem to called by the test runner.
        //public override string ToString()
        //{
        //    var result = Message;

        //    if (StackTrace is { })
        //    {
        //        result = $"{result}{Environment.NewLine}{StackTrace}";
        //    }

        //    return result;
        //}

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
