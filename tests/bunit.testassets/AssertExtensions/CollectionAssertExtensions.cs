using Xunit;
using Xunit.Sdk;

namespace Bunit;

/// <summary>
/// Collection test assertions.
/// </summary>
public static class CollectionAssertExtensions
{
	/// <summary>
	/// Verifies that a collection contains exactly a given number of elements, which
	/// meet the criteria provided by the element inspectors.
	/// </summary>
	/// <param name="collection">The collection to be inspected.</param>
	/// <param name="elementInspectors">The element inspectors, which inspect each element in turn. The total number of element inspectors must exactly match the number of elements in the collection.</param>
	public static void ShouldAllBe<T>(this IEnumerable<T> collection, params Action<T>[] elementInspectors)
	{
		Assert.Collection(collection, elementInspectors);
	}
}
