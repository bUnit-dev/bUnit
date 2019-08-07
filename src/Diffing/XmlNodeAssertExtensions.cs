using System.Xml;
using Org.XmlUnit;
using Org.XmlUnit.Builder;
using Org.XmlUnit.Diff;

namespace Egil.RazorComponents.Testing
{

    public static class XmlNodeAssertExtensions
    {        
        public static void ShouldBe(this XmlNode renderedHtml, XmlNode expectedHtml)
        {
            var diffResult = CreateDiff(expectedHtml, renderedHtml);
            if (diffResult.HasDifferences())
                throw new RazorComponentDoesNotMatchException(expectedHtml.FirstChild, renderedHtml.FirstChild, diffResult);
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
                .WithTest(test)
                .WithDifferenceEvaluator(DifferenceEvaluators.Chain(
                    DifferenceEvaluators.Default,
                    RegexAttributeDifferenceEvaluator.Default,
                    CssClassAttributeDifferenceEvaluator.Default)
                )
                .Build();
        }
    }
}
