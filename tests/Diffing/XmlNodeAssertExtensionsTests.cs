using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Shouldly;
using Xunit;

namespace Egil.RazorComponents.Testing.Library.Diffing
{
    public class XmlNodeAssertExtensionsTests
    {
        [Fact(DisplayName = "xmlNode.ShouldBe(xmlNode) throws correct exception when nodes are different")]
        public void MyTestMethod()
        {            
            var (renderedHtml, expectedHtml) = CreateTestXml(
                $"<h1 class='foo'></h1>",
                $"<div class='bar'></div>"
            );

            Should.Throw<RazorComponentDoesNotMatchException>(() => renderedHtml.ShouldBe(expectedHtml));
        }

        [Fact(DisplayName = "ShouldNotMatch does not throws exception when expected and rendered does not matches")]
        public void ShoudNotMatchDoesNotThrowsWhenNoMatche()
        {
            var (renderedHtml, expectedHtml) = CreateTestXml(
                $"<h1 class='foo'></h1>",
                $"<div class='bar'></div>"
            );

            renderedHtml.ShouldNotBe(expectedHtml);
        }

        [Fact(DisplayName = "ShouldNotMatch throws exception when expected and rendered matches")]
        public void ShoudNotMatchThrowsWhenMatches()
        {
            var (renderedHtml, expectedHtml) = CreateTestXml(
                $"<div class='foo'></div>",
                $"<div class='foo'></div>"
            );

            Should.Throw<RazorComponentsMatchException>(() => renderedHtml.ShouldNotBe(expectedHtml));
        }

        [Theory(DisplayName = "Order of classes and whitespace in class='...' attribute doesn't matter when comparing")]
        [InlineData("", " ")]
        [InlineData("foo bar", "foo bar")]
        [InlineData("foo bar", "bar foo")]
        [InlineData("foo  bar", "foo bar")]
        [InlineData("foo  bar", "foo   bar")]
        [InlineData(" foo  bar", "foo  bar  ")]
        [InlineData(" foo  bar", "    bar  foo")]
        public void ClassAttributeOrderTest(string inputClass, string expectedOutput)
        {
            var (renderedHtml, expectedHtml) = CreateTestXml(
                $"<div class='{inputClass}'></div>",
                $"<div class='{expectedOutput}'></div>"
            );

            renderedHtml.ShouldBe(expectedHtml);
        }

        [Fact(DisplayName = "Expected HTML with 'RegEx:' at the start of their attribute uses the following regex string to compare attribute")]
        public void RegexAttrTest()
        {
            var (renderedHtml, expectedHtml) = CreateTestXml(
                $"<div class='id-{GetHashCode()}'></div>",
                $"<div class='RegEx:^id-[\\d]+$'></div>"
            );

            renderedHtml.ShouldBe(expectedHtml);
        }

        private static (XmlNode RenderedHtml, XmlNode ExpectedHtml) CreateTestXml(string renderedHtml, string expectedHtml)
        {
            var doc = new XmlDocument();
            doc.LoadXml("<RenderResult>" +
                $"<RenderedHtml><Html>{renderedHtml}</Html></RenderedHtml>" +
                $"<ExpectedHtml><Html>{expectedHtml}</Html></ExpectedHtml>" +
                "</RenderResult>");

            var test = doc.SelectSingleNode("RenderResult/RenderedHtml");
            var control = doc.SelectSingleNode("RenderResult/ExpectedHtml");

            return (test, control);
        }


    }
}
