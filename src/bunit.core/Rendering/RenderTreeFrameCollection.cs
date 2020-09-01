using System.Collections.Generic;

using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a collection of <see cref="ArrayRange{RenderTreeFrame}"/>.
	/// </summary>
	public sealed class RenderTreeFrameCollection
	{
		private readonly Dictionary<int, ArrayRange<RenderTreeFrame>> _currentRenderTree = new Dictionary<int, ArrayRange<RenderTreeFrame>>();

		/// <summary>
		/// Gets the <see cref="ArrayRange{RenderTreeFrame}"/> associated with the <paramref name="componentId"/>.
		/// </summary>
		/// <param name="componentId"></param>
		/// <returns></returns>
		public ArrayRange<RenderTreeFrame> this[int componentId] => _currentRenderTree[componentId];

		/// <summary>
		/// Creates an instance of the <see cref="RenderTreeFrameCollection"/>,
		/// </summary>
		internal RenderTreeFrameCollection() { }

		/// <summary>
		/// Checks whether the collection contains a <see cref="ArrayRange{RenderTreeFrame}"/> for the <paramref name="componentId"/>.
		/// </summary>
		public bool Contains(int componentId) => _currentRenderTree.ContainsKey(componentId);

		internal void Add(int componentId, ArrayRange<RenderTreeFrame> frames) => _currentRenderTree.Add(componentId, frames);
	}
}
