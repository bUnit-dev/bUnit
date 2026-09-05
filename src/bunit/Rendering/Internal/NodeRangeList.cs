using System.Collections;
using AngleSharp;
using AngleSharp.Dom;

namespace Bunit.Rendering;

/// <summary>
/// A read-only <see cref="INodeList"/> over a range of sibling nodes.
/// </summary>
internal sealed class NodeRangeList : INodeList, IReadOnlyList<INode>
{
	public static readonly NodeRangeList Empty = new([]);

	private readonly IReadOnlyList<INode> nodes;

	public NodeRangeList(IReadOnlyList<INode> nodes) => this.nodes = nodes;

	public INode this[int index] => nodes[index];

	public int Length => nodes.Count;

	public int Count => nodes.Count;

	public IEnumerator<INode> GetEnumerator() => nodes.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
	{
		for (var i = 0; i < nodes.Count; i++)
		{
			nodes[i].ToHtml(writer, formatter);
		}
	}
}
