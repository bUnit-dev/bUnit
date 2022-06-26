using System.Collections;
using AngleSharp;
using AngleSharp.Dom;

namespace Bunit.RenderingV2.AngleSharp;

internal class NodeList : INodeList, IReadOnlyList<INode>
{
	private readonly List<INode> nodes = new();

	public INode this[int index] => nodes[index];

	public int Count => nodes.Count;

	public int Length => Count;

	internal void Add(INode node)
		=> nodes.Add(node);

	internal void AddRange(IEnumerable<INode> nodes)
		=> this.nodes.AddRange(nodes);

	public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
	{
		for (var i = 0; i < nodes.Count; i++)
		{
			nodes[i].ToHtml(writer, formatter);
		}
	}

	public IEnumerator<INode> GetEnumerator() => nodes.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
