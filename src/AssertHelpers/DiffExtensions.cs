using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using AngleSharp;
using AngleSharp.Diffing.Core;
using AngleSharp.Html;

namespace Egil.RazorComponents.Testing
{

    public static class DiffExtensions
    {
        public static string ToDiffHtml(this IMarkupFormattable nodes)
        {
            if (nodes is null) throw new ArgumentNullException(nameof(nodes));

            using var sw = new StringWriter();
            nodes.ToHtml(sw, new DiffMarkupFormatter());
            return sw.ToString();
        }

        public static string ToDiffAssertMessage(this IEnumerable<IDiff> diffs, IMarkupFormattable controlNodes, IMarkupFormattable testNodes, string? userMessage = "")
        {
            if (controlNodes is null) throw new ArgumentNullException(nameof(controlNodes));
            if (testNodes is null) throw new ArgumentNullException(nameof(testNodes));

            return ToDiffAssertMessage(diffs, controlNodes.ToDiffHtml(), testNodes.ToDiffHtml(), userMessage);
        }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "<Pending>")]
        public static string ToDiffAssertMessage(this IEnumerable<IDiff> diffs, string controlHtml, string testHtml, string? userMessage = "")
        {
            var diffCount = 0;
            var diffsText = string.Join(Environment.NewLine, diffs.Select((x, i) =>
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
                diffCount++;
                return $"  {i + 1}: {diffText}";
            }));

            var errorsPluralizer = diffCount > 1 ? "errors" : "error";
            return $"{GetUserMessage()}{Environment.NewLine}" +
                $"The following {diffCount} {errorsPluralizer} was found during diffing: {Environment.NewLine}" +
                $"{diffsText}{Environment.NewLine}{Environment.NewLine}" +
                $"Expected HTML:{controlHtml}{Environment.NewLine}{Environment.NewLine}" +
                $"Actual HTML:{testHtml}{Environment.NewLine}";

            string GetUserMessage() => userMessage is null
                ? $"The actual change does not match the expected change.{Environment.NewLine}"
                : $"{userMessage}.{Environment.NewLine}";
        }
    }
}
