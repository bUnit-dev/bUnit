using System.Collections;

namespace Bunit;

internal static class ICollectionExtensions
{
	internal static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this ICollection<T> source)
	{
		if (source == null)
		{
			throw new ArgumentNullException(nameof(source));
		}

		return source as IReadOnlyCollection<T> ?? new ReadOnlyCollectionAdapter<T>(source);
	}

	private sealed class ReadOnlyCollectionAdapter<T> : IReadOnlyCollection<T>
	{
		private readonly ICollection<T> source;
		public ReadOnlyCollectionAdapter(ICollection<T> source) => this.source = source;
		public int Count => source.Count;
		public IEnumerator<T> GetEnumerator() => source.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
