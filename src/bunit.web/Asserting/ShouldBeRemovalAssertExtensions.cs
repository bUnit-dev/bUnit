using System;
using System.Linq;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Bunit.Diffing;
using Bunit.Rendering;

namespace Bunit
{
	/// <summary>
	/// A set of removal diff assert extensions.
	/// </summary>
	public static class ShouldBeRemovalAssertExtensions
	{
		/// <summary>
		/// Verifies that the <paramref name="actualChange"/> <see cref="IDiff"/> is an removal,
		/// i.e. that one or more nodes have been removed, and verifies that the removed nodes are equal
		/// to the markup specified in the <paramref name="expectedChange"/> input.
		/// </summary>
		/// <param name="actualChange">The change to verify.</param>
		/// <param name="expectedChange">The expected removal to verify against.</param>
		/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
		public static void ShouldBeRemoval(this IDiff actualChange, string expectedChange, string? userMessage = null)
		{
			if (actualChange is null)
				throw new ArgumentNullException(nameof(actualChange));
			if (expectedChange is null)
				throw new ArgumentNullException(nameof(expectedChange));
			if (actualChange is not MissingNodeDiff actual)
				throw new DiffChangeAssertException(actualChange.Result, DiffResult.Missing, "The change was not an removal.");

			INodeList expected;
			if (actual.Control.Node.GetHtmlParser() is BunitHtmlParser parser)
			{
				expected = parser.Parse(expectedChange);
			}
			else
			{
				using var newParser = new BunitHtmlParser();
				expected = newParser.Parse(expectedChange);
			}

			ShouldBeRemoval(actualChange, expected, userMessage);
		}

		/// <summary>
		/// Verifies that the <paramref name="actualChange"/> <see cref="IDiff"/> is an removal,
		/// i.e. that one or more nodes have been removed, and verifies that the removed nodes are equal
		/// to the rendered markup from the <paramref name="expectedChange"/> <see cref="IRenderedFragmentBase"/>.
		/// </summary>
		/// <param name="actualChange">The change to verify.</param>
		/// <param name="expectedChange">The expected removal to verify against.</param>
		/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
		public static void ShouldBeRemoval(this IDiff actualChange, IRenderedFragment expectedChange, string? userMessage = null)
		{
			if (expectedChange is null)
				throw new ArgumentNullException(nameof(expectedChange));
			ShouldBeRemoval(actualChange, expectedChange.Nodes, userMessage);
		}

		/// <summary>
		/// Verifies that the <paramref name="actualChange"/> <see cref="IDiff"/> is an removal,
		/// i.e. that one or more nodes have been removed, and verifies that the removed nodes are equal
		/// to the <paramref name="expectedChange"/> nodes.
		/// </summary>
		/// <param name="actualChange">The change to verify.</param>
		/// <param name="expectedChange">The expected removal to verify against.</param>
		/// <param name="userMessage">A custom user message to display in case the verification fails.</param>
		public static void ShouldBeRemoval(this IDiff actualChange, INodeList expectedChange, string? userMessage = null)
		{
			if (actualChange is null)
				throw new ArgumentNullException(nameof(actualChange));
			if (expectedChange is null)
				throw new ArgumentNullException(nameof(expectedChange));
			if (actualChange is not MissingNodeDiff actual)
				throw new DiffChangeAssertException(actualChange.Result, DiffResult.Missing, "The change was not an removal.");

			var comparer = actual.Control.Node.Owner.Context.GetService<HtmlComparer>();

			var diffs = comparer.Compare(expectedChange, new[] { actual.Control.Node }).ToList();

			if (diffs.Count != 0)
			{
				throw new HtmlEqualException(diffs, expectedChange, actual.Control.Node, userMessage);
			}
		}
	}
}
