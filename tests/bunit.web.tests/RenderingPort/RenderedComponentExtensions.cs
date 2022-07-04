using System.Collections;
using AngleSharp.Dom;

namespace Bunit.RenderingPort;

internal static class RenderedComponentExtensions
{
	public static IElement Find<TComponent>(this IRenderedComponent<TComponent> rc, string cssSelector) where TComponent: IComponent
		=> rc.Nodes.QuerySelector(cssSelector) ?? throw new ElementNotFoundException(cssSelector);

	public static TElement Find<TComponent, TElement>(this IRenderedComponent<TComponent> rc, string cssSelector) where TComponent : IComponent
		where TElement : class, IElement
		=> Find(rc, cssSelector) is TElement result
			? result
			: throw new ElementNotFoundException(cssSelector);

	public static IHtmlCollection<IElement> FindAll<TComponent>(this IRenderedComponent<TComponent> rc, string cssSelector) where TComponent : IComponent
		=> rc.Nodes.QuerySelectorAll(cssSelector);

	public static IHtmlCollection<TElement> FindAll<TComponent, TElement>(this IRenderedComponent<TComponent> rc, string cssSelector) where TComponent : IComponent
		where TElement : class, IElement
		=> new HtmlCollection<TElement>(FindAll(rc, cssSelector).OfType<TElement>());

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
