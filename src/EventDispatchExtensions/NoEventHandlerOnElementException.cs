using System;
using System.Runtime.Serialization;

namespace Egil.RazorComponents.Testing.EventDispatchExtensions
{
    /// <summary>
    /// Exception that is thrown if an event is triggered 
    /// where there is no event handler bound to handle it.
    /// </summary>
    public class NoEventHandlerOnElementException : Exception
    {
        /// <inheritdoc/>
        public NoEventHandlerOnElementException(string message) : base(message)
        {
        }
    }
}