using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit
{
    /// <summary>
    /// Represents a render event for a <see cref="IRenderedFragment"/> or generally from the <see cref="TestRenderer"/>.
    /// </summary>
    public readonly struct RenderEvent
    {
        /// <summary>
        /// Gets the related <see cref="RenderBatch"/> from the render.
        /// </summary>
        public RenderBatch RenderBatch { get; }

        /// <summary>
        /// Creates an instance of the <see cref="RenderEvent"/> type.
        /// </summary>
        public RenderEvent(in RenderBatch renderBatch)
        {
            RenderBatch = renderBatch;
        }
    }
}