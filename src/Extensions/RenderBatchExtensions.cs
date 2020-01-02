using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Egil.RazorComponents.Testing.Extensions
{

    /// <summary>
    /// Helper methods for working with <see cref="RenderBatch"/>.
    /// </summary>
    public static class RenderBatchExtensions
    {
        /// <summary>
        /// Checks a <see cref="RenderBatch"/> for updates to a component with the specified <paramref name="componentId"/>.
        /// </summary>
        /// <param name="renderBatch">RenderBatch to search.</param>
        /// <param name="componentId">Id of component to check for updates to.</param>
        /// <returns>True if <see cref="RenderBatch"/> contains updates to component, false otherwise.</returns>
        public static bool HasUpdatesTo(in this RenderBatch renderBatch, int componentId)
        {
            for (int i = 0; i < renderBatch.UpdatedComponents.Count; i++)
            {
                var update = renderBatch.UpdatedComponents.Array[i];
                if (update.ComponentId == componentId && update.Edits.Count > 0)
                    return true;
            }
            return false;
        }
    }
}
