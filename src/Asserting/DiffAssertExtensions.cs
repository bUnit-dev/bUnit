using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Diffing;
using Egil.RazorComponents.Testing.Extensions;
using Xunit;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// A collection of <see cref="IDiff"/> assert extensions and generic assert extensions
    /// </summary>
    public static class DiffAssertExtensions
    {
        /// <summary>
        /// Verifies that a collection of <see cref="IDiff"/>s contains exactly one <see cref="IDiff"/>.
        /// </summary>
        /// <param name="diffs">The collection to be inspected</param>
        /// <returns>The expected single <see cref="IDiff"/> in the collection.</returns>
        public static IDiff ShouldHaveSingleChange(this IReadOnlyList<IDiff> diffs)
        {
            if (diffs is null) throw new ArgumentNullException(nameof(diffs));
            Assert.Equal(1, diffs.Count);
            return diffs[0];
        }

        /// <summary>
        /// Verifies that a collection of <see cref="IDiff"/>s contains exactly a given number of elements, which 
        /// meet the criteria provided by the <see cref="IDiff"/> inspectors
        /// </summary>
        /// <param name="diffs">The collection to be inspected</param>
        /// <param name="diffInspectors">The <see cref="IDiff"/> inspectors, which inspect each <see cref="IDiff"/> in turn. 
        /// The total number of <see cref="IDiff"/> inspectors must exactly match the number of <see cref="IDiff"/>s in the collection</param>
        public static void ShouldHaveChanges(this IReadOnlyList<IDiff> diffs, params Action<IDiff>[] diffInspectors)
        {
            Assert.Collection(diffs, diffInspectors);
        }

        /// <summary>
        /// Verifies that the rendered markup from the <paramref name="actual"/> <see cref="IRenderedFragment"/> is equal
        /// to the <paramref name="expected"/> markup, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <param name="actual">The rendered fragment to verify.</param>
        /// <param name="expected">The expected markup.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void ShouldBe(this IRenderedFragment actual, string expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            var actualNodes = actual.GetNodes();
            var expectedNodes = actual.TestContext.HtmlParser.Parse(expected);

            actualNodes.ShouldBe(expectedNodes, userMessage);
        }

        /// <summary>
        /// Verifies that the rendered markup from the <paramref name="actual"/> <see cref="IRenderedFragment"/> is equal
        /// to the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragment"/>, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <param name="actual">The rendered fragment to verify.</param>
        /// <param name="expected">The expected rendered fragment.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void ShouldBe(this IRenderedFragment actual, IRenderedFragment expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            actual.GetNodes().ShouldBe(expected.GetNodes(), userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> is equal
        /// to the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragment"/>, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <param name="actual">The list of nodes to verify.</param>
        /// <param name="expected">The expected rendered fragment.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void ShouldBe(this INodeList actual, IRenderedFragment expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            actual.ShouldBe(expected.GetNodes(), userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INode"/> is equal
        /// to the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragment"/>, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <param name="actual">The node to verify.</param>
        /// <param name="expected">The expected rendered fragment.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void ShouldBe(this INode actual, IRenderedFragment expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            actual.ShouldBe(expected.GetNodes(), userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> is equal
        /// to the <paramref name="expected"/> markup, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <param name="actual">The list of nodes to verify.</param>
        /// <param name="expected">The expected markup.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void ShouldBe(this INodeList actual, string expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));

            INodeList expectedNodes;
            if (actual.Length > 0 && actual[0].GetHtmlParser() is { } parser)
            {
                expectedNodes = parser.Parse(expected);
            }
            else
            {
                using var newParser = new TestHtmlParser();
                expectedNodes = newParser.Parse(expected);
            }
            ShouldBe(actual, expectedNodes, userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> is equal
        /// to the <paramref name="expected"/> <see cref="INodeList"/>, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <param name="actual">The list of nodes to verify.</param>
        /// <param name="expected">The expected list of nodes.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void ShouldBe(this INodeList actual, INodeList expected, string? userMessage = null)
        {
            var diffs = actual.CompareTo(expected);

            if (diffs.Count != 0)
                throw new HtmlEqualException(diffs, expected, actual, userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> is equal
        /// to the <paramref name="expected"/> <see cref="INode"/>, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <param name="actual">The list of nodes to verify.</param>
        /// <param name="expected">The expected node.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void ShouldBe(this INodeList actual, INode expected, string? userMessage = null)
        {
            var diffs = actual.CompareTo(expected);

            if (diffs.Count != 0)
                throw new HtmlEqualException(diffs, expected, actual, userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INode"/> is equal
        /// to the <paramref name="expected"/> <see cref="INodeList"/>, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <param name="actual">The node to verify.</param>
        /// <param name="expected">The expected list of nodes.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>

        public static void ShouldBe(this INode actual, INodeList expected, string? userMessage = null)
        {
            var diffs = actual.CompareTo(expected);

            if (diffs.Count != 0)
                throw new HtmlEqualException(diffs, expected, actual, userMessage);
        }
    }
}
