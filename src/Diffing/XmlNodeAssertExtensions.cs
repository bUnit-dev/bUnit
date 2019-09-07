using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Egil.RazorComponents.Testing.Diffing;
using Org.XmlUnit;
using Org.XmlUnit.Builder;
using Org.XmlUnit.Diff;

namespace Egil.RazorComponents.Testing
{

    public static class XmlNodeAssertExtensions
    {
        public static void ShouldBe(this XmlNode renderedHtml, XmlNode expectedHtml)
        {
            if (renderedHtml is null) throw new ArgumentNullException(nameof(renderedHtml));
            if (expectedHtml is null) throw new ArgumentNullException(nameof(expectedHtml));

            var diffResult = CreateDiff(expectedHtml, renderedHtml);
            if (diffResult.HasDifferences())
                throw new RazorComponentDoesNotMatchException(expectedHtml.FirstChild, renderedHtml.FirstChild, diffResult);
        }

        public static void ShouldNotBe(this XmlNode renderedHtml, XmlNode expectedHtml)
        {
            if (renderedHtml is null) throw new ArgumentNullException(nameof(renderedHtml));
            if (expectedHtml is null) throw new ArgumentNullException(nameof(expectedHtml));

            var diffResult = CreateDiff(expectedHtml, renderedHtml);
            if (!diffResult.HasDifferences())
                throw new RazorComponentsMatchException(expectedHtml.FirstChild);
        }

        private static Diff CreateDiff(XmlNode control, XmlNode test)
        {
            var controlSource = Input.FromNode(control.FirstChild).Build();
            var testSource = Input.FromNode(test.FirstChild).Build();
            return CreateDiff(controlSource, testSource);
        }

        public static Diff CreateDiff(ISource control, ISource test)
        {
            return DiffBuilder.Compare(control)
                .IgnoreWhitespace()
                .WithNamespaceContext(new Dictionary<string, string>() { { "regex", "urn:egil.razorcomponents.testing.library.regex" } })
                .WithTest(test)
                .WithDifferenceEvaluator(DifferenceEvaluators.Chain(
                    DifferenceEvaluators.Default,
                    HtmlDifferenceEvaluators.RegexAttributeDifferenceEvaluator,
                    HtmlDifferenceEvaluators.CssClassAttributeDifferenceEvaluator)
                )
                .Build();
        }

        public static string PrettyXml(this XmlNode? xml)
        {
            if (xml is null) return string.Empty;

            var result = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = false,
                ConformanceLevel = ConformanceLevel.Fragment,
                IndentChars = "  ",
            };

            using (var xmlWriter = XmlWriter.Create(result, settings))
            {
                xml.WriteTo(xmlWriter);
            }

            result.Insert(0, Environment.NewLine);
            result.AppendLine();

            return result.ToString();
        }
    }
}
