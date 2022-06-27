using System.Collections;
using AngleSharp;
using AngleSharp.Dom;

namespace Bunit.RenderingV2.ComponentTree;

internal readonly struct HtmlNodeSpan : IReadOnlyList<INode>, INodeList
{
	public static readonly HtmlNodeSpan Empty = new();

	public INodeList Source { get; } = EmptyNodeList.Instance;
	public int Offset { get; }
	public int Count { get; }
	public int Length { get; }

	public INode this[int index]
		=> Source[index + Offset];

	public HtmlNodeSpan(INodeList source, int offset)
		: this(source, offset, source.Length)
	{
	}

	public HtmlNodeSpan(INodeList source, int offset, int count)
	{
		Source = source;
		Offset = offset;
		Count = Length = count;
	}

	public IEnumerator<INode> GetEnumerator()
	{
		if (Source is null)
			yield break;

		for (int i = Offset; i < Count; i++)
		{
			yield return Source[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
	{
		for (int i = Offset; i < Count; i++)
		{
			Source[i].ToHtml(writer, formatter);
		}
	}

	class EmptyNodeList : INodeList
	{
		public static readonly INodeList Instance = new EmptyNodeList();
		public INode this[int index] => throw new IndexOutOfRangeException();
		public int Length { get; } = 0;
		public IEnumerator<INode> GetEnumerator() { yield break; }
		public void ToHtml(TextWriter writer, IMarkupFormatter formatter) { }
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
