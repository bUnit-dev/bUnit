using System.Collections;
using AngleSharp;
using AngleSharp.Dom;

namespace Bunit.RenderingV2.ComponentTree;

// This has pointers to the previous sibling and next sibling inside
// the source, if the NodeSpan is limited to only parts of the source.
// Unfortunately, there does not seem to be a way to detect when the
// nodes to the source from here, which is why there is a index cache
// for both siblings. The caches are checked and potentially updated
// each time Count property, indexer property, or the GetEnumerator
// method are used.
//
// This is probably not super inefficient, but its the best solution
// for now.
//
// TODO: Can we optimize this further to make lookups, iterations,
//       more effective.
// TODO: Are there benefits making this a struct?
internal class NodeSpan : IReadOnlyList<INode>, INodeList
{
	private readonly INode source;
	private int nextFrameIndex;

	private List<(INode Node, int FrameIndex)> Nodes { get; } = new();

	private int Offset => Head is not null
		? Head.Index()
		: 0;

	internal INode? Head { get; set; }

	public int Count => source.ChildNodes.Length - Offset;

	public INode this[int index] => source.ChildNodes[index + Offset];

	public NodeSpan(NodeSpan parent) => this.source = parent.source;

	public NodeSpan(INode source) => this.source = source;

	internal void RemoveAt(int frameIndex, INode parent)
	{
		if (ReferenceEquals(source, parent))
		{
			int i = 0;
			while (i < Nodes.Count && Nodes[i].FrameIndex < frameIndex)
			{
				i++;
			}

			while (i < Nodes.Count && Nodes[i].FrameIndex == frameIndex)
			{
				Remove(Nodes[i].Node);
				Nodes.Remove(Nodes[i]);
			}

			nextFrameIndex = frameIndex;
			var oldFrameIndex = i < Nodes.Count
				? Nodes[i].FrameIndex
				: 0;
			while (i < Nodes.Count)
			{
				var ni = Nodes[i];
				if (ni.FrameIndex == oldFrameIndex)
				{
					ni.FrameIndex = nextFrameIndex;
				}
				else if (ni.FrameIndex != oldFrameIndex)
				{
					oldFrameIndex = ni.FrameIndex;
					nextFrameIndex++;
					ni.FrameIndex = nextFrameIndex;
				}
				i++;
			}
		}
	}

	internal void Remove(INode node)
	{
		if (ReferenceEquals(node, Head))
		{
			Head = node.NextSibling;
		}

		source.RemoveChild(node);
	}

	internal void Append(INode node, INode parent)
	{
		if (ReferenceEquals(source, parent))
		{
			Nodes.Add((node, nextFrameIndex));
			nextFrameIndex++;
		}

		parent.AppendChild(node);
	}

	internal void Append(INodeList nodes, INode parent)
	{
		if (nodes.Length > 1)
		{
			if (ReferenceEquals(source, parent))
			{
				foreach (var n in nodes)
				{
					Nodes.Add((n, nextFrameIndex));
				}
				nextFrameIndex++;
			}

			var node = source.Owner!.CreateDocumentFragment();

			while (nodes.Length > 0)
			{
				node.AppendChild(nodes[0]);
			}

			parent.AppendChild(node);
		}
		else if (nodes.Length == 1)
		{
			Append(nodes[0], parent);
		}
	}

	internal INode GetNodeByFrameIndex(int frameIndex)
		=> Nodes.Single(x => x.FrameIndex == frameIndex).Node;

	public IEnumerator<INode> GetEnumerator() => source.ChildNodes.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	#region INodeList / IMarkupFormattable
	INode INodeList.this[int index] => this[index];
	int INodeList.Length => Count;
	void IMarkupFormattable.ToHtml(TextWriter writer, IMarkupFormatter formatter)
	{
		foreach (var item in this)
		{
			item.ToHtml(writer, formatter);
		}
	}
	#endregion
}

internal class NodeSpan2 : IReadOnlyList<INode>
{
	private readonly IElement source;

	public int Count { get; }

	public INode this[int index] => throw new NotImplementedException();

	public NodeSpan2(IElement source) => this.source = source;

	public void Append(INode node)
	{

	}

	public IEnumerator<INode> GetEnumerator() { yield break; }
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
