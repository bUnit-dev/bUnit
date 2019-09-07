using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using Org.XmlUnit.Builder;
using Org.XmlUnit.Diff;

namespace Egil.RazorComponents.Testing
{
    [SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
    public readonly struct TestRenderResult
    {
        public string Id { get; }
        public string DisplayName { get; }
        public XmlNode RenderedHtml { get; }
        public XmlNode? ExpectedHtml { get; }
        public IReadOnlyList<XmlNode> Snippets { get; }

        public TestRenderResult(string id, string displayName, XmlNode actual, XmlNode? expected, XmlNodeList xmlSnippets)
        {
            if(xmlSnippets is null) throw new ArgumentNullException(nameof(xmlSnippets));

            Id = id;
            DisplayName = displayName;
            RenderedHtml = actual;
            ExpectedHtml = expected;

            var snippets = new List<XmlNode>();
            foreach (XmlNode? snippet in xmlSnippets)
            {
                if(snippet is null) continue;
                snippets.Add(snippet);
            }
            Snippets = snippets.AsReadOnly();
        }
    }
}