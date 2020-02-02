using System;
using System.Diagnostics.CodeAnalysis;

namespace AngleSharpWrappers
{
    /// <summary>
    /// Indicate that an DOM node has been removed from the DOM tree after a render
    /// </summary>
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
    public class NodeNoLongerAvailableException : Exception
    {
        /// <inheritdoc/>
        [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public NodeNoLongerAvailableException()
            : base("The DOM node you tried to access is no longer available in the DOM tree. It has probably been removed after a render.")
        {
        }
    }
}