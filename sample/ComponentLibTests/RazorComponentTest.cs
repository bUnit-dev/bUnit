using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace ComponentLib
{
    [Trait("Category", "Razor Component Tests")]
    public abstract class RazorComponentTest // : IClassFixture<RazorComponentFixture>
    {
        public IReadOnlyList<TestRenderResult> RenderResults { get; }

        public RazorComponentTest()
        {
            // TODO: Get this injected by xunit once for all tests in class. 
            // Doesnt currently work as there have to be a parameter less constructor
            // for razor file generator to work, and if there are multiple, xunit
            // picks the first without parameters...
            using var fixture = new RazorComponentFixture();
            var services = new ServiceCollection();
            AddServices(services);
            fixture.Render(BuildRenderTree, services);
            RenderResults = fixture.RenderResults;
        }

        protected virtual void AddServices(IServiceCollection services) { }

        protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

        [Fact(DisplayName = "Rendered HTML should be the same as expected HTML")]
        public virtual void ExecuteTest()
        {
            foreach (var result in RenderResults)
            {
                AssertRenderHtmlIsExpected(result);
            }
        }

        protected static void AssertRenderHtmlIsExpected(TestRenderResult result)
        {
            var diff = result.CreateRenderDiff();
            diff.HasDifferences().ShouldBeFalse(() => CreateValidationErrorMessage(result, diff));
        }

        private static string CreateValidationErrorMessage(TestRenderResult result, Org.XmlUnit.Diff.Diff diffResult)
        {
            const string Tab = "\t";
            return $"should be" +
                            $"{Environment.NewLine}{Environment.NewLine}{PrettyXml(result.ExpectedHtml?.FirstChild)}" +
                            $"{Environment.NewLine}{Environment.NewLine}{Tab}but was" +
                            $"{Environment.NewLine}{Environment.NewLine}{PrettyXml(result.RenderedHtml.FirstChild)}" +
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