using System;
using System.Collections.Generic;
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
            var expected = comparison.ControlDetails.Value.ToString()!.Split(Space, StringSplitOptions.RemoveEmptyEntries);
            var actual = comparison.TestDetails.Value.ToString()!.Split(Space, StringSplitOptions.RemoveEmptyEntries);

            if (SetEqual(expected, actual)) return ComparisonResult.EQUAL;
            else return outcome;
        }

        private static bool SetEqual(string[] expected, string[] actual)
        {
            if (expected.Length != actual.Length) return false;
            return new HashSet<string>(expected).SetEquals(new HashSet<string>(actual));
        }

        public static readonly DifferenceEvaluator Default = new CssClassAttributeDifferenceEvaluator().Evaluate;
    }
}
