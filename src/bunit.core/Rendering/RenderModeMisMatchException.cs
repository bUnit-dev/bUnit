#if NET9_0_OR_GREATER
namespace Bunit.Rendering;

/// <summary>
/// Represents an exception that is thrown when a component under test has mismatching render modes assigned between parent and child components.
/// </summary>
public sealed class RenderModeMisMatchException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MissingRendererInfoException"/> class.
	/// </summary>
	public RenderModeMisMatchException()
		: base("""
			   A component under test has mismatching render modes assigned between parent and child components.
			   Ensure that the render mode of the parent component matches the render mode of the child component.
			   Learn more about render modes at https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-9.0#render-mode-propagation.
			   """)
	{
		HelpLink = "https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-9.0#render-mode-propagation";
	}
}
#endif
