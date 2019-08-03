using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace ComponentLib
{
    public class Fixture : ComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; private set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<CascadingValue<Fixture>>(10);
            builder.AddAttribute(11, "Value", this);
            builder.AddAttribute(12, "IsFixed", true);
            builder.AddAttribute(13, "ChildContent", ChildContent);
            builder.CloseComponent();
        }
    }
}
