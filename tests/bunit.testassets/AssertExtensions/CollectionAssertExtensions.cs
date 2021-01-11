using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace Bunit
{
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

		/// <summary>
		/// Verifies that a collection contains exactly a given number of elements, which
		/// meet the criteria provided by the element inspectors.
		/// </summary>
		/// <param name="collection">The collection to be inspected.</param>
		/// <param name="elementInspectors">The element inspectors, which inspect each element and its index in the collection in turn. The total number of element inspectors must exactly match the number of elements in the collection.</param>
		public static void ShouldAllBe<T>(this IEnumerable<T> collection, params Action<T, int>[] elementInspectors)
		{
			T[] elements = collection.ToArray();
			int expectedCount = elementInspectors.Length;
			int actualCount = elements.Length;

			if (expectedCount != actualCount)
				throw new CollectionException(collection, expectedCount, actualCount);

			for (int idx = 0; idx < actualCount; idx++)
			{
				try
				{
					elementInspectors[idx](elements[idx], idx);
				}
				catch (Exception ex)
				{
					throw new CollectionException(collection, expectedCount, actualCount, idx, ex);
				}
			}
		}
	}
}
