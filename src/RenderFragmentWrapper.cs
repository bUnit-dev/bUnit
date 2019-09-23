using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Egil.RazorComponents.Testing
{
    public class RenderFragmentWrapper : ComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (ChildContent is null) throw new ArgumentNullException(nameof(ChildContent));
            if (builder is null) throw new ArgumentNullException(nameof(builder));

            builder.AddContent(0, ChildContent);
        }
    }
}
