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
	}
}
#endif
