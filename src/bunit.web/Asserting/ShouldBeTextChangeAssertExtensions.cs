using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Bunit.Asserting;
using Bunit.Diffing;
using Bunit.Rendering;

namespace Bunit
{
	/// <summary>
	/// Verification helpers for text.
	/// </summary>
	public static class ShouldBeTextChangeAssertExtensions
	{
		/// <summary>
		/// Verifies that a list of diffs contains only a single change, and that change is a change to a text node.
		/// </summary>
		/// <param name="diffs">The list of diffs to verify against.</param>
		/// <param name="expectedChange">The expected text change.</param>
		/// <param name="userMessage">A custom error message to show if the verification fails.</param>
		public static void ShouldHaveSingleTextChange(this IEnumerable<IDiff> diffs, string expectedChange, string? userMessage = null)
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
			if (actualChange is null)
				throw new ArgumentNullException(nameof(actualChange));
			if (expectedChange is null)
				throw new ArgumentNullException(nameof(expectedChange));
			if (actualChange is not NodeDiff actual)
				throw new DiffChangeAssertException(actualChange.Result, DiffResult.Different, "The change was not a text change.");

			var parser = actual.Control.Node.Owner.Context.GetService<BunitHtmlParser>();
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
			if (expectedChange is null)
				throw new ArgumentNullException(nameof(expectedChange));
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
			if (actualChange is null)
				throw new ArgumentNullException(nameof(actualChange));
			if (expectedChange is null)
				throw new ArgumentNullException(nameof(expectedChange));
			if (actualChange is not NodeDiff actual)
				throw new DiffChangeAssertException(actualChange.Result, DiffResult.Different, "The change was not a text change.");

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
			if (actualChange is null)
				throw new ArgumentNullException(nameof(actualChange));
			if (expectedAttrName is null)
				throw new ArgumentNullException(nameof(expectedAttrName));
			if (expectedAttrValue is null)
				throw new ArgumentNullException(nameof(expectedAttrValue));
			if (actualChange is not AttrDiff actual)
				throw new DiffChangeAssertException(actualChange.Result, DiffResult.Different, "The change was not a attribute change.");

			if (!expectedAttrName.Equals(actual.Test.Attribute.Name, StringComparison.Ordinal))
			{
				throw new ActualExpectedAssertException(
					actual.Test.Attribute.Name,
					expectedAttrName,
					"Actual attribute name",
					"Expected attribute name",
					userMessage ?? "The name of the changed attribute does not match the expected name.");
			}

			if (!expectedAttrValue.Equals(actual.Test.Attribute.Value, StringComparison.Ordinal))
			{
				throw new ActualExpectedAssertException(
					actual.Test.Attribute.Value,
					expectedAttrValue,
					"Actual attribute value",
					"Expected attribute value",
					userMessage ?? "The value of the changed attribute does not match the expected value.");
			}
		}
	}
}
