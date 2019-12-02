using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public abstract class FragmentBase : IComponent
    {
        [Parameter] public RenderFragment ChildContent { get; set; } = default!;

        public void Attach(RenderHandle renderHandle) { }

        public virtual Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            if (ChildContent is null) throw new InvalidOperationException($"No {nameof(ChildContent)} specified in test component.");
            return Task.CompletedTask;
        }
    }
}
