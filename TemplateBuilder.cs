using Microsoft.AspNetCore.Components;
using System;

namespace Egil.RazorComponents.Testing
{
    public class TemplateBuilder<TItem>
    {
        public RenderFragment<TItem> Component<TComponent>()
            where TComponent : class, IComponent
        {
            return Component(typeof(TComponent));
        }

        public RenderFragment<TItem> Component(Type componentType)
        {
            return item => builder =>
            {
                builder.OpenComponent(1, componentType);
                builder.CloseComponent();
            };
        }
    }
}
