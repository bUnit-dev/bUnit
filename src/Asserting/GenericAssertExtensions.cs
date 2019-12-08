using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing
{
    public static class GenericAssertExtensions
    {
        public static void ShouldAllBe<T>(this IEnumerable<T> collection, params Action<T>[] elementInspectors)
        {
            Assert.Collection(collection, elementInspectors);
        }

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

        public static T ShouldNotBeNull<T>(this T? actual) where T : class
        {
            if (actual is { })
            {
                return actual;
            }

            Assert.NotNull(actual);
            return default!; // BANG here because assert will throw and we will never reach this.
        }
    }
}
