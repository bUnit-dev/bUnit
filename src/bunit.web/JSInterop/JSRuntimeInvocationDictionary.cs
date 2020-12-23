using System;
using System.Collections;
using System.Collections.Generic;

namespace Bunit.JSInterop
{
	/// <summary>
	/// Represents a dictionary of <see cref="JSRuntimeInvocation"/>, keyed by their identifier.
	/// </summary>
	public sealed class JSRuntimeInvocationDictionary : IReadOnlyCollection<JSRuntimeInvocation>
	{
		private readonly Dictionary<string, List<JSRuntimeInvocation>> _invocations = new();
		private int _count;

		/// <summary>
		/// Gets all invocations for a specific <paramref name="identifier"/>.
		/// </summary>
		/// <param name="identifier">The identifier to get invocations for.</param>
		/// <returns>An <see cref="IReadOnlyList{JSRuntimeInvocation}"/>.</returns>
		public IReadOnlyList<JSRuntimeInvocation> this[string identifier]
		{
			get => _invocations.ContainsKey(identifier)
				? _invocations[identifier]
				: Array.Empty<JSRuntimeInvocation>();
		}

		/// <summary>
		/// Gets a read only collection of all the identifiers used in invocations in this dictionary.
		/// </summary>
		public IReadOnlyCollection<string> Identifiers => _invocations.Keys;

		/// <summary>
		/// Gets the total number of invocations registered in the dictionary.
		/// </summary>
		public int Count => _count;

		/// <summary>
		/// Gets an <see cref="IEnumerator{JSRuntimeInvocation}"/> that will
		/// iterate over all invocations in the dictionary.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<JSRuntimeInvocation> GetEnumerator()
		{
			foreach (var kvp in _invocations)
			{
				foreach (var item in kvp.Value)
				{
					yield return item;
				}
			}
		}

		/// <summary>
		/// Register a new invocation with this dictionary.
		/// </summary>
		internal void RegisterInvocation(JSRuntimeInvocation invocation)
		{
			_count++;
			if (!_invocations.ContainsKey(invocation.Identifier))
			{
				_invocations.Add(invocation.Identifier, new List<JSRuntimeInvocation>());
			}
			_invocations[invocation.Identifier].Add(invocation);
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
