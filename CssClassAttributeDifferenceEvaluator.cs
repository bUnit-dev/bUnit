using System;
using System.Linq;
using Org.XmlUnit.Diff;

namespace Egil.RazorComponents.Testing
{
    internal class CssClassAttributeDifferenceEvaluator
    {
        private readonly static char[] Space = new char[] { ' ' };
        public ComparisonResult Evaluate(Comparison comparison, ComparisonResult outcome)
        {
            if (outcome == ComparisonResult.EQUAL) return outcome;
            if (comparison.Type != ComparisonType.ATTR_VALUE) return outcome;
            if (!comparison.TestDetails.Target.Name.Equals("class", StringComparison.OrdinalIgnoreCase)) return outcome;

            var expected = comparison.ControlDetails.Value.ToString().Split(Space, StringSplitOptions.RemoveEmptyEntries).ToHashSet();
            var actual = comparison.TestDetails.Value.ToString().Split(Space, StringSplitOptions.RemoveEmptyEntries).ToHashSet();

            if (expected.SetEquals(actual))
                return ComparisonResult.EQUAL;
            else return outcome;
        }

        internal static readonly DifferenceEvaluator Default = new CssClassAttributeDifferenceEvaluator().Evaluate;
    }
}
