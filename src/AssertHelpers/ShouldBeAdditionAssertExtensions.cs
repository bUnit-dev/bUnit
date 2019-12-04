using System;
using System.Linq;
using AngleSharp;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Diffing;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    public static class ShouldBeAdditionAssertExtensions
    {
        public static void ShouldBeAddition(this IDiff actualChange, string expectedChange, string? userMessage = null)
        {
            if (actualChange is null) throw new ArgumentNullException(nameof(actualChange));
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));

            var actual = Assert.IsType<UnexpectedNodeDiff>(actualChange);
            var parser = actual.Test.Node.Owner.Context.GetService<HtmlParser>();
            var expected = parser.Parse(expectedChange);

            ShouldBeAddition(actualChange, expected, userMessage);
        }

        public static void ShouldBeAddition(this IDiff actualChange, IRenderedFragment expectedChange, string? userMessage = null)
        {
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));
            ShouldBeAddition(actualChange, expectedChange.GetNodes(), userMessage);
        }

        public static void ShouldBeAddition(this IDiff actualChange, INodeList expectedChange, string? userMessage = null)
        {
            if (actualChange is null) throw new ArgumentNullException(nameof(actualChange));
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));

            var actual = Assert.IsType<UnexpectedNodeDiff>(actualChange);
            var comparer = actual.Test.Node.Owner.Context.GetService<HtmlComparer>();

            var diffs = comparer.Compare(expectedChange, new[] { actual.Test.Node }).ToList();

            if (diffs.Count != 0)
            {
                var msg = diffs.ToDiffAssertMessage(expectedChange.ToHtml(), actual.Test.Node.ToHtml(), userMessage);
                Assert.True(diffs.Count == 0, msg);
            }
        }
    }
}
