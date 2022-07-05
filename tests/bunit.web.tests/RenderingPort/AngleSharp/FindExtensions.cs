using System.Collections;
using AngleSharp.Dom;

namespace Bunit.RenderingPort;

public static class FindExtensions
{
	public static IElement Find(this IElement element, string cssSelector)
		=> element.QuerySelector(cssSelector) ?? throw new ElementNotFoundException(cssSelector);

	public static TElement Find<TElement>(this IElement element, string cssSelector)
		where TElement : class, IElement
		=> Find(element, cssSelector) is TElement result
			? result
			: throw new ElementNotFoundException(cssSelector);

	public static IElement Find(this INodeList list, string cssSelector)
			=> list.QuerySelector(cssSelector) ?? throw new ElementNotFoundException(cssSelector);

	public static TElement Find<TElement>(this INodeList list, string cssSelector)
		where TElement : class, IElement
		=> Find(list, cssSelector) is TElement result
			? result
			: throw new ElementNotFoundException(cssSelector);

	public static IHtmlCollection<IElement> FindAll(this IElement element, string cssSelector)
		=> element.QuerySelectorAll(cssSelector);

	public static IHtmlCollection<TElement> FindAll<TElement>(this IElement element, string cssSelector)
		where TElement : class, IElement
		=> new HtmlCollection<TElement>(FindAll(element, cssSelector).OfType<TElement>());

	public static IHtmlCollection<IElement> FindAll(this INodeList list, string cssSelector)
		=> list.QuerySelectorAll(cssSelector);

	public static IHtmlCollection<TElement> FindAll<TElement>(this INodeList list, string cssSelector)
		where TElement : class, IElement
		=> new HtmlCollection<TElement>(FindAll(list, cssSelector).OfType<TElement>());

	private class HtmlCollection<TElement> : IHtmlCollection<TElement>
		where TElement : class, IElement
	{
		private readonly List<TElement> elements;

		public TElement this[int index] => elements[index];

		public TElement? this[string id] => elements.GetElementById(id);

		public int Length => elements.Count;

		public HtmlCollection(IEnumerable<TElement> elements)
		{
			this.elements = elements.ToList();
		}

		public IEnumerator<TElement> GetEnumerator() => elements.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => elements.GetEnumerator();
	}
}
