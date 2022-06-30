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
	private int previousSiblingIndexCache;
	private int nextSiblingIndexCache;

	internal INode? PreviousSibling { get; set; }

	internal INode? NextSibling { get; set; }

	int INodeList.Length => Count;

	INode INodeList.this[int index] => this[index];

	public int Count
		=> ReferenceEquals(PreviousSibling, NextSibling)
			? 0
			: CalculateLength() - CalculateOffset();


	public INode this[int index]
	{
		get
		{
			var calculatedIndex = index + CalculateOffset();
			if (calculatedIndex < CalculateLength())
			{
				return source.ChildNodes[calculatedIndex];
			}
			else
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}
		}
	}

	public NodeSpan(INode source, INode? previousSibling = null, INode? nextSibling = null)
	{
		this.source = source;
		this.PreviousSibling = previousSibling;
		this.NextSibling = nextSibling;
	}

	public IEnumerator<INode> GetEnumerator()
	{
		if (source is null || ReferenceEquals(PreviousSibling, NextSibling))
			yield break;

		var length = CalculateLength();
		for (int i = CalculateOffset(); i < length; i++)
		{
			yield return source.ChildNodes[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	// Finds the offset in the source where the
	// node span starts. Previous sibling points to the node
	// that precedes this span's range.
	private int CalculateOffset()
	{
		if (PreviousSibling is null)
			return 0;

		if (source.ChildNodes.Length <= previousSiblingIndexCache || !ReferenceEquals(source.ChildNodes[previousSiblingIndexCache], PreviousSibling))
		{
			previousSiblingIndexCache = source.ChildNodes.Index(PreviousSibling);
		}

		return previousSiblingIndexCache + 1;
	}

	// Find the index of the next sibling node, if present.
	// This is used as the length of this node span, as
	// next sibling represents the node just outside the
	// span's range.
	private int CalculateLength()
	{
		if (NextSibling is null)
			return source.ChildNodes.Length;

		if (source.ChildNodes.Length <= nextSiblingIndexCache || !ReferenceEquals(source.ChildNodes[nextSiblingIndexCache], NextSibling))
		{
			nextSiblingIndexCache = source.ChildNodes.Index(NextSibling);
		}

		return nextSiblingIndexCache;
	}

	void IMarkupFormattable.ToHtml(TextWriter writer, IMarkupFormatter formatter)
	{
		foreach (var item in this)
		{
			item.ToHtml(writer, formatter);
		}
	}
}
