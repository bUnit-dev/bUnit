using System.Collections.Generic;

using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit.Rendering
{
	public sealed class RenderTreeFrameCollection
	{
		private Dictionary<int, ArrayRange<RenderTreeFrame>> _currentRenderTree = new Dictionary<int, ArrayRange<RenderTreeFrame>>();

		public ArrayRange<RenderTreeFrame> this[int componentId] => _currentRenderTree[componentId];

		internal RenderTreeFrameCollection() { }

		public bool Contains(int componentId) => _currentRenderTree.ContainsKey(componentId);

		internal void Add(int componentId, ArrayRange<RenderTreeFrame> frames) => _currentRenderTree.Add(componentId, frames);

		public bool TryFindComponent<TComponent>(int parentComponentId, out int componentId, out TComponent component)
		{
			var result = false;
			componentId = -1;
			component = default!;

			var frames = _currentRenderTree[parentComponentId];

			for (var i = 0; i < frames.Count; i++)
			{
				ref var frame = ref frames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					if (frame.Component is TComponent c)
					{
						componentId = frame.ComponentId;
						component = c;
						result = true;
						break;
					}

					if (TryFindComponent(frame.ComponentId, out componentId, out component))
					{
						result = true;
						break;
					}
				}
			}

			return result;
		}
	}
}
