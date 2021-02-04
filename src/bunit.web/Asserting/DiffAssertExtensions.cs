using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AngleSharp.Diffing.Core;
using Bunit.Asserting;

namespace Bunit
{
	/// <summary>
	/// A collection of <see cref="IDiff"/> assert extensions and generic assert extensions.
	/// </summary>
	public static class DiffAssertExtensions
	{
		/// <summary>
		/// Verifies that a collection of <see cref="IDiff"/>s contains exactly one <see cref="IDiff"/>.
		/// </summary>
		/// <param name="diffs">The collection to be inspected.</param>
		/// <returns>The expected single <see cref="IDiff"/> in the collection.</returns>
		public static IDiff ShouldHaveSingleChange(this IEnumerable<IDiff> diffs)
		{
			if (diffs is null)
				throw new ArgumentNullException(nameof(diffs));
			if (diffs.Take(2).Count() != 1) // Optimized way of writing "diffs.Count() != 1"
				throw new ActualExpectedAssertException(diffs.Count().ToString(CultureInfo.InvariantCulture), "1", "Actual changes", "Expected changes", "There were more than one change");

			return diffs.First();
		}

		/// <summary>
		/// Verifies that a collection of <see cref="IDiff"/>s contains exactly a given number of elements, which
		/// meet the criteria provided by the <see cref="IDiff"/> inspectors.
		/// </summary>
		/// <param name="diffs">The collection to be inspected.</param>
		/// <param name="diffInspectors">The <see cref="IDiff"/> inspectors, which inspect each <see cref="IDiff"/> in turn.
		/// The total number of <see cref="IDiff"/> inspectors must exactly match the number of <see cref="IDiff"/>s in the collection.</param>
		public static void ShouldHaveChanges(this IEnumerable<IDiff> diffs, params Action<IDiff>[] diffInspectors)
		{
			if (diffs is null)
				throw new ArgumentNullException(nameof(diffs));
			if (diffs.Take(diffInspectors.Length + 1).Count() != diffInspectors.Length) // Optimized way of writing "diffs.Count() != diffInspectors.Length"
				throw new ActualExpectedAssertException(diffs.Count().ToString(CultureInfo.InvariantCulture), diffInspectors.Length.ToString(CultureInfo.InvariantCulture), "Actual changes", "Expected changes", "The actual number of changes does not match the expected.");

			int index = 0;
			foreach (var diff in diffs)
			{
				diffInspectors[index](diff);
				index++;
			}
		}
	}
}
