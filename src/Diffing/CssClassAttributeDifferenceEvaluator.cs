using System;
using System.Linq;
using Org.XmlUnit.Diff;

namespace Egil.RazorComponents.Testing
{
    public class CssClassAttributeDifferenceEvaluator
    {
        private readonly static char[] Space = new char[] { ' ' };

        public ComparisonResult Evaluate(Comparison comparison, ComparisonResult outcome)
        {
            if (outcome == ComparisonResult.EQUAL) return outcome;
            if (comparison.Type != ComparisonType.ATTR_VALUE) return outcome;
            if (!comparison.TestDetails.Target.Name.Equals("class", StringComparison.OrdinalIgnoreCase)) return outcome;

            // BANG: Value should not be null on ControlDetails and TestDetails.
            var expected = comparison.ControlDetails.Value.ToString()!.Split(Space, StringSplitOptions.RemoveEmptyEntries).ToHashSet();
            var actual = comparison.TestDetails.Value.ToString()!.Split(Space, StringSplitOptions.RemoveEmptyEntries).ToHashSet();

            if (expected.SetEquals(actual))
                return ComparisonResult.EQUAL;
            else return outcome;
        }

        public static readonly DifferenceEvaluator Default = new CssClassAttributeDifferenceEvaluator().Evaluate;
    }
}
