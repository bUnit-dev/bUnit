using System.Collections;
using AngleSharp.Dom;

namespace Bunit.RenderingPort;

internal static class RenderedComponentExtensions
{
	public static IElement Find(this IRenderedComponent rc, string cssSelector)
		=> rc.Nodes.Find(cssSelector);

	public static TElement Find<TElement>(this IRenderedComponent rc, string cssSelector)
		where TElement : class, IElement
		=> rc.Nodes.Find<TElement>(cssSelector);

	public static IHtmlCollection<IElement> FindAll(this IRenderedComponent rc, string cssSelector)
		=> rc.Nodes.FindAll(cssSelector);

	public static IHtmlCollection<TElement> FindAll<TElement>(this IRenderedComponent rc, string cssSelector)
		where TElement : class, IElement
		=> rc.Nodes.FindAll<TElement>(cssSelector);
}
