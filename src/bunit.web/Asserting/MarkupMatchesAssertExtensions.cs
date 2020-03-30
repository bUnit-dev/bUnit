using System;
using AngleSharp.Dom;
using Bunit.Diffing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;

namespace Bunit
{
    /// <summary>
    /// Assert helpers for comparing markup
    /// </summary>
    public static class MarkupMatchesAssertExtensions
    {

        /// <summary>
        /// Verifies that the rendered markup from the <paramref name="actual"/> <see cref="IRenderedFragment"/> matches
        /// the <paramref name="expected"/> markup, using the <see cref="HtmlComparer"/> type.
        /// </summary>
        /// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
        /// <param name="actual">The rendered fragment to verify.</param>
        /// <param name="expected">The expected markup.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void MarkupMatches(this IRenderedFragment actual, string expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

			var htmlParser = actual.Services.GetRequiredService<TestHtmlParser>();
            var expectedNodes = htmlParser.Parse(expected);

            actual.Nodes.MarkupMatches(expectedNodes, userMessage);
        }

        /// <summary>
        /// Verifies that the rendered markup from the <paramref name="actual"/> <see cref="IRenderedFragment"/> matches
        /// the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragment"/>, using the <see cref="HtmlComparer"/> type.
        /// </summary>
        /// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
        /// <param name="actual">The rendered fragment to verify.</param>
        /// <param name="expected">The expected rendered fragment.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void MarkupMatches(this IRenderedFragment actual, IRenderedFragment expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            actual.Nodes.MarkupMatches(expected.Nodes, userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> matches
        /// the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragment"/>, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
        /// <param name="actual">The list of nodes to verify.</param>
        /// <param name="expected">The expected rendered fragment.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void MarkupMatches(this INodeList actual, IRenderedFragment expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            actual.MarkupMatches(expected.Nodes, userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INode"/> matches
        /// the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragment"/>, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
        /// <param name="actual">The node to verify.</param>
        /// <param name="expected">The expected rendered fragment.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void MarkupMatches(this INode actual, IRenderedFragment expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));
            if (expected is null) throw new ArgumentNullException(nameof(expected));

            actual.MarkupMatches(expected.Nodes, userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INode"/> matches
        /// the <paramref name="expected"/> markup, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
        /// <param name="actual">The node to verify.</param>
        /// <param name="expected">The expected markup.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void MarkupMatches(this INode actual, string expected, string? userMessage = null)
        {
            if (actual is null) throw new ArgumentNullException(nameof(actual));

            INodeList expectedNodes;
            if (actual.GetHtmlParser() is { } parser)
            {
                expectedNodes = parser.Parse(expected);
            }
            else
            {
                using var newParser = new TestHtmlParser();
                expectedNodes = newParser.Parse(expected);
            }
            MarkupMatches(actual, expectedNodes, userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> matches
        /// the <paramref name="expected"/> markup, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
        /// <param name="actual">The list of nodes to verify.</param>
        /// <param name="expected">The expected markup.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void MarkupMatches(this INodeList actual, string expected, string? userMessage = null)
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
            MarkupMatches(actual, expectedNodes, userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> matches
        /// the <paramref name="expected"/> <see cref="INodeList"/>, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
        /// <param name="actual">The list of nodes to verify.</param>
        /// <param name="expected">The expected list of nodes.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void MarkupMatches(this INodeList actual, INodeList expected, string? userMessage = null)
        {
            var diffs = actual.CompareTo(expected);

            if (diffs.Count != 0)
                throw new HtmlEqualException(diffs, expected, actual, userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> matches
        /// the <paramref name="expected"/> <see cref="INode"/>, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
        /// <param name="actual">The list of nodes to verify.</param>
        /// <param name="expected">The expected node.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void MarkupMatches(this INodeList actual, INode expected, string? userMessage = null)
        {
            var diffs = actual.CompareTo(expected);

            if (diffs.Count != 0)
                throw new HtmlEqualException(diffs, expected, actual, userMessage);
        }

        /// <summary>
        /// Verifies that the <paramref name="actual"/> <see cref="INode"/> matches
        /// the <paramref name="expected"/> <see cref="INodeList"/>, using the <see cref="HtmlComparer"/> 
        /// type.
        /// </summary>
        /// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
        /// <param name="actual">The node to verify.</param>
        /// <param name="expected">The expected list of nodes.</param>
        /// <param name="userMessage">A custom user message to display in case the verification fails.</param>
        public static void MarkupMatches(this INode actual, INodeList expected, string? userMessage = null)
        {
            var diffs = actual.CompareTo(expected);

            if (diffs.Count != 0)
                throw new HtmlEqualException(diffs, expected, actual, userMessage);
        }
    }
}
