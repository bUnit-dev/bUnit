using System;

namespace Bunit
{
    /// <summary>
    /// Represents an exception thrown when the state predicate does not pass or if it throws itself.
    /// </summary>
    public class WaitForStateFailedException : Exception
    {
        private const string TIMEOUT_NO_RENDER = "The state predicate did not pass before the timeout period passed.";
        private const string EXCEPTION_IN_PREDICATE = "The state predicate throw an unhandled exception.";

        private WaitForStateFailedException() : base(TIMEOUT_NO_RENDER, new TimeoutException(TIMEOUT_NO_RENDER))
        {
        }

        private WaitForStateFailedException(Exception innerException) : base(EXCEPTION_IN_PREDICATE, innerException)
        {
        }

        internal static WaitForStateFailedException CreateNoMatchBeforeTimeout()
            => new WaitForStateFailedException();

        internal static WaitForStateFailedException CreatePredicateThrowException(Exception innerException)
            => new WaitForStateFailedException(innerException);
    }
}
