using System;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public class RazorComponentFixture
    {
        public virtual ComponentBuilder<TComponent> Component<TComponent>() where TComponent : ComponentBase
        {
            return new ComponentBuilder<TComponent>();
        }

        public virtual ComponentBuilder<TComponent, TItem> Component<TComponent, TItem>() where TComponent : ComponentBase
        {
            return new ComponentBuilder<TComponent, TItem>().WithItems(new TItem[0]);
        }

        public virtual FragmentBuilder Fragment(Type componentType)
        {
            return new FragmentBuilder(componentType);
        }

        public virtual FragmentBuilder<TComponent> Fragment<TComponent>() where TComponent : ComponentBase
        {
            return new FragmentBuilder<TComponent>();
        }
    }
}
