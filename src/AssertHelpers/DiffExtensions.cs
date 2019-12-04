using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AngleSharp.Diffing.Core;

namespace Egil.RazorComponents.Testing
{
    public static class DiffExtensions
    {
        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "<Pending>")]
        public static string ToDiffAssertMessage(this IEnumerable<IDiff> diffs, string controlHtml, string testHtml, string? userMessage = "")
        {
            var diffsText = string.Join(Environment.NewLine, diffs.Select((x, i) =>
            {
                var diffText = x switch
                {
                    //NodeDiff diff when diff.Target == DiffTarget.Text =>
                    //    $"The control text '{diff.Control.Node.TextContent}' at {diff.Control.Path} is different from the test text '{diff.Test.Node.TextContent}' at {diff.Test.Path}.",
                    NodeDiff diff => $"The control {diff.Control.Node.NodeType.ToString().ToLowerInvariant()} {diff.Control.Path} and the test {diff.Test.Node.NodeType.ToString().ToLowerInvariant()} {diff.Test.Path} are different.",
                    AttrDiff diff => $"The value of the control attribute {diff.Control.Path} and test attribute {diff.Test.Path} are different.",
                    MissingNodeDiff diff => $"The control {diff.Control.Node.NodeType.ToString().ToLowerInvariant()} at {diff.Control.Path} is missing.",
                    MissingAttrDiff diff => $"The control attribute at {diff.Control.Path} is missing.",
                    UnexpectedNodeDiff diff => $"The test {diff.Test.Node.NodeType.ToString().ToLowerInvariant()} at {diff.Test.Path} was not expected.",
                    UnexpectedAttrDiff diff => $"The test attribute at {diff.Test.Path} was not expected.",
                    _ => throw new InvalidOperationException($"Unknown diff type detected: {x.GetType()}")
                };
                return $"  {i + 1}: {diffText}";
            }));

            return $"{GetUserMessage()}{Environment.NewLine}" +
                $"The following errors was found during diffing: {Environment.NewLine}{diffsText}{Environment.NewLine}{Environment.NewLine}" +
                $"Control HTML was:{Environment.NewLine}{controlHtml}{Environment.NewLine}" +
                $"Test HTML was:{Environment.NewLine}{testHtml}{Environment.NewLine}";


            string GetUserMessage() => userMessage is null
                ? $"The actual change does not match the expected change.{Environment.NewLine}"
                : $"{userMessage}.{Environment.NewLine}";
        }
    }
}
