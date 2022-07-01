using AngleSharp.Dom;

namespace Bunit.RenderingV2.AngleSharp;

public static class AngleSharpDomExtensions
{
	/// <summary>
	/// Runs the mutation macro as defined in 5.2.2 Mutation methods
	/// of http://www.w3.org/TR/domcore/.
	/// </summary>
	/// <param name="parent">The parent, which invokes the algorithm.</param>
	/// <param name="nodes">The nodes list to add.</param>
	/// <returns>A (single) node.</returns>
	private static INode MutationMacro(this INode parent, INodeList nodes)
	{
		if (nodes.Length > 1)
		{
			var node = parent.Owner!.CreateDocumentFragment();

			while (nodes.Length > 0)
			{
				node.AppendChild(nodes[0]);
			}

			return node;
		}

		return nodes[0];
	}

	/// <summary>
	/// Inserts nodes before the given child.
	/// </summary>
	/// <param name="nodes">The nodes to insert.</param>
	/// <param name="child">The child node to insert before.</param>
	/// <returns>The current element.</returns>
	public static void InsertBefore(this INodeList nodes, INode child)
	{
		var parent = child.Parent;

		if (parent is not null && nodes.Length > 0)
		{
			var node = nodes.Length == 1
				? nodes[0]
				: parent.MutationMacro(nodes);

			parent.PreInsert(node, child);
		}
	}

	/// <summary>
	/// Inserts nodes after the given child.
	/// </summary>
	/// <param name="nodes">The nodes to insert.</param>
	/// <param name="child">The child node to insert after.</param>
	/// <returns>The current element.</returns>
	public static void InsertAfter(this INodeList nodes, INode child)
	{
		var parent = child.Parent;

		if (parent is not null && nodes.Length > 0)
		{
			var node = nodes.Length == 1
				? nodes[0]
				: parent.MutationMacro(nodes);

			parent.PreInsert(node, child.NextSibling);
		}
	}

	/// <summary>
	/// Appends nodes to parent node.
	/// </summary>
	/// <param name="parent">The parent, where to append to.</param>
	/// <param name="nodes">The node list to append.</param>
	public static void AppendNodes(this INode parent, INodeList nodes)
	{
		if (nodes.Length > 0)
		{
			var node = nodes.Length == 1
				? nodes[0]
				: parent.MutationMacro(nodes);

			parent.PreInsert(node, null);
		}
	}
}
