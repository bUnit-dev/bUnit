namespace Bunit.Extensions;

/// <summary>
/// Helper methods for working with <see cref="IEnumerable{T}"/>.
/// </summary>
internal static class EnumerableExtensions
{
	/// <summary>
	/// Returns true if the enumerable is null or empty.
	/// </summary>
	public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable)
	{
		return enumerable is null || !enumerable.Any();
	}
}
