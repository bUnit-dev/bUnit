using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.RazorComponents.Testing.TestUtililities
{
    /// <summary>
    /// Assert helpers
    /// </summary>
    public static class AssertExtensions
    {
        /// <summary>
        /// Verifies that a collection produced by the provided factory is empty.
        /// </summary>
        /// <param name="collectionFactory">The factory which produces the collection to verify.</param>
        public static void ShouldBeEmpty(this Func<System.Collections.IEnumerable> collectionFactory)
        {
            if (collectionFactory is null) throw new ArgumentNullException(nameof(collectionFactory));
            Assert.Empty(collectionFactory());
        }

        /// <summary>
        /// Verifies that a collection produced by the provided factory contains exactly a 
        /// given number of elements, which meet the criteria provided by the element inspectors.
        /// </summary>
        /// <typeparam name="T">Type of object in the in the collection</typeparam>
        /// <param name="collectionFactory">The factory which produces the collection to verify.</param>
        /// <param name="elementInspectors">The element inspectors, which inspect each element in turn. The total number of element inspectors must exactly match the number of elements in the collection.</param>
        public static void ShouldAllBe<T>(this Func<IEnumerable<T>> collectionFactory, params Action<T>[] elementInspectors)
        {
            if (collectionFactory is null) throw new ArgumentNullException(nameof(collectionFactory));
            Assert.Collection(collectionFactory(), elementInspectors);
        }
    }
}
