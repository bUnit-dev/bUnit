using System;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit
{
    /// <summary>
    /// Represents a render event for a <see cref="IRenderedFragment"/> or generally from the <see cref="TestRenderer"/>.
    /// </summary>
    public readonly struct RenderEvent : IEquatable<RenderEvent>
    {
        private readonly TestRenderer _renderer;

        /// <summary>
        /// Gets the related <see cref="RenderBatch"/> from the render.
        /// </summary>
        public RenderBatch RenderBatch { get; }

        /// <summary>
        /// Creates an instance of the <see cref="RenderEvent"/> type.
        /// </summary>
        public RenderEvent(in RenderBatch renderBatch, TestRenderer renderer)
        {
            RenderBatch = renderBatch;
            _renderer = renderer;
        }

        /// <summary>
        /// Checks whether the component with <paramref name="componentId"/> or one or more of 
        /// its sub components was changed during the <see cref="RenderEvent"/>.
        /// </summary>
        /// <param name="componentId">Id of component to check for updates to.</param>
        /// <returns>True if <see cref="RenderEvent"/> contains updates to component, false otherwise.</returns>
        public bool HasChangesTo(int componentId)
        {
            for (int i = 0; i < RenderBatch.UpdatedComponents.Count; i++)
            {
                var update = RenderBatch.UpdatedComponents.Array[i];
                if (update.ComponentId == componentId && update.Edits.Count > 0)
                    return true;
            }
            for (int i = 0; i < RenderBatch.DisposedEventHandlerIDs.Count; i++)
            {
                if (RenderBatch.DisposedEventHandlerIDs.Array[i].Equals(componentId))
                    return true;
            }
            return HasChangedToChildren(_renderer.GetCurrentRenderTreeFrames(componentId));
        }

        /// <summary>
        /// Checks whether the component with <paramref name="componentId"/> or one or more of 
        /// its sub components was rendered during the <see cref="RenderEvent"/>.
        /// </summary>
        /// <param name="componentId">Id of component to check if rendered.</param>
        /// <returns>True if the component or a sub component rendered, false otherwise.</returns>
        public bool DidComponentRender(int componentId)
        {
            for (int i = 0; i < RenderBatch.UpdatedComponents.Count; i++)
            {
                var update = RenderBatch.UpdatedComponents.Array[i];
                if (update.ComponentId == componentId)
                    return true;
            }
            for (int i = 0; i < RenderBatch.DisposedEventHandlerIDs.Count; i++)
            {
                if (RenderBatch.DisposedEventHandlerIDs.Array[i].Equals(componentId))
                    return true;
            }
            return DidChildComponentRender(_renderer.GetCurrentRenderTreeFrames(componentId));
        }

        private bool HasChangedToChildren(ArrayRange<RenderTreeFrame> componentRenderTreeFrames)
        {
            for (int i = 0; i < componentRenderTreeFrames.Count; i++)
            {
                var frame = componentRenderTreeFrames.Array[i];
                if (frame.FrameType == RenderTreeFrameType.Component)
                    if (HasChangesTo(frame.ComponentId))
                        return true;
            }
            return false;
        }

        private bool DidChildComponentRender(ArrayRange<RenderTreeFrame> componentRenderTreeFrames)
        {
            for (int i = 0; i < componentRenderTreeFrames.Count; i++)
            {
                var frame = componentRenderTreeFrames.Array[i];
                if (frame.FrameType == RenderTreeFrameType.Component)
                    if (DidComponentRender(frame.ComponentId))
                        return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(RenderEvent other) => RenderBatch.Equals(other.RenderBatch);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is RenderEvent other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(RenderBatch);

        /// <inheritdoc/>
        public static bool operator ==(RenderEvent left, RenderEvent right) => left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(RenderEvent left, RenderEvent right) => !left.Equals(right);
    }
}