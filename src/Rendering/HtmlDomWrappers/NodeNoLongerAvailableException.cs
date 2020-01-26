using System;
using System.Runtime.Serialization;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    /// <summary>
    /// Indicate that an DOM node has been removed from the DOM tree after a render
    /// </summary>
    public class NodeNoLongerAvailableException : Exception
    {
        /// <inheritdoc/>
        public NodeNoLongerAvailableException() : base("The DOM node you tried to access is no longer available in the DOM tree. It has probably been removed after a render.")
        {
        }
    }
}