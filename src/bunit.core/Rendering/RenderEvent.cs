using System;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents an render event from a <see cref="ITestRenderer"/>.
	/// </summary>
	public sealed class RenderEvent
	{
		private readonly RenderBatch renderBatch;

		/// <summary>
		/// Gets a collection of <see cref="ArrayRange{RenderTreeFrame}"/>, accessible via the ID
		/// of the component they are created by.
		/// </summary>
		public RenderTreeFrameDictionary Frames { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="RenderEvent"/> class.
		/// </summary>
		/// <param name="renderBatch">The <see cref="RenderBatch"/> update from the render event.</param>
		/// <param name="frames">The <see cref="RenderTreeFrameDictionary"/> from the current render.</param>
		internal RenderEvent(RenderBatch renderBatch, RenderTreeFrameDictionary frames)
		{
			this.renderBatch = renderBatch;
			Frames = frames;
		}

		/// <summary>
		/// Gets the render status for a <paramref name="renderedComponent"/>.
		/// </summary>
		/// <param name="renderedComponent">The <paramref name="renderedComponent"/> to get the status for.</param>
		/// <returns>A tuple of statuses indicating whether the rendered component rendered during the render cycle, if it changed or if it was disposed.</returns>
		public (bool Rendered, bool Changed, bool Disposed) GetRenderStatus(IRenderedFragmentBase renderedComponent)
		{
			if (renderedComponent is null)
				throw new ArgumentNullException(nameof(renderedComponent));

			var result = (Rendered: false, Changed: false, Disposed: false);

			if (DidComponentDispose(renderedComponent))
			{
				result.Disposed = true;
			}
			else
			{
				(result.Rendered, result.Changed) = GetRenderAndChangeStatus(renderedComponent);
			}

			return result;
		}

		private bool DidComponentDispose(IRenderedFragmentBase renderedComponent)
		{
			for (var i = 0; i < renderBatch.DisposedComponentIDs.Count; i++)
			{
				if (renderBatch.DisposedComponentIDs.Array[i].Equals(renderedComponent.ComponentId))
				{
					return true;
				}
			}

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
		private (bool Rendered, bool HasChanges) GetRenderAndChangeStatus(IRenderedFragmentBase renderedComponent)
		{
			var result = (Rendered: false, HasChanges: false);

			GetStatus(renderedComponent.ComponentId);

			return result;

			void GetStatus(int componentId)
			{
				for (var i = 0; i < renderBatch.UpdatedComponents.Count; i++)
				{
					ref var update = ref renderBatch.UpdatedComponents.Array[i];
					if (update.ComponentId == componentId)
					{
						result.Rendered = true;
						result.HasChanges = update.Edits.Count > 0;
						break;
					}
				}

				if (!result.HasChanges)
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

						if (result.HasChanges)
						{
							break;
						}
					}
				}
			}
		}
	}
}
