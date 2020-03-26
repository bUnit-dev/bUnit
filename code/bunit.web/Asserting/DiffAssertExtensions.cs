using System;
using System.Collections.Generic;
using AngleSharp.Diffing.Core;
using Bunit.Asserting;
using Xunit;

namespace Bunit
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
			if (diffs is null)
				throw new ArgumentNullException(nameof(diffs));
			if (diffs.Count != 1)
				throw new ActualExpectedAssertException(diffs.Count.ToString(), "1", "Actual changes", "Expected changes", "There were more than one change");

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
			if (diffs is null)
				throw new ArgumentNullException(nameof(diffs));

			if (diffs.Count != diffInspectors.Length)
				throw new ActualExpectedAssertException(diffs.Count.ToString(), diffInspectors.Length.ToString(), "Actual changes", "Expected changes", "The actual number of changes does not match the expected.");

			for (int i = 0; i < diffInspectors.Length; i++)
			{
				diffInspectors[i](diffs[i]);
			}
		}

	}
}
