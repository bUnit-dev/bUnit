﻿using System;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit
{
    /// <summary>
    /// Represents a render event for a <see cref="IRenderedFragment"/> or generally from the <see cref="TestRenderer"/>.
    /// </summary>
    public sealed class RenderEvent
    {
        private readonly TestRenderer _renderer;
        private readonly RenderBatch _renderBatch;

        /// <summary>
        /// Gets the related <see cref="RenderBatch"/> from the render.
        /// </summary>
        public ref readonly RenderBatch RenderBatch => ref _renderBatch;

        /// <summary>
        /// Creates an instance of the <see cref="RenderEvent"/> type.
        /// </summary>
        public RenderEvent(in RenderBatch renderBatch, TestRenderer renderer)
        {
            _renderBatch = renderBatch;
            _renderer = renderer;
        }

        /// <summary>
        /// Checks whether the <paramref name="renderedFragment"/> or one or more of 
        /// its sub components was changed during the <see cref="RenderEvent"/>.
        /// </summary>
        /// <param name="renderedFragment">Component to check for updates to.</param>
        /// <returns>True if <see cref="RenderEvent"/> contains updates to component, false otherwise.</returns>
        public bool HasChangesTo(IRenderedFragment renderedFragment)
            => HasChangesTo((renderedFragment ?? throw new ArgumentNullException(nameof(renderedFragment))).ComponentId);

        /// <summary>
        /// Checks whether the <paramref name="renderedFragment"/> or one or more of 
        /// its sub components was rendered during the <see cref="RenderEvent"/>.
        /// </summary>
        /// <param name="renderedFragment">Component to check if rendered.</param>
        /// <returns>True if the component or a sub component rendered, false otherwise.</returns>
        public bool DidComponentRender(IRenderedFragment renderedFragment)
            => DidComponentRender((renderedFragment ?? throw new ArgumentNullException(nameof(renderedFragment))).ComponentId);

        private bool HasChangesTo(int componentId)
        {
            for (int i = 0; i < _renderBatch.UpdatedComponents.Count; i++)
            {
                var update = _renderBatch.UpdatedComponents.Array[i];
                if (update.ComponentId == componentId && update.Edits.Count > 0)
                    return true;
            }
            for (int i = 0; i < _renderBatch.DisposedEventHandlerIDs.Count; i++)
            {
                if (_renderBatch.DisposedEventHandlerIDs.Array[i].Equals(componentId))
                    return true;
            }
            return HasChangedToChildren(_renderer.GetCurrentRenderTreeFrames(componentId));
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

        private bool DidComponentRender(int componentId)
        {
            for (int i = 0; i < _renderBatch.UpdatedComponents.Count; i++)
            {
                var update = _renderBatch.UpdatedComponents.Array[i];
                if (update.ComponentId == componentId)
                    return true;
            }
            for (int i = 0; i < _renderBatch.DisposedEventHandlerIDs.Count; i++)
            {
                if (_renderBatch.DisposedEventHandlerIDs.Array[i].Equals(componentId))
                    return true;
            }
            return DidChildComponentRender(_renderer.GetCurrentRenderTreeFrames(componentId));
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
    }
}