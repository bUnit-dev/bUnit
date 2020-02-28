using System;

namespace Xunit.Sdk
{
    /// <summary>
    /// Represents a failure to find an element in the searched target
    /// using a css selector.
    /// </summary>
    public class ElementNotFoundException : Exception
    {
        /// <summary>
        /// The css selector used to search with.
        /// </summary>
        public string CssSelector { get; }

        /// <inheritdoc/>
        public ElementNotFoundException(string cssSelector) : base($"No elements were found that matches the selector '{cssSelector}'")
        {
            CssSelector = cssSelector;
        }

        /// <inheritdoc/>
        public ElementNotFoundException()
        {
            CssSelector = string.Empty;
        }

        /// <inheritdoc/>
        public ElementNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            CssSelector = string.Empty;
        }
    }
}
