using System.Globalization;
using AngleSharp.Diffing.Core;
using Bunit.Asserting;

namespace Bunit;

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

		return diffs.FirstOrDefault()
			?? throw new ActualExpectedAssertException(
				actual: "0",
				expected: "1",
				actualText: "Actual changes",
				expectedText: "Expected changes",
				message: "There were more than one change");
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
		if (diffInspectors is null)
			throw new ArgumentNullException(nameof(diffInspectors));

		var diffCount = 0;
		foreach (var diff in diffs)
		{
			if (diffCount >= diffInspectors.Length)
			{
				break;
			}

			diffInspectors[diffCount](diff);
			diffCount++;
		}

		if (diffCount != diffInspectors.Length)
		{
			throw new ActualExpectedAssertException(
				actual: diffCount.ToString(CultureInfo.InvariantCulture),
				expected: diffInspectors.Length.ToString(CultureInfo.InvariantCulture),
				actualText: "Actual changes",
				expectedText: "Expected changes",
				message: "The actual number of changes does not match the expected.");
		}
	}
}
