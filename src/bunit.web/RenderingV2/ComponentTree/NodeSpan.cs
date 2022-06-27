using System.Collections;
using AngleSharp;
using AngleSharp.Dom;

namespace Bunit.RenderingV2.ComponentTree;

internal readonly record struct NodeSpan : IReadOnlyList<INode>, INodeList
{
	private readonly INode? first;
	private readonly INode? last;

	private int Offset => 0;

	internal readonly IElement Source { get; }

	public INode? First => first;

	public INode? Last => last;

	// TODO: INode.Index() searches through ChildNotes of parent to find the index of a node,
	//       so perf on this is poor to say the least.
	public int Count => Source.ChildNodes.Length;

	public int Length => Count;

	public INode this[int index]
		=> Source.ChildNodes[index + Offset];

	public NodeSpan(IElement source)
	{
		Source = source;
	}

	public NodeSpan(IElement source, INode? first, INode? last)
	{
		Source = source;
		this.first = first;
		this.last = last;
	}

	public IEnumerator<INode> GetEnumerator()
	{
		if (Source is null)
			yield break;

		for (int i = 0; i < Count; i++)
		{
			yield return this[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
	{
		foreach (var item in this)
		{
			item.ToHtml(writer, formatter);
		}
	}
}
