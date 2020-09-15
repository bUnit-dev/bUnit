using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Bunit.Extensions
{
	/// <summary>
	/// Helper methods for working with <see cref="IEnumerable{T}"/>
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

		/// <summary>
		/// Filters a enumerable of nullable objects
		/// to a collection with the any null objects filtered out.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> NotNull<TResult>(this IEnumerable<TResult?> source)
			where TResult : class
		{
			if (source is null) throw new ArgumentNullException(nameof(source));
			foreach (var item in source)
			{
				if (item is null)
					continue;
				yield return item;
			}
		}
	}
}
