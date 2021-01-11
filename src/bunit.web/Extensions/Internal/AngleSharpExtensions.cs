using System.Collections.Generic;
using AngleSharp.Diffing.Extensions;
using AngleSharp.Dom;
using Bunit.Diffing;
using Bunit.Rendering;

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
		/// <param name="node">The node to wrap.</param>
		/// <returns>The <see cref="IEnumerable{INode}"/>.</returns>
		public static IEnumerable<INode> AsEnumerable(this INode node)
		{
			yield return node;
		}

		/// <summary>
		/// Gets the <see cref="BunitHtmlParser"/> stored in the <paramref name="node"/>s
		/// owning context, if one is available.
		/// </summary>
		/// <returns>The <see cref="BunitHtmlParser"/> or null if not found.</returns>
		public static BunitHtmlParser? GetHtmlParser(this INode? node)
		{
			return node?.Owner.Context.GetService<BunitHtmlParser>();
		}

		/// <summary>
		/// Gets the <see cref="BunitHtmlParser"/> stored in the <paramref name="nodes"/>s
		/// owning context, if one is available.
		/// </summary>
		/// <returns>The <see cref="BunitHtmlParser"/> or null if not found.</returns>
		public static BunitHtmlParser? GetHtmlParser(this INodeList nodes)
		{
			return nodes?.Length > 0 ? nodes[0].GetHtmlParser() : null;
		}

		/// <summary>
		/// Gets the <see cref="HtmlComparer"/> stored in the <paramref name="node"/>s
		/// owning context, if one is available.
		/// </summary>
		/// <returns>The <see cref="HtmlComparer"/> or null if not found.</returns>
		public static HtmlComparer? GetHtmlComparer(this INode? node)
		{
			return node?.Owner.Context.GetService<HtmlComparer>();
		}

		/// <summary>
		/// Gets the <see cref="HtmlComparer"/> stored in the <paramref name="nodes"/>s
		/// owning context, if one is available.
		/// </summary>
		/// <returns>The <see cref="HtmlComparer"/> or null if not found.</returns>
		public static HtmlComparer? GetHtmlComparer(this INodeList nodes)
		{
			return nodes?.Length > 0 ? nodes[0].GetHtmlComparer() : null;
		}

		/// <summary>
		/// Gets the <see cref="TestContext"/> stored in the <paramref name="node"/>s
		/// owning context, if one is available.
		/// </summary>
		/// <returns>The <see cref="TestContext"/> or null if not found.</returns>
		public static TestContextBase? GetTestContext(this INode? node)
		{
			return node?.Owner.Context.GetService<TestContextBase>();
		}

		/// <summary>
		/// Gets the <see cref="TestContext"/> stored in the <paramref name="nodes"/>s
		/// owning context, if one is available.
		/// </summary>
		/// <returns>The <see cref="TestContext"/> or null if not found.</returns>
		public static TestContextBase? GetTestContext(this INodeList nodes)
		{
			return nodes?.Length > 0 ? nodes[0].GetTestContext() : null;
		}

		/// <summary>
		/// Gets the parents of the <paramref name="element"/>, starting with
		/// the <paramref name="element"/> itself.
		/// </summary>
		public static IEnumerable<IElement> GetParentsAndSelf(this IElement element)
		{
			yield return element;
			foreach (var node in element.GetParents())
			{
				if (node is IElement parent)
					yield return parent;
			}
		}
	}
}
