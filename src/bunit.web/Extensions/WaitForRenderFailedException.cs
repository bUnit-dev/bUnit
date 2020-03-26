using System;

namespace Bunit
{
    /// <summary>
    /// Represents an exception that is thrown when a render does not happen within the specified wait period.
    /// </summary>
    public class WaitForRenderFailedException : Exception
    {
        private const string MESSAGE = "No render happened before the timeout period passed.";

        /// <inheritdoc/>
        public override string Message => MESSAGE;
    }
}
