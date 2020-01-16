using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing.Asserting
{
    /// <summary>
    /// Collection test assertions
    /// </summary>
    public static class GenericAssertExtensions
    {
        /// <summary>
        /// Verifies that <paramref name="actual"/> is not null
        /// and returns <paramref name="actual"/> again.
        /// </summary>
        /// <returns>Returns <paramref name="actual"/> if it is not null.</returns>
        public static T ShouldNotBeNull<T>([NotNullIfNotNull("actual")]this T? actual) where T : class
        {
            if (actual is null)
                throw new XunitException($"{nameof(ShouldNotBeNull)}() Failure");
            return actual;
        }

        /// <summary>
        /// Verifies that <paramref name="actual"/> is not null
        /// and returns <paramref name="actual"/> again.
        /// </summary>
        /// <returns>Returns <paramref name="actual"/> if it is not null.</returns>
        public static T ShouldNotBeNull<T>([NotNullIfNotNull("actual")]this T? actual) where T : struct
        {
            if (actual is null)
                throw new XunitException($"{nameof(ShouldNotBeNull)}() Failure");
            return actual.Value;
        }

        /// <summary>
        /// Verifies that a nullable <paramref name="actual"/> is not null
        /// and of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>Returns <paramref name="actual"/> as <typeparamref name="T"/>.</returns>
        public static T ShouldBeOfType<T>([NotNullIfNotNull("actual")]this object? actual)
        {
            return Assert.IsType<T>(actual);
        }

        /// <summary>
        /// Verifies that a non nullable struct is the same as its nullable counter part.
        /// </summary>
        public static void ShouldBe<T>(this T actual, T? expected)
             where T : struct
        {
            Assert.Equal(expected, actual);
        }
    }

    /// <summary>
    /// Collection test assertions
    /// </summary>
    public static class CollectionAssertExtensions
    {


        /// <summary>
        /// Verifies that a collection contains exactly a given number of elements, which
        /// meet the criteria provided by the element inspectors.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection to be inspected</param>
        /// <param name="elementInspectors">The element inspectors, which inspect each element in turn. The total number of element inspectors must exactly match the number of elements in the collection.</param>
        public static void ShouldAllBe<T>(this IEnumerable<T> collection, params Action<T>[] elementInspectors)
        {
            Assert.Collection(collection, elementInspectors);
        }

        /// <summary>
        /// Verifies that a collection contains exactly a given number of elements, which
        /// meet the criteria provided by the element inspectors.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection to be inspected</param>
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
