using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Diffing;
using Egil.RazorComponents.Testing.Extensions;

namespace Egil.RazorComponents.Testing.Asserting
{
    public static class CompareToExtensions
    {
        public static IReadOnlyList<IDiff> CompareTo(this IRenderedFragment actual, string expected)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            var actualNodes = actual.GetNodes();
            var expectedNodes = actual.TestContext.HtmlParser.Parse(expected);

            return actualNodes.CompareTo(expectedNodes);
        }

        public static IReadOnlyList<IDiff> CompareTo(this IRenderedFragment actual, IRenderedFragment expected)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            return actual.GetNodes().CompareTo(expected.GetNodes());
        }

        public static IReadOnlyList<IDiff> CompareTo(this INode actual, INodeList expected)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            var comparer = actual.GetHtmlComparer()
                ?? expected.GetHtmlComparer()
                ?? new HtmlComparer();

            return comparer.Compare(expected, actual.AsEnumerable()).ToArray();
        }

        public static IReadOnlyList<IDiff> CompareTo(this INodeList actual, INode expected)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            var comparer = expected.GetHtmlComparer()
                ?? actual.GetHtmlComparer()
                ?? new HtmlComparer();

            return comparer.Compare(expected.AsEnumerable(), actual).ToArray();
        }

        public static IReadOnlyList<IDiff> CompareTo(this INodeList actual, INodeList expected)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            if (actual.Length == 0 && expected.Length == 0) return Array.Empty<IDiff>();

            var comparer = actual.GetHtmlComparer()
                ?? expected.GetHtmlComparer()
                ?? new HtmlComparer();

            return comparer.Compare(expected, actual).ToArray();
        }
    }
}
