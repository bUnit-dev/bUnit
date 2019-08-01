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
        private const string TestRootElementName = "ROOT";

        public static void ShouldBe(this ComponentRenderedText componentRenderedText, string expectedHtml)
        {
            var testHtml = string.Concat(componentRenderedText.Tokens).Trim();
            var diffResult = CreateDiff(expectedHtml, testHtml);

            diffResult.HasDifferences().ShouldBeFalse(CreateValidationErrorMessage(expectedHtml, testHtml, diffResult));
        }

        public static Diff CreateDiff(string expectedHtml, string testHtml)
        {
            Diff? diffResult = null;

            Should.NotThrow(() =>
            {
                var test = Input.FromString(WrapInTestRoot(testHtml)).Build();
                var control = Input.FromString(WrapInTestRoot(expectedHtml)).Build();
                diffResult = CreateDiff(control, test);
            }, CreateParseErrorText(expectedHtml, testHtml));

            return diffResult!;
        }

        public static Diff CreateDiff(XmlNode control, XmlNode test)
        {
            return CreateDiff(Input.FromNode(control).Build(), Input.FromNode(test).Build());
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

        private static string CreateParseErrorText(string expectedHtml, string testHtml)
        {
            return $"Error while parsing and creating diff of HTML snippets. " +
                   $"{Environment.NewLine}{Environment.NewLine}Expected HTML:" +
                   $"{Environment.NewLine}{Environment.NewLine}{PrettyXml(expectedHtml)}" +
                   $"{Environment.NewLine}{Environment.NewLine}Test HTML: " +
                   $"{Environment.NewLine}{Environment.NewLine}{PrettyXml(testHtml)}";
        }

        private static string CreateValidationErrorMessage(string expectedHtml, string testHtml, Org.XmlUnit.Diff.Diff diffResult)
        {
            return $"should be" +
                            $"{Environment.NewLine}{Environment.NewLine}{PrettyXml(expectedHtml)}" +
                            $"{Environment.NewLine}{Environment.NewLine}{Tab}but was" +
                            $"{Environment.NewLine}{Environment.NewLine}{PrettyXml(testHtml)}" +
                            $"{Environment.NewLine}{Environment.NewLine}{Tab}with the following differences:" +
                            $"{Environment.NewLine}{CreateDiffMessage(diffResult)}" +
                            $"{Environment.NewLine}";
        }

        private static string CreateDiffMessage(Org.XmlUnit.Diff.Diff diffResult)
        {
            return diffResult.Differences
                            .Select(diff => $"- {diff.ToString()}")
                            .Aggregate(string.Empty, (acc, diff) => $"{acc}{Environment.NewLine}{diff}");
        }

        private static string WrapInTestRoot(string html)
        {
            return $"<{TestRootElementName}>{Environment.NewLine}{html ?? string.Empty}{Environment.NewLine}</{TestRootElementName}>";
        }

        private static string PrettyXml(string xml)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                var element = XElement.Parse(xml);
                var settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true,
                    NewLineOnAttributes = false
                };

                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }

                return stringBuilder.ToString();
            }
            catch
            {
                return xml;
            }
        }
    }
}
