using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents an render event from a <see cref="ITestRenderer"/>.
	/// </summary>
	public sealed class RenderEvent
	{
		private readonly RenderBatch _renderBatch;

		/// <summary>
		/// A collection of <see cref="ArrayRange{RenderTreeFrame}"/>, accessible via the ID
		/// of the component they are created by.
		/// </summary>
		public RenderTreeFrameCollection Frames { get; }

		/// <summary>
		/// Creates an instance of the <see cref="RenderEvent"/> type.
		/// </summary>
		/// <param name="renderBatch">The <see cref="RenderBatch"/> update from the render event.</param>
		/// <param name="frames">The <see cref="RenderTreeFrameCollection"/> from the current render.</param>
		internal RenderEvent(RenderBatch renderBatch, RenderTreeFrameCollection frames)
		{
			_renderBatch = renderBatch;
			Frames = frames;
		}

		/// <summary>
		/// Gets the render status for a <paramref name="renderedComponent"/>.
		/// </summary>
		/// <param name="renderedComponent">The <paramref name="renderedComponent"/> to get the status for.</param>
		/// <returns>A tuple of statuses indicating whether the rendered component rendered during the render cycle, if it changed or if it was disposed.</returns>
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

		/// <summary>
		/// This method determines if the <paramref name="renderedComponent"/> or any of the
		/// components underneath it in the render tree rendered and whether they they changed
		/// their render tree during render.
		///
		/// It does this by getting the status from the <paramref name="renderedComponent"/>,
		/// then from all its children, using a recursive pattern, where the internal methods
		/// GetStatus and GetStatusFromChildren call each other until there are no more children,
		/// or both a render and a change is found.
		/// </summary>
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
