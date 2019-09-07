using System;
using System.Xml;
using Egil.RazorComponents.Testing.Diffing;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
    public class RazorComponentsMatchException : XunitException
    {
        public RazorComponentsMatchException(XmlNode expectedHtml) : base($"Expected HTML and Rendered HTML should not match." +
            $"The expected HTML was:{Environment.NewLine}" +
            $"{expectedHtml.PrettyXml()}")
        {
        }
    }
}
