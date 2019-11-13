using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Xunit;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using AngleSharp.Diffing.Core;

namespace Egil.RazorComponents.Testing
{
    public class Snapshot : TestingComponentBase
    {
        [Inject] private IHtmlComparer HtmlComparer { get; set; } = default!;

        [Parameter] public string Description { get; set; } = string.Empty;

        [Parameter] public RenderFragment? ChildContent { get; set; }

        public override void ExecuteTest()
        {
            if (ChildContent is null) throw new InvalidOperationException($"Parameter {nameof(ChildContent)} cannot be null.");

            RenderContext.RenderComponentUnderTest(ChildContent);
            var testParts = RenderContext.GetComponents();
            var given = testParts.FirstOrDefault(x => x.Component is Given);
            var givenHtml = RenderContext.GetHtml(given.Id);

            var expect = testParts.FirstOrDefault(x => x.Component is Expect);
            var expectHtml = RenderContext.GetHtml(expect.Id);

            var diffs = HtmlComparer.Compare(expectHtml, givenHtml);
            Assert.True(diffs.Count == 0, $"{Description} failed.{Environment.NewLine}{StringifyDiffs(expectHtml, givenHtml, diffs)}");
        }

        [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "<Pending>")]
        private static string StringifyDiffs(string controlHtml, string testHtml, List<IDiff> diffs)
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