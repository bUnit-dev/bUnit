using System.Xml;
using Org.XmlUnit.Builder;
using Org.XmlUnit.Diff;

namespace Egil.RazorComponents.Testing
{
    public readonly struct TestRenderResult
    {
        public string Id { get; }
        public string DisplayName { get; }
        public XmlNode RenderedHtml { get; }
        public XmlNode? ExpectedHtml { get; }

        public TestRenderResult(string id, string displayName, XmlNode actual, XmlNode? expected)
        {
            Id = id;
            DisplayName = displayName;
            RenderedHtml = actual;
            ExpectedHtml = expected;
        }

        public Diff CreateRenderDiff()
        {
            var control = Input.FromNode(ExpectedHtml?.FirstChild).Build();
            var test = Input.FromNode(RenderedHtml.FirstChild).Build();

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