using AngleSharp.Dom;

namespace Bunit.Rendering;

/// <summary>
/// The markup produced for one root render tree, and the range of it each component rendered.
/// </summary>
/// <remarks>
/// Shared by every component under the root, so they all read from one AngleSharp document.
/// Replaced rather than mutated, which makes its reference identity the version stamp.
/// </remarks>
internal sealed class RootMarkupSnapshot
{
	private readonly BunitHtmlParser htmlParser;
	private readonly Dictionary<int, ComponentMarkupRange> rangesById;
	private readonly object gate = new();
	private INodeList? nodes;
	private int[]? sourceIndices;
	private IElement[]? elements;

	public string Markup { get; }

	/// <summary>
	/// Gets the component ranges in post-order, i.e. a component always precedes its ancestors.
	/// </summary>
	public IReadOnlyList<ComponentMarkupRange> Ranges { get; }

	public int RootComponentId { get; }

	public RootMarkupSnapshot(int rootComponentId, HtmlizerResult result, BunitHtmlParser htmlParser)
	{
		RootComponentId = rootComponentId;
		Markup = result.Markup;
		Ranges = result.Ranges;
		this.htmlParser = htmlParser;

		rangesById = new Dictionary<int, ComponentMarkupRange>(result.Ranges.Count);
		foreach (var range in result.Ranges)
		{
			rangesById[range.ComponentId] = range;
		}
	}

	public INodeList Nodes
	{
		get
		{
			lock (gate)
			{
				return nodes ??= htmlParser.Parse(Markup);
			}
		}
	}

	public IDocument? Document => Nodes.Length > 0 ? Nodes[0].Owner : null;

	public string GetMarkup(int componentId)
		=> rangesById.TryGetValue(componentId, out var range)
			? Markup[range.Start..range.End]
			: string.Empty;

	public bool TryGetRange(int componentId, out ComponentMarkupRange range)
		=> rangesById.TryGetValue(componentId, out range);

	/// <summary>
	/// Gets the nodes the component rendered, as a view into <see cref="Nodes"/>. Returns
	/// <see langword="false"/> when they cannot be identified there and the component needs
	/// a document of its own.
	/// </summary>
	public bool TryGetNodeView(int componentId, out INodeList view)
	{
		if (!rangesById.TryGetValue(componentId, out var range))
		{
			view = NodeRangeList.Empty;
			return false;
		}

		if (componentId == RootComponentId)
		{
			view = Nodes;
			return true;
		}

		if (range.Start == range.End)
		{
			view = NodeRangeList.Empty;
			return true;
		}

		var slice = Markup.AsSpan(range.Start, range.End - range.Start);
		var firstNonWhitespace = IndexOfFirstNonWhitespace(slice);
		var lastNonWhitespace = IndexOfLastNonWhitespace(slice);

		// AngleSharp merges adjacent text nodes, so leading or trailing text is fused
		// into the parent's text node and no range of nodes can represent this component.
		if (firstNonWhitespace < 0 || slice[firstNonWhitespace] != '<' || slice[lastNonWhitespace] != '>')
		{
			view = NodeRangeList.Empty;
			return false;
		}

		if (!TryGetElementAt(range.Start + firstNonWhitespace, out var first))
		{
			view = NodeRangeList.Empty;
			return false;
		}

		view = CollectSiblingRun(first, range);
		return true;
	}

	private static NodeRangeList CollectSiblingRun(IElement first, ComponentMarkupRange range)
	{
		var result = new List<INode>();
		var lastElementIndex = -1;

		for (INode? node = first; node is not null; node = node.NextSibling)
		{
			// An element without a source position - synthesized from a stray end tag, or in a
			// foreign namespace such as SVG - cannot be placed, but it is inside this markup.
			if (node is IElement element && GetSourceIndex(element) is var index and >= 0 && !range.Contains(index))
			{
				break;
			}

			if (node is IElement)
			{
				lastElementIndex = result.Count;
			}

			result.Add(node);
		}

		// The markup is known to end with a tag, so anything after the last element is the parent's.
		if (lastElementIndex + 1 < result.Count)
		{
			result.RemoveRange(lastElementIndex + 1, result.Count - lastElementIndex - 1);
		}

		return new NodeRangeList(result);
	}

	private bool TryGetElementAt(int sourceIndex, [NotNullWhen(true)] out IElement? element)
	{
		EnsureElementIndex();

		var position = Array.BinarySearch(sourceIndices!, sourceIndex);
		if (position < 0)
		{
			element = null;
			return false;
		}

		// The parser clones formatting elements, so several can share one source index.
		while (position > 0 && sourceIndices![position - 1] == sourceIndex)
		{
			position--;
		}

		element = elements![position];
		return true;
	}

	private void EnsureElementIndex()
	{
		lock (gate)
		{
			if (sourceIndices is not null)
			{
				return;
			}

			var found = new List<IElement>();
			foreach (var node in Nodes)
			{
				CollectElements(node, found);
			}

			found.Sort(static (x, y) => GetSourceIndex(x).CompareTo(GetSourceIndex(y)));

			sourceIndices = new int[found.Count];
			for (var i = 0; i < found.Count; i++)
			{
				sourceIndices[i] = GetSourceIndex(found[i]);
			}

			elements = found.ToArray();
		}

		static void CollectElements(INode node, List<IElement> found)
		{
			if (node is IElement element && GetSourceIndex(element) >= 0)
			{
				found.Add(element);
			}

			foreach (var child in node.ChildNodes)
			{
				CollectElements(child, found);
			}
		}
	}

	/// <summary>
	/// Gets the index in <see cref="Markup"/> the element was parsed from, or -1 when the
	/// parser synthesized it, e.g. an implicit &lt;tbody&gt;.
	/// </summary>
	internal static int GetSourceIndex(IElement element)
		=> element.SourceReference?.Position.Index ?? -1;

	private static int IndexOfFirstNonWhitespace(ReadOnlySpan<char> value)
	{
		for (var i = 0; i < value.Length; i++)
		{
			if (!char.IsWhiteSpace(value[i]))
				return i;
		}

		return -1;
	}

	private static int IndexOfLastNonWhitespace(ReadOnlySpan<char> value)
	{
		for (var i = value.Length - 1; i >= 0; i--)
		{
			if (!char.IsWhiteSpace(value[i]))
				return i;
		}

		return -1;
	}
}
