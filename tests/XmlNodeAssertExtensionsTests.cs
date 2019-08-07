using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Shouldly;
using Xunit;

namespace Egil.RazorComponents.Testing.Library.Tests
{
    public class XmlNodeAssertExtensionsTests
    {
        [Fact(DisplayName = "xmlNode.ShouldBe(xmlNode) throws correct exception when nodes are different")]
        public void MyTestMethod()
        {
            var doc = new XmlDocument();
            doc.LoadXml("<RenderResult>" +
                "<RenderedHtml><Html></Html></RenderedHtml>" +
                "<ExpectedHtml><Html><div class='x'></div></Html></ExpectedHtml>" +
                "</RenderResult>");

            var control = doc.SelectSingleNode("RenderResult/ExpectedHtml");
            var test = doc.SelectSingleNode("RenderResult/RenderedHtml");

            Should.Throw<RazorComponentDoesNotMatchException>(() => XmlNodeAssertExtensions.ShouldBe(test, control));
        }
    }
}
