using System;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a render event from a <see cref="ITestRenderer"/>.
	/// </summary>
	public sealed class RenderEvent
	{
		private readonly ITestRenderer _renderer;
		private readonly RenderBatch _renderBatch;

		/// <summary>
		/// Gets the related <see cref="RenderBatch"/> from the render.
		/// </summary>
		public ref readonly RenderBatch RenderBatch => ref _renderBatch;

		/// <summary>
		/// Creates an instance of the <see cref="RenderEvent"/> type.
		/// </summary>
		public RenderEvent(in RenderBatch renderBatch, ITestRenderer renderer)
		{
			_renderBatch = renderBatch;
			_renderer = renderer;
		}

		/// <summary>
		/// Checks whether the a component with <paramref name="componentId"/> or one or more of 
		/// its sub components was changed during the <see cref="RenderEvent"/>.
		/// </summary>
		/// <param name="componentId">Id of component to check for updates to.</param>
		/// <returns>True if <see cref="RenderEvent"/> contains updates to component, false otherwise.</returns>
		public bool HasMarkupChanges(int componentId)
		{
			return HasChangesToRoot(componentId);

			bool HasChangesToRoot(int componentId)
			{
				for (var i = 0; i < _renderBatch.UpdatedComponents.Count; i++)
				{
					ref var update = ref _renderBatch.UpdatedComponents.Array[i];
					if (update.ComponentId == componentId && update.Edits.Count > 0)
						return true;
				}

				var renderFrames = _renderer.GetCurrentRenderTreeFrames(componentId);
				return HasChangedToChildren(renderFrames);
			}

			bool HasChangedToChildren(ArrayRange<RenderTreeFrame> componentRenderTreeFrames)
			{
				for (var i = 0; i < componentRenderTreeFrames.Count; i++)
				{
					ref var frame = ref componentRenderTreeFrames.Array[i];
					if (frame.FrameType == RenderTreeFrameType.Component)
						if (HasChangesToRoot(frame.ComponentId))
							return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Checks whether the a component with <paramref name="componentId"/> was disposed.
		/// </summary>
		/// <param name="componentId">Id of component to check.</param>
		/// <returns>True if component was disposed, false otherwise.</returns>
		public bool DidComponentDispose(int componentId)
		{
			for (var i = 0; i < _renderBatch.DisposedComponentIDs.Count; i++)
				if (_renderBatch.DisposedComponentIDs.Array[i].Equals(componentId))
					return true;

			return false;
		}

		/// <summary>
		/// Checks whether the a component with <paramref name="componentId"/> or one or more of 
		/// its sub components was rendered during the <see cref="RenderEvent"/>.
		/// </summary>
		/// <param name="componentId">Id of component to check if rendered.</param>
		/// <returns>True if the component or a sub component rendered, false otherwise.</returns>
		public bool DidComponentRender(int componentId)
		{
			return DidComponentRenderRoot(componentId);

			bool DidComponentRenderRoot(int componentId)
			{
				for (var i = 0; i < _renderBatch.UpdatedComponents.Count; i++)
				{
					ref var update = ref _renderBatch.UpdatedComponents.Array[i];
					if (update.ComponentId == componentId)
						return true;
				}

				return DidChildComponentRender(componentId);
			}

			bool DidChildComponentRender(int componentId)
			{
				var frames = _renderer.GetCurrentRenderTreeFrames(componentId);

				for (var i = 0; i < frames.Count; i++)
				{
					ref var frame = ref frames.Array[i];
					if (frame.FrameType == RenderTreeFrameType.Component)
						if (DidComponentRenderRoot(frame.ComponentId))
							return true;
				}

				return false;
			}
		}
	}
}
