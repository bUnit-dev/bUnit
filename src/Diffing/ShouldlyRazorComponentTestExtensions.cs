using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components.Rendering;
using Org.XmlUnit;
using Org.XmlUnit.Builder;
using Org.XmlUnit.Diff;
using Shouldly;

namespace Egil.RazorComponents.Testing
{
    public static class ShouldlyRazorComponentTestExtensions
    {
        private const string Tab = "\t";

        public static void ShouldBe(this XmlNode renderedHtml, XmlNode expectedHtml)
        {
            var diffResult = CreateDiff(expectedHtml, renderedHtml);

            diffResult.HasDifferences().ShouldBeFalse(CreateValidationErrorMessage(expectedHtml, renderedHtml, diffResult));
        }

        public static Diff CreateDiff(XmlNode control, XmlNode test)
        {
            return CreateDiff(Input.FromNode(control.FirstChild).Build(), Input.FromNode(test.FirstChild).Build());
        }

        public static Diff CreateDiff(ISource control, ISource test)
        {
            return DiffBuilder.Compare(control)
                .IgnoreWhitespace()
                .WithTest(test)
                .WithDifferenceEvaluator(DifferenceEvaluators.Chain(
                    DifferenceEvaluators.Default,
                    RegexAttributeDifferenceEvaluator.Default,
                    CssClassAttributeDifferenceEvaluator.Default)
                )
                .Build();
        }

        private static string CreateValidationErrorMessage(XmlNode expectedHtml, XmlNode renderedHtml, Org.XmlUnit.Diff.Diff diffResult)
        {
            return $"should be" +
                            $"{Environment.NewLine}{Environment.NewLine}{PrettyXml(expectedHtml.FirstChild)}" +
                            $"{Environment.NewLine}{Environment.NewLine}{Tab}but was" +
                            $"{Environment.NewLine}{Environment.NewLine}{PrettyXml(renderedHtml.FirstChild)}" +
                            $"{Environment.NewLine}{Environment.NewLine}{Tab}with the following differences:" +
                            $"{Environment.NewLine}{CreateDiffMessage(diffResult)}" +
                            $"{Environment.NewLine}";
        }

        private static string CreateDiffMessage(Diff diffResult)
        {
            return diffResult.Differences
                            .Select(diff => $"- {diff.ToString()}")
                            .Aggregate(string.Empty, (acc, diff) => $"{acc}{Environment.NewLine}{diff}");
        }

        private static string PrettyXml(XmlNode? xml)
        {
            if (xml is null) return string.Empty;

            var stringBuilder = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = false,
                ConformanceLevel = ConformanceLevel.Fragment,
                IndentChars = "  ",
            };

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                xml.WriteTo(xmlWriter);
            }

            return stringBuilder.ToString();
        }
    }
}
