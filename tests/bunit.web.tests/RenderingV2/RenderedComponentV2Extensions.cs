using AngleSharp.Dom;

namespace Bunit.RenderingV2;

internal static class RenderedComponentV2Extensions
{
	public static IElement Find(this IRenderedComponent rc, string cssSelector)
		=> rc.Nodes.QuerySelector(cssSelector) ?? throw new ElementNotFoundException(cssSelector);
}
