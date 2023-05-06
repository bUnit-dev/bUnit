namespace Bunit.Extensions;

/// <summary>
/// Extensions for Blazor types.
/// </summary>
public static class BlazorExtensions
{
	/// <summary>
	/// Creates a <see cref="RenderFragment"/> that will render the <paramref name="markup"/>.
	/// </summary>
	/// <param name="markup">Markup to render.</param>
	/// <returns>The <see cref="RenderFragment"/>.</returns>
	public static RenderFragment ToMarkupRenderFragment([StringSyntax("Html")]this string? markup)
	{
		if (string.IsNullOrEmpty(markup))
			return _ => { };
		return
			builder => builder.AddMarkupContent(0, markup);
	}
}
