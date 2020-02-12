using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using AngleSharp;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Bunit;

namespace Xunit.Sdk
{
    /// <summary>
    /// Represents an differences between pieces of markup.
    /// </summary>
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
    public class HtmlEqualException : AssertActualExpectedException
    {
        /// <summary>
        /// Creates an instance of the <see cref="HtmlEqualException"/> type.
        /// </summary>
        public HtmlEqualException(IEnumerable<IDiff> diffs, IMarkupFormattable expected, IMarkupFormattable actual, string? userMessage)
            : base(PrintHtml(expected), PrintHtml(actual), CreateUserMessage(diffs, userMessage), "Expected HTML", "Actual HTML")
        {
        }

        private static string CreateUserMessage(IEnumerable<IDiff> diffs, string? userMessage)
        {
            var result = $"HTML comparison failed. ";
            if(userMessage is { })
                result = $"{result}{userMessage}{Environment.NewLine}";

            return $"{result}The following errors was found:{Environment.NewLine}{PrintDiffs(diffs)}";
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
