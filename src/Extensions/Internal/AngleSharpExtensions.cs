using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Text;
using Bunit.Diffing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunit
{
    /// <summary>
    /// Extensions for AngleSharp types.
    /// </summary>
    internal static class AngleSharpExtensions
    {        
        /// <summary>
        /// Wraps the <paramref name="node"/> in an <see cref="IEnumerable{INode}"/>.
        /// </summary>
        /// <param name="node">The node to wrap</param>
        /// <returns>The <see cref="IEnumerable{INode}"/>.</returns>
        public static IEnumerable<INode> AsEnumerable(this INode node)
        {
            yield return node;
        }

        /// <summary>
        /// Gets the <see cref="TestHtmlParser"/> stored in the <paramref name="node"/>s
        /// owning context, if one is available. 
        /// </summary>
        /// <param name="node"></param>
        /// <returns>The <see cref="TestHtmlParser"/> or null if not found.</returns>
        public static TestHtmlParser? GetHtmlParser(this INode? node)
        {
            return node?.Owner.Context.GetService<TestHtmlParser>();
        }

        /// <summary>
        /// Gets the <see cref="TestHtmlParser"/> stored in the <paramref name="nodes"/>s
        /// owning context, if one is available. 
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns>The <see cref="TestHtmlParser"/> or null if not found.</returns>
        public static TestHtmlParser? GetHtmlParser(this INodeList nodes)
        {
            return nodes?.Length > 0 ? nodes[0].GetHtmlParser() : null;
        }

        /// <summary>
        /// Gets the <see cref="HtmlComparer"/> stored in the <paramref name="node"/>s
        /// owning context, if one is available. 
        /// </summary>
        /// <param name="node"></param>
        /// <returns>The <see cref="HtmlComparer"/> or null if not found.</returns>
        public static HtmlComparer? GetHtmlComparer(this INode? node)
        {
            return node?.Owner.Context.GetService<HtmlComparer>();
        }

        /// <summary>
        /// Gets the <see cref="HtmlComparer"/> stored in the <paramref name="nodes"/>s
        /// owning context, if one is available. 
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns>The <see cref="HtmlComparer"/> or null if not found.</returns>
        public static HtmlComparer? GetHtmlComparer(this INodeList nodes)
        {
            return nodes?.Length > 0 ? nodes[0].GetHtmlComparer() : null;
        }
    }
}
