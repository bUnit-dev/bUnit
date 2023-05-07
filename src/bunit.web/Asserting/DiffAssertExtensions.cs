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
	[Obsolete("Will be removed in v2.")]
	public static IDiff ShouldHaveSingleChange(this IEnumerable<IDiff> diffs)
	{
		if (diffs is null)
			throw new ArgumentNullException(nameof(diffs));

		var diffsArray = diffs.ToArray();

		if (diffsArray.Length != 1)
			throw new ActualExpectedAssertException(
				actual: diffsArray.Length.ToString(CultureInfo.InvariantCulture),
				expected: "1",
				actualText: "Actual changes",
				expectedText: "Expected changes",
				message: "There were more than one change");

		return diffsArray.First();
	}

	/// <summary>
	/// Verifies that a collection of <see cref="IDiff"/>s contains exactly a given number of elements, which
	/// meet the criteria provided by the <see cref="IDiff"/> inspectors.
	/// </summary>
	/// <param name="diffs">The collection to be inspected.</param>
	/// <param name="diffInspectors">The <see cref="IDiff"/> inspectors, which inspect each <see cref="IDiff"/> in turn.
	/// The total number of <see cref="IDiff"/> inspectors must exactly match the number of <see cref="IDiff"/>s in the collection.</param>
	[Obsolete("Will be removed in v2.")]
	public static void ShouldHaveChanges(this IEnumerable<IDiff> diffs, params Action<IDiff>[] diffInspectors)
	{
		if (diffs is null)
			throw new ArgumentNullException(nameof(diffs));
		if (diffInspectors is null)
			throw new ArgumentNullException(nameof(diffInspectors));

		var diffsArray = diffs.ToArray();

		if (diffsArray.Length != diffInspectors.Length)
			throw new ActualExpectedAssertException(
				actual: diffsArray.Length.ToString(CultureInfo.InvariantCulture),
				expected: diffInspectors.Length.ToString(CultureInfo.InvariantCulture),
				actualText: "Actual changes",
				expectedText: "Expected changes",
				message: "The actual number of changes does not match the expected.");

		var index = 0;
		foreach (var diff in diffsArray)
		{
			diffInspectors[index](diff);
			index++;
		}
	}
}
