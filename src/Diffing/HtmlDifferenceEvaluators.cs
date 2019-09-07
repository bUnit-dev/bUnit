using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Egil.RazorComponents.Testing.Rendering;
using Org.XmlUnit.Diff;

namespace Egil.RazorComponents.Testing.Diffing
{
    public static class HtmlDifferenceEvaluators
    {
        private const string RegexNamespace = RenderedFactXmlDocumentExtensions.RegexXmlNamespace;
        private readonly static char[] Space = new char[] { ' ' };

        public static ComparisonResult RegexAttributeDifferenceEvaluator(Comparison comparison, ComparisonResult outcome)
        {
            if (comparison is null) throw new ArgumentNullException(nameof(comparison));

            if (outcome == ComparisonResult.EQUAL) return outcome;
            if (comparison.Type != ComparisonType.ATTR_NAME_LOOKUP) return outcome;

            if (comparison.ControlDetails.Value is XmlQualifiedName a1 && a1.Namespace == RegexNamespace)
            {
                var testAttr = comparison.TestDetails.Target.Attributes.GetNamedItem(a1.Name);
                var ctrlAttr = comparison.ControlDetails.Target.Attributes.GetNamedItem(a1.Name, a1.Namespace);
                if (Regex.IsMatch(testAttr.Value, ctrlAttr.Value))
                {
                    return ComparisonResult.EQUAL;
                }
            }
            else if (comparison.TestDetails.Value is XmlQualifiedName a2 && comparison.ControlDetails.Target.Attributes.GetNamedItem(a2.Name, RegexNamespace) is XmlNode ctrlAttr)
            {
                return ComparisonResult.EQUAL;
            }
            return outcome;
        }

        public static ComparisonResult CssClassAttributeDifferenceEvaluator(Comparison comparison, ComparisonResult outcome)
        {
            if (comparison is null) throw new ArgumentNullException(nameof(comparison));

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
    }
}
