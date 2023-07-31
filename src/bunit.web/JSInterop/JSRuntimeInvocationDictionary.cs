using System.Collections;

namespace Bunit;

/// <summary>
/// Represents a dictionary of <see cref="JSRuntimeInvocation"/>, keyed by their identifier.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Name makes sense in this context.")]
public sealed class JSRuntimeInvocationDictionary : IReadOnlyCollection<JSRuntimeInvocation>
{
	private readonly Dictionary<string, List<JSRuntimeInvocation>> invocations = new(StringComparer.Ordinal);

	/// <summary>
	/// Gets all invocations for a specific <paramref name="identifier"/>.
	/// </summary>
	/// <param name="identifier">The identifier to get invocations for.</param>
	/// <returns>An <see cref="IReadOnlyList{JSRuntimeInvocation}"/>.</returns>
	public IReadOnlyList<JSRuntimeInvocation> this[string identifier] => invocations.TryGetValue(identifier, out var value) ? value : Array.Empty<JSRuntimeInvocation>();

	/// <summary>
	/// Gets a read only collection of all the identifiers used in invocations in this dictionary.
	/// </summary>
	public IReadOnlyCollection<string> Identifiers => invocations.Keys;

	/// <summary>
	/// Gets the total number of invocations registered in the dictionary.
	/// </summary>
	public int Count { get; private set; }

	/// <summary>
	/// Gets an <see cref="IEnumerator{JSRuntimeInvocation}"/> that will
	/// iterate over all invocations in the dictionary.
	/// </summary>
	/// <returns>An iterator with all the <see cref="JSRuntimeInvocation"/> registered in this dictionary.</returns>
	public IEnumerator<JSRuntimeInvocation> GetEnumerator()
	{
		foreach (var kvp in invocations)
		{
			foreach (var item in kvp.Value)
			{
				yield return item;
			}
		}
	}

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	/// Register a new invocation with this dictionary.
	/// </summary>
	internal void RegisterInvocation(JSRuntimeInvocation invocation)
	{
		Count++;

		if (!invocations.TryGetValue(invocation.Identifier, out var value))
		{
			value = new List<JSRuntimeInvocation>();
			invocations.Add(invocation.Identifier, value);
		}

		value.Add(invocation);
	}
}
