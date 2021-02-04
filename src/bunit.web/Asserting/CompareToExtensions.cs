using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Bunit.Diffing;
using Bunit.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	/// <summary>
	/// Extension methods that allows different rendered fragments to be compared to others or strings.
	/// </summary>
	public static class CompareToExtensions
	{
		/// <summary>
		/// Compare the rendered markup in the <paramref name="actual"/> <see cref="IRenderedFragmentBase"/>
		/// with that in the <paramref name="expected"/> markup string.
		/// </summary>
		/// <param name="actual">Source of rendered markup to check.</param>
		/// <param name="expected">Markup to compare with.</param>
		/// <returns>Any differences found.</returns>
		public static IReadOnlyList<IDiff> CompareTo(this IRenderedFragment actual, string expected)
		{
			if (actual is null)
				throw new ArgumentNullException(nameof(actual));
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

			var htmlParser = actual.Services.GetRequiredService<BunitHtmlParser>();
			var expectedNodes = htmlParser.Parse(expected);

			return actual.Nodes.CompareTo(expectedNodes);
		}

		/// <summary>
		/// Compare the rendered markup in the <paramref name="actual"/> <see cref = "IRenderedFragmentBase" /> to
		/// the rendered markup in the <paramref name="expected"/> <see cref = "IRenderedFragmentBase" />.
		/// </summary>
		/// <param name="actual">Source of rendered markup to check.</param>
		/// <param name="expected">Source of rendered markup to compare with.</param>
		/// <returns>Any differences found.</returns>
		public static IReadOnlyList<IDiff> CompareTo(this IRenderedFragment actual, IRenderedFragment expected)
		{
			if (actual is null)
				throw new ArgumentNullException(nameof(actual));
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

			return actual.Nodes.CompareTo(expected.Nodes);
		}

		/// <summary>
		/// Compares an <paramref name="actual"/> <see cref="INode"/> with an <paramref name="expected"/>
		/// <see cref="INodeList"/>.
		/// </summary>
		/// <param name="actual">The node to check.</param>
		/// <param name="expected">The node list to compare with.</param>
		/// <returns>Any differences found.</returns>
		public static IReadOnlyList<IDiff> CompareTo(this INode actual, INodeList expected)
		{
			if (actual is null)
				throw new ArgumentNullException(nameof(actual));
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

			var comparer = actual.GetHtmlComparer()
				?? expected.GetHtmlComparer()
				?? new HtmlComparer();

			return comparer.Compare(expected, actual.AsEnumerable()).ToArray();
		}

		/// <summary>
		/// Compares an <paramref name="actual"/> <see cref="INodeList"/> with an <paramref name="expected"/>
		/// <see cref="INode"/>.
		/// </summary>
		/// <param name="actual">The node list to check.</param>
		/// <param name="expected">The node to compare with.</param>
		/// <returns>Any differences found.</returns>
		public static IReadOnlyList<IDiff> CompareTo(this INodeList actual, INode expected)
		{
			if (actual is null)
				throw new ArgumentNullException(nameof(actual));
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

			var comparer = expected.GetHtmlComparer()
				?? actual.GetHtmlComparer()
				?? new HtmlComparer();

			return comparer.Compare(expected.AsEnumerable(), actual).ToArray();
		}

		/// <summary>
		/// Compares an <paramref name="actual"/> <see cref="INodeList"/> with an <paramref name="expected"/>
		/// <see cref="INodeList"/>.
		/// </summary>
		/// <param name="actual">The node list to check.</param>
		/// <param name="expected">The node list to compare with.</param>
		/// <returns>Any differences found.</returns>
		public static IReadOnlyList<IDiff> CompareTo(this INodeList actual, INodeList expected)
		{
			if (actual is null)
				throw new ArgumentNullException(nameof(actual));
			if (expected is null)
				throw new ArgumentNullException(nameof(expected));

			if (actual.Length == 0 && expected.Length == 0)
				return Array.Empty<IDiff>();

			var comparer = actual.GetHtmlComparer()
				?? expected.GetHtmlComparer()
				?? new HtmlComparer();

			return comparer.Compare(expected, actual).ToArray();
		}
	}
}
