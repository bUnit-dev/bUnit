using System;

namespace Bunit
{
    /// <summary>
    /// Represents an exception thrown when the awaited assertion does not pass.
    /// </summary>
    public class WaitForAssertionFailedException : Exception
    {
        private const string MESSAGE = "The assertion did not pass within the timeout period.";
        
        internal WaitForAssertionFailedException(Exception assertionException) : base(MESSAGE, assertionException)
        {
        }
    }
}
