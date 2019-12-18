using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing
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
    }
}
