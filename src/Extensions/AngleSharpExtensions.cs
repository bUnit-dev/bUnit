using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Text;
using Egil.RazorComponents.Testing.Diffing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing.Extensions
{
    /// <summary>
    /// Extensions for AngleSharp types.
    /// </summary>
    public static class AngleSharpExtensions
    {
        /// <summary>
        /// Writes the serialization of the node guided by the formatter.
        /// </summary>
        /// <param name="nodes">The nodes to serialize.</param>
        /// <param name="writer">The output target of the serialization.</param>
        /// <param name="formatter">The formatter to use.</param>
        public static void ToHtml(this IEnumerable<INode> nodes, TextWriter writer, IMarkupFormatter formatter)
        {
            if (nodes is null) throw new ArgumentNullException(nameof(nodes));

            foreach (var node in nodes)
            {
                node.ToHtml(writer, formatter);
            }
        }

        /// <summary>
        /// Uses the <see cref="DiffMarkupFormatter"/> to generate a HTML markup string
        /// from a <see cref="IEnumerable{INode}"/> <paramref name="nodes"/>.
        /// The generated HTML markup will NOT include the internal Blazor attributes
        /// added to elements.
        /// </summary>
        public static string ToDiffMarkup(this IEnumerable<INode> nodes)
        {
            if (nodes is null) throw new ArgumentNullException(nameof(nodes));

            using var sw = new StringWriter();
            nodes.ToHtml(sw, new DiffMarkupFormatter());
            return sw.ToString();
        }

        /// <summary>
        /// Uses the <see cref="DiffMarkupFormatter"/> to generate a HTML markup string
        /// from a <see cref="IMarkupFormattable"/> <paramref name="markupFormattable"/>.
        /// The generated HTML markup will NOT include the internal Blazor attributes
        /// added to elements.
        /// </summary>
        public static string ToDiffMarkup(this IMarkupFormattable markupFormattable)
        {
            if (markupFormattable is null) throw new ArgumentNullException(nameof(markupFormattable));

            using var sw = new StringWriter();
            markupFormattable.ToHtml(sw, new DiffMarkupFormatter());
            return sw.ToString();
        }

        /// <summary>
        /// Uses the <see cref="PrettyMarkupFormatter"/> to generate a HTML markup string
        /// from a <see cref="IEnumerable{INode}"/> <paramref name="nodes"/>.
        /// </summary>
        public static string ToMarkup(this IEnumerable<INode> nodes)
        {
            if (nodes is null) throw new ArgumentNullException(nameof(nodes));

            using var sw = new StringWriter();
            var formatter = new PrettyMarkupFormatter()
            {
                NewLine = Environment.NewLine,
                Indentation = "  "
            };
            nodes.ToHtml(sw, formatter);
            return sw.ToString();
        }

        /// <summary>
        /// Uses the <see cref="PrettyMarkupFormatter"/> to generate a HTML markup
        /// from a <see cref="IMarkupFormattable"/> <paramref name="markupFormattable"/>.
        /// </summary>
        public static string ToMarkup(this IMarkupFormattable markupFormattable)
        {
            if (markupFormattable is null) throw new ArgumentNullException(nameof(markupFormattable));

            using var sw = new StringWriter();
            var formatter = new PrettyMarkupFormatter()
            {
                NewLine = Environment.NewLine,
                Indentation = "  "
            };
            markupFormattable.ToHtml(sw, formatter);
            return sw.ToString();
        }

        /// <summary>
        /// Converts an <see cref="IElement"/> into a HTML markup string, 
        /// with only its tag and attributes included in the output. All 
        /// child nodes are skipped.
        /// </summary>
        public static string ToMarkupElementOnly(this IElement element)
        {
            if (element is null) throw new ArgumentNullException(nameof(element));

            var result = new StringBuilder();
            result.Append(Symbols.LessThan);

            var prefix = element.Prefix;
            var name = element.LocalName;
            var tag = !string.IsNullOrEmpty(prefix) ? string.Concat(prefix, ":", name) : name;

            result.Append(tag);

            foreach (var attribute in element.Attributes)
            {
                result.Append(' ').Append(HtmlMarkupFormatter.Instance.Attribute(attribute));
            }

            if (element.HasChildNodes)
            {
                result.Append(Symbols.GreaterThan);
                result.Append("...");
                result.Append($"</{tag}>");
            }
            else
            {
                result.Append(" />");
            }

            return result.ToString();
        }

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
        /// Wraps the <paramref name="node"/> in an <see cref="IEnumerable{INode}"/>.
        /// </summary>
        /// <param name="node">The node to wrap</param>
        /// <returns>The <see cref="IEnumerable{INode}"/>.</returns>
        public static IEnumerable<INode> AsEnumerable(this INode node)
        {
            yield return node;
        }


        /// <summary>
        /// Gets the <see cref="HtmlParser"/> stored in the <paramref name="node"/>s
        /// owning context, if one is available. 
        /// </summary>
        /// <param name="node"></param>
        /// <returns>The <see cref="HtmlParser"/> or null if not found.</returns>
        public static HtmlParser? GetHtmlParser(this INode? node)
        {
            return node?.Owner.Context.GetService<HtmlParser>();
        }

        /// <summary>
        /// Gets the <see cref="HtmlParser"/> stored in the <paramref name="nodes"/>s
        /// owning context, if one is available. 
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns>The <see cref="HtmlParser"/> or null if not found.</returns>
        public static HtmlParser? GetHtmlParser(this INodeList nodes)
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
