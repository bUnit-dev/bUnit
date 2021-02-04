using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a dictionary of <see cref="ArrayRange{RenderTreeFrame}"/> keyed by the owning component's component id.
	/// </summary>
	public sealed class RenderTreeFrameDictionary : IReadOnlyDictionary<int, ArrayRange<RenderTreeFrame>>
	{
		private readonly Dictionary<int, ArrayRange<RenderTreeFrame>> currentRenderTree = new();

		/// <inheritdoc/>
		public int Count => currentRenderTree.Count;

		/// <inheritdoc/>
		public IEnumerable<int> Keys => currentRenderTree.Keys;

		/// <inheritdoc/>
		public IEnumerable<ArrayRange<RenderTreeFrame>> Values => currentRenderTree.Values;

		/// <summary>
		/// Gets the <see cref="ArrayRange{RenderTreeFrame}"/> associated with the <paramref name="componentId"/>.
		/// </summary>
		/// <param name="componentId">Id of the component whose <see cref="RenderTreeFrame"/> to get.</param>
		/// <returns>The <see cref="RenderTreeFrame"/> for the component with <paramref name="componentId"/>.</returns>
		public ArrayRange<RenderTreeFrame> this[int componentId] => currentRenderTree[componentId];

		/// <summary>
		/// Initializes a new instance of the <see cref="RenderTreeFrameDictionary"/> class.
		/// </summary>
		internal RenderTreeFrameDictionary() { }

		/// <summary>
		/// Checks whether the collection contains a <see cref="ArrayRange{RenderTreeFrame}"/> for the <paramref name="componentId"/>.
		/// </summary>
		public bool Contains(int componentId) => currentRenderTree.ContainsKey(componentId);

		/// <inheritdoc/>
		public bool ContainsKey(int key) => currentRenderTree.ContainsKey(key);

		/// <inheritdoc/>
		public bool TryGetValue(int key, out ArrayRange<RenderTreeFrame> value) => currentRenderTree.TryGetValue(key, out value);

		/// <inheritdoc/>
		public IEnumerator<KeyValuePair<int, ArrayRange<RenderTreeFrame>>> GetEnumerator() => currentRenderTree.GetEnumerator();

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator() => currentRenderTree.GetEnumerator();

		internal void Add(int componentId, ArrayRange<RenderTreeFrame> frames) => currentRenderTree.Add(componentId, frames);
	}
}
