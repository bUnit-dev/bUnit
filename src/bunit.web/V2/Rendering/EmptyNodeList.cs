using System.Collections;
using AngleSharp;
using AngleSharp.Dom;

namespace Bunit.V2.Rendering;

internal class EmptyNodeList : INodeList
{
	public static readonly INodeList Empty = new EmptyNodeList();

	INode INodeList.this[int index]
	{
		get
		{
			throw new IndexOutOfRangeException();
		}
	}

	public int Length => 0;

	public IEnumerator<INode> GetEnumerator() { yield break; }

	public void ToHtml(TextWriter writer, IMarkupFormatter formatter) { }

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
