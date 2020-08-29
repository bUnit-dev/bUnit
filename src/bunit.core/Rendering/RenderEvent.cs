using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit.Rendering
{
	public sealed class RenderEvent
	{
		private readonly RenderBatch _renderBatch;

		public RenderTreeFrameCollection Frames { get; }

		internal RenderEvent(RenderBatch renderBatch, RenderTreeFrameCollection frames)
		{
			_renderBatch = renderBatch;
			Frames = frames;
		}

		public (bool rendered, bool changed, bool disposed) GetRenderStatus(IRenderedFragmentBase renderedComponent)
		{
			if (renderedComponent is null)
				throw new ArgumentNullException(nameof(renderedComponent));

			var result = (rendered: false, changed: false, disposed: false);

			if (DidComponentDispose(renderedComponent))
			{
				result.disposed = true;
			}
			else
			{
				(result.rendered, result.changed) = GetRenderAndChangeStatus(renderedComponent);
			}

			return result;
		}

		private bool DidComponentDispose(IRenderedFragmentBase renderedComponent)
		{
			for (var i = 0; i < _renderBatch.DisposedComponentIDs.Count; i++)
				if (_renderBatch.DisposedComponentIDs.Array[i].Equals(renderedComponent.ComponentId))
					return true;

			return false;
		}

		private (bool rendered, bool hasChanges) GetRenderAndChangeStatus(IRenderedFragmentBase renderedComponent)
		{
			var result = (rendered: false, hasChanges: false);

			GetStatus(renderedComponent.ComponentId);

			return result;

			void GetStatus(int componentId)
			{
				for (var i = 0; i < _renderBatch.UpdatedComponents.Count; i++)
				{
					ref var update = ref _renderBatch.UpdatedComponents.Array[i];
					if (update.ComponentId == componentId)
					{
						result.rendered = true;
						result.hasChanges = update.Edits.Count > 0;
						break;
					}
				}

				if (!result.hasChanges)
				{
					GetStatusFromChildren(componentId);
				}
			}

			void GetStatusFromChildren(int componentId)
			{
				var frames = Frames[componentId];
				for (var i = 0; i < frames.Count; i++)
				{
					ref var frame = ref frames.Array[i];
					if (frame.FrameType == RenderTreeFrameType.Component)
					{
						GetStatus(frame.ComponentId);

						if (result.hasChanges)
						{
							break;
						}
					}
				}
			}
		}
	}
}
