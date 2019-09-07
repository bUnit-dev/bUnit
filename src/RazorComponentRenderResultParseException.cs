using System;
using System.Runtime.Serialization;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
    public class RazorComponentRenderResultParseException : XunitException
    {
        public RazorComponentRenderResultParseException(string userMessage, Exception innerException) : base(userMessage, innerException)
        {
        }
    }
}