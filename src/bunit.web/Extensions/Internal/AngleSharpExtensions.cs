using System.Collections.Generic;

using AngleSharp.Dom;

using Bunit.Diffing;

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
