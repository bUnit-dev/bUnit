using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Xunit.Sdk;

namespace Bunit
{
    /// <summary>
    /// Helper methods for querying <see cref="IRenderedFragment"/>.
    /// </summary>
    public static class RenderedFragmentQueryExtensions
    {
        /// <summary>
        /// Returns the first element from the rendered fragment or component under test,
        /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal 
        /// of the rendered nodes.
        /// </summary>
        /// <param name="cssSelector">The group of selectors to use.</param>
        public static IElement Find(this IRenderedFragment renderedFragment, string cssSelector)
        {
            if (renderedFragment is null) throw new ArgumentNullException(nameof(renderedFragment));
            var result = renderedFragment.Nodes.QuerySelector(cssSelector);
            if (result is null)
                throw new ElementNotFoundException(cssSelector);
            else
                return result;
        }

        /// <summary>
        /// Returns a list of elements from the rendered fragment or component under test, 
        /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal 
        /// of the rendered nodes.
        /// </summary>
        /// <param name="cssSelector">The group of selectors to use.</param>
        public static IHtmlCollection<IElement> FindAll(this IRenderedFragment renderedFragment, string cssSelector)
        {
            if (renderedFragment is null) throw new ArgumentNullException(nameof(renderedFragment));
            return renderedFragment.Nodes.QuerySelectorAll(cssSelector);
        }
    }
}
