using System;
using System.Runtime.Serialization;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing
{
    public class RazorComponentRenderResultParseException : XunitException
    {
        public RazorComponentRenderResultParseException(string userMessage, Exception innerException) : base(userMessage, innerException)
        {
        }
    }
}