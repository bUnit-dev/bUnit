using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Bunit.Diffing;
using Xunit;
using Xunit.Sdk;

namespace Bunit
{
    /// <summary>
    /// Verification helpers for text
    /// </summary>
    public static class ShouldBeTextChangeAssertExtensions
    {
        /// <summary>
        /// Verifies that a list of diffs contains only a single change, and that change is a change to a text node.
        /// </summary>
        /// <param name="diffs">The list of diffs to verify against.</param>
        /// <param name="expectedChange">The expected text change.</param>
        /// <param name="userMessage">A custom error message to show if the verification fails.</param>
        public static void ShouldHaveSingleTextChange(this IReadOnlyList<IDiff> diffs, string expectedChange, string? userMessage = null)
        {
            DiffAssertExtensions.ShouldHaveSingleChange(diffs).ShouldBeTextChange(expectedChange, userMessage);
        }

        /// <summary>
        /// Verifies that a diff is a change to a text node.
        /// </summary>
        /// <param name="actualChange">The diff to verify.</param>
        /// <param name="expectedChange">The expected text change.</param>
        /// <param name="userMessage">A custom error message to show if the verification fails.</param>
        public static void ShouldBeTextChange(this IDiff actualChange, string expectedChange, string? userMessage = null)
        {
            if (actualChange is null) throw new ArgumentNullException(nameof(actualChange));
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));

            var actual = Assert.IsType<NodeDiff>(actualChange);
            var parser = actual.Control.Node.Owner.Context.GetService<TestHtmlParser>();
            var expected = parser.Parse(expectedChange);

            ShouldBeTextChange(actualChange, expected, userMessage);
        }

        /// <summary>
        /// Verifies that a diff is a change to a text node.
        /// </summary>
        /// <param name="actualChange">The diff to verify.</param>
        /// <param name="expectedChange">The rendered fragment containing the expected text change.</param>
        /// <param name="userMessage">A custom error message to show if the verification fails.</param>
        public static void ShouldBeTextChange(this IDiff actualChange, IRenderedFragment expectedChange, string? userMessage = null)
        {
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));
            ShouldBeTextChange(actualChange, expectedChange.Nodes, userMessage);
        }

        /// <summary>
        /// Verifies that a diff is a change to a text node.
        /// </summary>
        /// <param name="actualChange">The diff to verify.</param>
        /// <param name="expectedChange">The node list containing the expected text change.</param>
        /// <param name="userMessage">A custom error message to show if the verification fails.</param>
        public static void ShouldBeTextChange(this IDiff actualChange, INodeList expectedChange, string? userMessage = null)
        {
            if (actualChange is null) throw new ArgumentNullException(nameof(actualChange));
            if (expectedChange is null) throw new ArgumentNullException(nameof(expectedChange));

            var actual = Assert.IsType<NodeDiff>(actualChange);
            var comparer = actual.Control.Node.Owner.Context.GetService<HtmlComparer>();

            var diffs = comparer.Compare(expectedChange, new[] { actual.Test.Node }).ToList();

            if (diffs.Count != 0)
            {
                throw new HtmlEqualException(diffs, expectedChange, actual.Test.Node, userMessage);
            }
        }

        /// <summary>
        /// Verifies that the <paramref name="actualChange"/> diff is a change to the value of the specific attribute (<paramref name="expectedAttrName"/>).
        /// </summary>
        /// <param name="actualChange">The actual change that has happened.</param>
        /// <param name="expectedAttrName">The expected name of the changed attribute.</param>
        /// <param name="expectedAttrValue">The expected value of the changed attribute.</param>
        /// <param name="userMessage">A custom user message to show when the verification fails.</param>
        public static void ShouldBeAttributeChange(this IDiff actualChange, string expectedAttrName, string expectedAttrValue, string? userMessage = null)
        {
            if (actualChange is null) throw new ArgumentNullException(nameof(actualChange));
            if (expectedAttrName is null) throw new ArgumentNullException(nameof(expectedAttrName));
            if (expectedAttrValue is null) throw new ArgumentNullException(nameof(expectedAttrValue));

            var actual = Assert.IsType<AttrDiff>(actualChange);

            if (!expectedAttrName.Equals(actual.Test.Attribute.Name, StringComparison.Ordinal))
            {
                throw new AssertActualExpectedException(
                    expectedAttrName, actual.Test.Attribute.Name,
                    userMessage ?? "The name of the changed attribute does not match the expected name.",
                    "Expected attribute name",
                    "Actual attribute name");
            }
            if (!expectedAttrValue.Equals(actual.Test.Attribute.Value, StringComparison.Ordinal))
            {
                throw new AssertActualExpectedException(
                    expectedAttrValue, actual.Test.Attribute.Value,
                    userMessage ?? "The value of the changed attribute does not match the expected value.",
                    "Expected attribute value",
                    "Actual attribute value");
            }
        }
    }
}
