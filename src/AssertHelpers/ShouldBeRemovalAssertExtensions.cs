using System;
using System.Linq;
using AngleSharp;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Diffing;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    public static class ShouldBeRemovalAssertExtensions
    {
        public static void ShouldBeRemoval(this IDiff actualChange, string expectedChange, string? userMessage = null)
        {
            if (actualChange is null) throw new ArgumentNullException(nameof(actualChange));
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));

            var actual = Assert.IsType<MissingNodeDiff>(actualChange);
            var parser = actual.Control.Node.Owner.Context.GetService<HtmlParser>();
            var expected = parser.Parse(expectedChange);

            ShouldBeRemoval(actualChange, expected, userMessage);
        }

        public static void ShouldBeRemoval(this IDiff actualChange, IRenderedFragment expectedChange, string? userMessage = null)
        {
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));
            ShouldBeRemoval(actualChange, expectedChange.GetNodes(), userMessage);
        }

        public static void ShouldBeRemoval(this IDiff actualChange, INodeList expectedChange, string? userMessage = null)
        {
            if (actualChange is null) throw new ArgumentNullException(nameof(actualChange));
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));

            var actual = Assert.IsType<MissingNodeDiff>(actualChange);

            var comparer = actual.Control.Node.Owner.Context.GetService<HtmlComparer>();

            var diffs = comparer.Compare(expectedChange, new[] { actual.Control.Node }).ToList();

            if (diffs.Count != 0)
            {
                var msg = diffs.ToDiffAssertMessage(expectedChange, actual.Control.Node, userMessage);
                Assert.True(diffs.Count == 0, msg);
            }
        }
    }
}
