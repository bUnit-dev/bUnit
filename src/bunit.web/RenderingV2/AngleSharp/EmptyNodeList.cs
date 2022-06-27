using System.Collections;
using AngleSharp;
using AngleSharp.Dom;

namespace Bunit.RenderingV2.AngleSharp;

internal class EmptyNodeList : INodeList
{
	public static readonly INodeList Instance = new EmptyNodeList();
	public INode this[int index] => throw new IndexOutOfRangeException();
	public int Length { get; } = 0;
	public IEnumerator<INode> GetEnumerator() { yield break; }
	public void ToHtml(TextWriter writer, IMarkupFormatter formatter) { }
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
