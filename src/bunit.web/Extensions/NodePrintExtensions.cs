using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Text;
using Bunit.Diffing;

namespace Bunit
{
	/// <summary>
	/// Helper methods for pretty printing markup from <see cref="INode"/> and <see cref="INodeList"/>.
	/// </summary>
	public static class NodePrintExtensions
	{
		/// <summary>
		/// Writes the serialization of the node guided by the formatter.
		/// </summary>
		/// <param name="nodes">The nodes to serialize.</param>
		/// <param name="writer">The output target of the serialization.</param>
		/// <param name="formatter">The formatter to use.</param>
		public static void ToHtml(this IEnumerable<INode> nodes, TextWriter writer, IMarkupFormatter formatter)
		{
			if (nodes is null)
				throw new ArgumentNullException(nameof(nodes));

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
			if (nodes is null)
				throw new ArgumentNullException(nameof(nodes));

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
			if (markupFormattable is null)
				throw new ArgumentNullException(nameof(markupFormattable));

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
			if (nodes is null)
				throw new ArgumentNullException(nameof(nodes));

			using var sw = new StringWriter();
			var formatter = new PrettyMarkupFormatter()
			{
				NewLine = Environment.NewLine,
				Indentation = "  ",
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
			if (markupFormattable is null)
				throw new ArgumentNullException(nameof(markupFormattable));

			using var sw = new StringWriter();
			var formatter = new PrettyMarkupFormatter()
			{
				NewLine = Environment.NewLine,
				Indentation = "  ",
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
			if (element is null)
				throw new ArgumentNullException(nameof(element));

			var result = new StringBuilder();
			result.Append(Symbols.LessThan);

			var prefix = element.Prefix;
			var name = element.LocalName;
			var tag = !string.IsNullOrEmpty(prefix) ? string.Concat(prefix, ":", name) : name;

			result.Append(tag);

			foreach (var attribute in element.Attributes)
			{
				result.Append(' ').Append(DiffMarkupFormatter.Instance.ConvertToString(attribute));
			}

			if (element.HasChildNodes)
			{
				result.Append(Symbols.GreaterThan);
				result.Append("...");
				result.Append("</").Append(tag).Append('>');
			}
			else
			{
				result.Append(" />");
			}

			return result.ToString();
		}
	}
}
