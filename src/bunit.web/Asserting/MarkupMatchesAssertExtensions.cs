using System;
using AngleSharp.Dom;
using Bunit.Diffing;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	/// <summary>
	/// Assert helpers for comparing markup
	/// </summary>
	public static class MarkupMatchesAssertExtensions
	{
		/// <summary>
		/// Verifies that the rendered markup from the <paramref name="actual"/> markup fragment matches
		/// the <paramref name="expected"/> markup fragment, using the <see cref="HtmlComparer"/> type.
		/// </summary>
		/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
		/// <param name="actual">The markup fragment to verify.</param>
		/// <param name="expected">The expected markup fragment.</param>
		/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
		public static void MarkupMatches(this string actual, string expected, string? userMessage = null)
		{
			using var parser = new HtmlParser();
			var actualNodes = parser.Parse(actual);
			var expectedNodes = parser.Parse(expected);
			actualNodes.MarkupMatches(expectedNodes, userMessage);
		}

		/// <summary>
		/// Verifies that the rendered markup from the <paramref name="actual"/> markup fragment matches
		/// the <paramref name="expected"/> <see cref="IRenderedFragmentBase"/>, using the <see cref="HtmlComparer"/> type.
		/// </summary>
		/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
		/// <param name="actual">The markup fragment to verify.</param>
		/// <param name="expected">The expected <see cref="IRenderedFragmentBase"/>.</param>
		/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
		public static void MarkupMatches(this string actual, IRenderedFragment expected, string? userMessage = null)
		{
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

			var actualNodes = actual.ToNodeList(expected.Services.GetRequiredService<HtmlParser>());
			actualNodes.MarkupMatches(expected, userMessage);
		}

		/// <summary>
		/// Verifies that the rendered markup from the <paramref name="actual"/> markup fragment matches
		/// the <paramref name="expected"/> <see cref="INodeList"/>, using the <see cref="HtmlComparer"/> type.
		/// </summary>
		/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
		/// <param name="actual">The markup fragment to verify.</param>
		/// <param name="expected">The expected <see cref="INodeList"/>.</param>
		/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
		public static void MarkupMatches(this string actual, INodeList expected, string? userMessage = null)
		{
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

			var actualNodes = actual.ToNodeList(expected.GetHtmlParser());
			actualNodes.MarkupMatches(expected, userMessage);
		}

		/// <summary>
		/// Verifies that the rendered markup from the <paramref name="actual"/> markup fragment matches
		/// the <paramref name="expected"/> <see cref="INode"/>, using the <see cref="HtmlComparer"/> type.
		/// </summary>
		/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
		/// <param name="actual">The markup fragment to verify.</param>
		/// <param name="expected">The expected <see cref="INode"/>.</param>
		/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
		public static void MarkupMatches(this string actual, INode expected, string? userMessage = null)
		{
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

			var actualNodes = actual.ToNodeList(expected.GetHtmlParser());
			actualNodes.MarkupMatches(expected, userMessage);
		}

		/// <summary>
		/// Verifies that the rendered markup from the <paramref name="actual"/> <see cref="IRenderedFragmentBase"/> matches
		/// the <paramref name="expected"/> markup, using the <see cref="HtmlComparer"/> type.
		/// </summary>
		/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
		/// <param name="actual">The rendered fragment to verify.</param>
		/// <param name="expected">The expected markup.</param>
		/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
		public static void MarkupMatches(this IRenderedFragment actual, string expected, string? userMessage = null)
		{
			if (actual is null)
				throw new ArgumentNullException(nameof(actual));
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

			var expectedNodes = expected.ToNodeList(actual.Services.GetRequiredService<HtmlParser>());
			actual.Nodes.MarkupMatches(expectedNodes, userMessage);
		}

		/// <summary>
		/// Verifies that the rendered markup from the <paramref name="actual"/> <see cref="IRenderedFragmentBase"/> matches
		/// the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragmentBase"/>, using the <see cref="HtmlComparer"/> type.
		/// </summary>
		/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
		/// <param name="actual">The rendered fragment to verify.</param>
		/// <param name="expected">The expected rendered fragment.</param>
		/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
		public static void MarkupMatches(this IRenderedFragment actual, IRenderedFragment expected, string? userMessage = null)
		{
			if (actual is null)
				throw new ArgumentNullException(nameof(actual));
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

			actual.Nodes.MarkupMatches(expected.Nodes, userMessage);
		}

		/// <summary>
		/// Verifies that the <paramref name="actual"/> <see cref="INodeList"/> matches
		/// the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragmentBase"/>, using the <see cref="HtmlComparer"/> 
		/// type.
		/// </summary>
		/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
		/// <param name="actual">The list of nodes to verify.</param>
		/// <param name="expected">The expected rendered fragment.</param>
		/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
		public static void MarkupMatches(this INodeList actual, IRenderedFragment expected, string? userMessage = null)
		{
			if (actual is null)
				throw new ArgumentNullException(nameof(actual));
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

			actual.MarkupMatches(expected.Nodes, userMessage);
		}

		/// <summary>
		/// Verifies that the <paramref name="actual"/> <see cref="INode"/> matches
		/// the rendered markup from the <paramref name="expected"/> <see cref="IRenderedFragmentBase"/>, using the <see cref="HtmlComparer"/> 
		/// type.
		/// </summary>
		/// <exception cref="HtmlEqualException">Thrown when the <paramref name="actual"/> markup does not match the <paramref name="expected"/> markup.</exception>
		/// <param name="actual">The node to verify.</param>
		/// <param name="expected">The expected rendered fragment.</param>
		/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
		public static void MarkupMatches(this INode actual, IRenderedFragment expected, string? userMessage = null)
		{
			if (actual is null)
				throw new ArgumentNullException(nameof(actual));
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

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
			if (actual is null)
				throw new ArgumentNullException(nameof(actual));

			var expectedNodes = expected.ToNodeList(actual.GetHtmlParser());
			actual.MarkupMatches(expectedNodes, userMessage);
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
			if (actual is null)
				throw new ArgumentNullException(nameof(actual));

			var expectedNodes = expected.ToNodeList(actual.GetHtmlParser());
			actual.MarkupMatches(expectedNodes, userMessage);
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

		private static INodeList ToNodeList(this string markup, HtmlParser? htmlParser)
		{
			if (htmlParser is null)
			{
				using var newHtmlParser = new HtmlParser();
				return newHtmlParser.Parse(markup);
			}
			else
			{
				return htmlParser.Parse(markup);
			}
		}
	}
}
