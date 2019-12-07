using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Asserting;
using Egil.RazorComponents.Testing.Diffing;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    public static class ShouldBeTextChangeAssertExtensions
    {
        public static void ShouldHaveSingleTextChange(this IReadOnlyList<IDiff> diffs, string expectedChange, string? userMessage = null)
        {
            DiffAssertExtensions.ShouldHaveSingleChange(diffs).ShouldBeTextChange(expectedChange, userMessage);
        }

        public static void ShouldBeTextChange(this IDiff actualChange, string expectedChange, string? userMessage = null)
        {
            if (actualChange is null) throw new ArgumentNullException(nameof(actualChange));
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));

            var actual = Assert.IsType<NodeDiff>(actualChange);
            var parser = actual.Control.Node.Owner.Context.GetService<HtmlParser>();
            var expected = parser.Parse(expectedChange);

            ShouldBeTextChange(actualChange, expected, userMessage);
        }

        public static void ShouldBeTextChange(this IDiff actualChange, IRenderedFragment expectedChange, string? userMessage = null)
        {
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));
            ShouldBeTextChange(actualChange, expectedChange.GetNodes(), userMessage);
        }

        public static void ShouldBeTextChange(this IDiff actualChange, INodeList expectedChange, string? userMessage = null)
        {
            if (actualChange is null) throw new ArgumentNullException(nameof(actualChange));
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));

            var actual = Assert.IsType<NodeDiff>(actualChange);
            var comparer = actual.Control.Node.Owner.Context.GetService<HtmlComparer>();

            var diffs = comparer.Compare(expectedChange, new[] { actual.Test.Node }).ToList();

            if (diffs.Count != 0)
            {
                HtmlEqualException.ThrowHtmlEqualException(diffs,expectedChange, actual.Test.Node, userMessage);
            }
        }
    }
}
