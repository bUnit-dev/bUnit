using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharpWrappers;

namespace Bunit
{
    /// <summary>
    /// Helper methods for querying <see cref="IElement"/> types.
    /// </summary>
    public static class ElementQueryExtensions
    {
        /// <summary>
        /// Returns the first element within this element (using depth-first pre-order traversal
        /// of the document's nodes) that matches the specified group of selectors.
        /// </summary>
        /// <param name="selector">The group of selectors to use.</param>
        /// <param name="element">The element to search within</param>
        public static IElement Find(this IElement element, string selector)
        {
            if (element is null) throw new ArgumentNullException(nameof(element));

            return element.QuerySelector(selector);            
        }

        /// <summary>
        /// Returns a list of the elements within the rendered fragment or component under test, 
        /// (using depth-first pre-order traversal of the document's nodes) that match the specified group of selectors.
        /// </summary>
        /// <param name="selector">The group of selectors to use.</param>
        /// <param name="element">The element to search within</param>
        public static IHtmlCollection<IElement> FindAll(this IElement element, string selector)
        {
            if (element is null) throw new ArgumentNullException(nameof(element));
            return element.QuerySelectorAll(selector);
        }

        /// <summary>
        /// Returns the first element within this element (using depth-first pre-order traversal
        /// of the document's nodes) that matches the specified group of selectors.
        /// </summary>
        /// <param name="selector">The group of selectors to use.</param>
        /// <param name="nodelist">The elements to search within</param>
        public static IElement Find(this INodeList nodelist, string selector)
        {
            if (nodelist is null) throw new ArgumentNullException(nameof(nodelist));
            return nodelist.QuerySelector(selector);
        }

        /// <summary>
        /// Returns a list of the elements within the rendered fragment or component under test, 
        /// (using depth-first pre-order traversal of the document's nodes) that match the specified group of selectors.
        /// </summary>
        /// <param name="selector">The group of selectors to use.</param>
        /// <param name="nodelist">The elements to search within</param>
        public static IHtmlCollection<IElement> FindAll(this INodeList nodelist, string selector)
        {
            if (nodelist is null) throw new ArgumentNullException(nameof(nodelist));
            return nodelist.QuerySelectorAll(selector);
        }
    }
}
