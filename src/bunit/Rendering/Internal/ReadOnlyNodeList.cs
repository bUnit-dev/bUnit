using System.Collections;
using AngleSharp;
using AngleSharp.Dom;

namespace Bunit.Rendering;

internal sealed class ReadOnlyNodeList : INodeList, IReadOnlyList<INode>
{
	private readonly List<INode> nodes;

	public ReadOnlyNodeList(List<INode> nodes) => this.nodes = nodes;

	public INode this[int index] => nodes[index];

	public int Length => nodes.Count;

	public int Count => nodes.Count;

	public IEnumerator<INode> GetEnumerator() => nodes.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
	{
		foreach (var node in nodes)
		{
			node.ToHtml(writer, formatter);
		}
	}
}
