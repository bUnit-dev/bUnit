#if NET5_0_OR_GREATER

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a set of options that can be passed to <see cref="Stub{TComponent}"/>.
	/// </summary>
	public record StubOptions
	{
		/// <summary>
		/// Gets an instance of the default <see cref="StubOptions"/> for <see cref="Stub{TComponent}"/>.
		/// </summary>
		public static readonly StubOptions Default = new();

		/// <summary>
		/// Gets whther to render a placeholder element in DOM with the name of the replaced element.
		/// </summary>
		/// <remarks>
		/// This only works when if the parent element in the DOM tree allows custom elements to be added to it.
		/// Certain elements
		/// </remarks>
		public bool RenderPlaceholder { get; init; }

		/// <summary>
		/// Gets whether to render parameters captured by the <see cref="Stub{TComponent}"/>
		/// to the rendered placeholder in the DOM. Only used when <see cref="RenderPlaceholder"/> is <c>true</c>.
		/// </summary>
		public bool RenderParameters { get; init; } = true;
	}
}
#endif
