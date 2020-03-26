using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AngleSharp;
using AngleSharp.Diffing.Core;
using Bunit;
using Bunit.Asserting;

namespace Bunit
{
    /// <summary>
    /// Represents an differences between pieces of markup.
    /// </summary>
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
    public class HtmlEqualException : ActualExpectedAssertException
    {
        /// <summary>
        /// Creates an instance of the <see cref="HtmlEqualException"/> type.
        /// </summary>
        public HtmlEqualException(IEnumerable<IDiff> diffs, IMarkupFormattable expected, IMarkupFormattable actual, string? userMessage)
            : base(PrintHtml(actual), PrintHtml(expected), "Actual HTML", "Expected HTML", CreateUserMessage(diffs, userMessage))
        {
        }

        private static string CreateUserMessage(IEnumerable<IDiff> diffs, string? userMessage)
        {
            return $"HTML comparison failed. {userMessage}{Environment.NewLine}{Environment.NewLine}The following errors were found:{Environment.NewLine}{PrintDiffs(diffs)}";
        }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "<Pending>")]
        private static string PrintDiffs(IEnumerable<IDiff> diffs)
        {
            return string.Join(Environment.NewLine, diffs.Select((x, i) =>
            {
                var diffText = x switch
                {
                    NodeDiff diff => $"The expected {diff.Control.Node.NodeType.ToString().ToLowerInvariant()} at {diff.Control.Path} and the actual {diff.Test.Node.NodeType.ToString().ToLowerInvariant()} at {diff.Test.Path} are different.",
                    AttrDiff diff => $"The value of the expected attribute {diff.Control.Path} and actual attribute {diff.Test.Path} are different.",
                    MissingNodeDiff diff => $"The {diff.Control.Node.NodeType.ToString().ToLowerInvariant()} at {diff.Control.Path} is missing.",
                    MissingAttrDiff diff => $"The attribute at {diff.Control.Path} is missing.",
                    UnexpectedNodeDiff diff => $"The {diff.Test.Node.NodeType.ToString().ToLowerInvariant()} at {diff.Test.Path} was not expected.",
                    UnexpectedAttrDiff diff => $"The attribute at {diff.Test.Path} was not expected.",
                    _ => throw new InvalidOperationException($"Unknown diff type detected: {x.GetType()}")
                };
                return $"  {i + 1}: {diffText}";
            })) + Environment.NewLine;
        }

        private static string PrintHtml(IMarkupFormattable nodes) => nodes.ToDiffMarkup() + Environment.NewLine;
    }
}
