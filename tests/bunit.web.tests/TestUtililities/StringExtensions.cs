namespace Bunit.TestUtililities;
internal static class StringExtensions
{
	public static RenderFragment ToRenderFragment(this string markup)
		=> builder => builder.AddMarkupContent(0, markup);
}
