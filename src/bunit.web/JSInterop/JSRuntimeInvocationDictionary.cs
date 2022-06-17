using System.Collections;
using System.Collections.Concurrent;

namespace Bunit;

/// <summary>
/// Represents a dictionary of <see cref="JSRuntimeInvocation"/>, keyed by their identifier.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Name makes sense in this context.")]
public sealed class JSRuntimeInvocationDictionary : IReadOnlyCollection<JSRuntimeInvocation>
{
	private readonly ConcurrentDictionary<string, List<JSRuntimeInvocation>> invocations = new(StringComparer.Ordinal);

	/// <summary>
	/// Gets all invocations for a specific <paramref name="identifier"/>.
	/// </summary>
	/// <param name="identifier">The identifier to get invocations for.</param>
	/// <returns>An <see cref="IReadOnlyList{JSRuntimeInvocation}"/>.</returns>
	public IReadOnlyList<JSRuntimeInvocation> this[string identifier]
		=> invocations.GetOrAdd(identifier, _ => new List<JSRuntimeInvocation>());

	/// <summary>
	/// Gets a read only collection of all the identifiers used in invocations in this dictionary.
	/// </summary>
	public IReadOnlyCollection<string> Identifiers => invocations.Keys.ToReadOnlyCollection();

	/// <summary>
	/// Gets the total number of invocations registered in the dictionary.
	/// </summary>
	public int Count => invocations.Count;

	/// <summary>
	/// Gets an <see cref="IEnumerator{JSRuntimeInvocation}"/> that will
	/// iterate over all invocations in the dictionary.
	/// </summary>
	/// <returns>An iterator with all the <see cref="JSRuntimeInvocation"/> registered in this dictionary.</returns>
	public IEnumerator<JSRuntimeInvocation> GetEnumerator()
		=> invocations.SelectMany(kvp => kvp.Value).GetEnumerator();

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	/// Register a new invocation with this dictionary.
	/// </summary>
	internal void RegisterInvocation(JSRuntimeInvocation invocation)
		=> invocations.AddOrUpdate(invocation.Identifier,
			_ => new List<JSRuntimeInvocation> { invocation },
			(_, list) =>
			{
				list.Add(invocation);
				return list;
			});
}
