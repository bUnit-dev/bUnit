using System.Text.RegularExpressions;
using Org.XmlUnit.Diff;

namespace Egil.RazorComponents.Testing
{
    internal class RegexAttributeDifferenceEvaluator
    {
        public ComparisonResult Evaluate(Comparison comparison, ComparisonResult outcome)
        {
            if (outcome == ComparisonResult.EQUAL) return outcome;
            if (comparison.Type != ComparisonType.ATTR_VALUE) return outcome;
            if (!comparison.ControlDetails.Value.ToString().StartsWith("RegEx:")) return outcome;

            var pattern = comparison.ControlDetails.Value.ToString().Substring(6);

            if (Regex.IsMatch(comparison.TestDetails.Value.ToString(), pattern))
                return ComparisonResult.EQUAL;
            else return outcome;
        }

        internal static readonly DifferenceEvaluator Default = new RegexAttributeDifferenceEvaluator().Evaluate;
    }
}
