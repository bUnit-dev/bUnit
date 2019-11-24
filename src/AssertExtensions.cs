using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Diffing;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    public static class AssertExtensions
    {
        public static IReadOnlyList<IDiff> CompareTo(this IRenderedFragment actual, IRenderedFragment expected)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            return actual.GetNodes().CompareTo(expected.GetNodes());
        }

        public static IReadOnlyList<IDiff> CompareTo(this INodeList actual, INodeList expected)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            if (actual.Length == 0 && expected.Length == 0) return Array.Empty<IDiff>();

            var comparer = actual.Length > 0
                ? actual[0].Owner.Context.GetService<HtmlComparer>()
                : expected[0].Owner.Context.GetService<HtmlComparer>();

            return comparer.Compare(expected, actual).ToArray();
        }

        public static void ShouldBeAddition(this IDiff actualChange, IRenderedFragment expectedChange, string? userMessage = null)
        {
            if (actualChange is null) throw new ArgumentNullException(nameof(actualChange));
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));

            var actual = Assert.IsType<UnexpectedNodeDiff>(actualChange);
            var expected = expectedChange.GetNodes();
            var comparer = expected[0].Owner.Context.GetService<HtmlComparer>();

            var diffs = comparer.Compare(expected, new[] { actual.Test.Node }).ToList();
            Assert.True(diffs.Count == 0, $"{GetUserMessage()}{StringifyDiffs(expected.ToHtml(), actual.Test.Node.ToHtml(), diffs)}");

            string GetUserMessage() => userMessage is null
                ? $"The actual change does not match the expected change.{Environment.NewLine}"
                : $"{userMessage}.{Environment.NewLine}";
        }

        public static void ShouldBe(this IRenderedFragment actual, string expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            var actualNodes = actual.GetNodes();
            var expectedNodes = actual.TestContext.HtmlParser.Parse(expected);

            actualNodes.ShouldBe(expectedNodes, userMessage);
        }

        public static void ShouldBe(this IRenderedFragment actual, IRenderedFragment expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            actual.GetNodes().ShouldBe(expected.GetNodes(), userMessage);
        }

        public static void ShouldBe(this INodeList actual, INodeList expected, string? userMessage = null)
        {
            var diffs = CompareTo(actual, expected);

            Assert.True(diffs.Count == 0, $"{GetUserMessage()}{StringifyDiffs(expected.ToHtml(), actual.ToHtml(), diffs)}");

            string GetUserMessage() => userMessage is null
                ? string.Empty
                : $"{userMessage}.{Environment.NewLine}";
        }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "<Pending>")]
        private static string StringifyDiffs(string controlHtml, string testHtml, IReadOnlyList<IDiff> diffs)
        {
            var diffsText = string.Join($"{Environment.NewLine}", diffs.Select((x, i) =>
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

            return $"{Environment.NewLine}The following errors was found during diffing: {Environment.NewLine}{diffsText}{Environment.NewLine}{Environment.NewLine}" +
                $"Control HTML was:{Environment.NewLine}{controlHtml}{Environment.NewLine}" +
                $"Test HTML was:{Environment.NewLine}{testHtml}{Environment.NewLine}";
        }
    }
}
