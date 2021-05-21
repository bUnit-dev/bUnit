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
		/// Gets whether to add parameters captured by the <see cref="Stub{TComponent}"/>
		/// to the rendered markup element.
		/// </summary>
		public bool AddParameters { get; init; } = true;

		/// <summary>
		/// Gets whether to add the <c>diff:ignore</c> attribute to the
		/// rendered markup element. The <c>diff:ignore</c> tells the semantic
		/// HTML comparer in bUnit to ignore the element when comparing
		/// the markup from the component under test with expected markup.
		/// </summary>
		/// <remarks>Learn more about the <c>diff:ignore</c> and related attributes at
		/// https://bunit.dev/docs/verification/semantic-html-comparison</remarks>
		public bool AddDiffIgnore { get; init; } = true;
	}
}
#endif
