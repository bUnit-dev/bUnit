using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Egil.RazorComponents.Testing
{
    internal class RenderFragmentWrapper : ComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (ChildContent is null) throw new ArgumentNullException(nameof(ChildContent));
            builder.AddContent(0, ChildContent);
        }
    }
}
