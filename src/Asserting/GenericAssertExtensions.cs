using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    public static class GenericAssertExtensions
    {
        public static void ShouldAllBe<T>(this IEnumerable<T> collection, params Action<T>[] elementInspectors)
        {
            Assert.Collection(collection, elementInspectors);
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
