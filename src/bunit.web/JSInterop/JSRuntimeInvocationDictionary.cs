using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Bunit.JSInterop
{
	/// <summary>
	/// Represents a dictionary of <see cref="JSRuntimeInvocation"/>, keyed by their identifier.
	/// </summary>
	[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Implementing IReadOnlyDictionary would require a different GetEnumerator implementation, which is less useful in testing scenarios. This is however a dictionary.")]
	public sealed class JSRuntimeInvocationDictionary : IReadOnlyCollection<JSRuntimeInvocation>
	{
		private readonly Dictionary<string, List<JSRuntimeInvocation>> invocations = new(StringComparer.Ordinal);

		/// <summary>
		/// Gets all invocations for a specific <paramref name="identifier"/>.
		/// </summary>
		/// <param name="identifier">The identifier to get invocations for.</param>
		/// <returns>An <see cref="IReadOnlyList{JSRuntimeInvocation}"/>.</returns>
		public IReadOnlyList<JSRuntimeInvocation> this[string identifier]
		{
			get => invocations.ContainsKey(identifier)
				? invocations[identifier]
				: Array.Empty<JSRuntimeInvocation>();
		}

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

			if (!invocations.ContainsKey(invocation.Identifier))
			{
				invocations.Add(invocation.Identifier, new List<JSRuntimeInvocation>());
			}

			invocations[invocation.Identifier].Add(invocation);
		}
	}
}
