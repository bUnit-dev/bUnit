using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Asserting;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    public static class DiffAssertExtensions
    {
        public static IDiff ShouldHaveSingleChange(this IReadOnlyList<IDiff> diffs)
        {
            if (diffs is null) throw new ArgumentNullException(nameof(diffs));
            Assert.Equal(1, diffs.Count);
            return diffs[0];
        }

        public static void ShouldHaveChanges(this IReadOnlyList<IDiff> diffs, params Action<IDiff>[] expectedDiffAsserts)
        {
            Assert.Collection(diffs, expectedDiffAsserts);
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
            var diffs = actual.CompareTo(expected);

            if (diffs.Count != 0) 
                HtmlEqualException.ThrowHtmlEqualException(diffs, expected, actual, userMessage);
        }
    }
}
