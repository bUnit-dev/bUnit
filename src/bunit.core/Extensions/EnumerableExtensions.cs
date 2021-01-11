using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Bunit.Extensions
{
	/// <summary>
	/// Helper methods for working with <see cref="IEnumerable{T}"/>.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Returns true if the numerable is null or empty.
		/// </summary>
		public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable)
		{
			return enumerable is null || !enumerable.Any();
		}
	}
}
