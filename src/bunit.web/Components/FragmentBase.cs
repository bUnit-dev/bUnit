using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <summary>
    /// Represents a fragment that can be used in <see cref="SnapshotTest"/> or <see cref="Fixture"/>.
    /// </summary>
    public abstract class FragmentBase : IComponent
    {
        internal static void NoopTestMethod() { }
        internal static Task NoopTestMethodAsync() => Task.CompletedTask;

        /// <summary>
        /// Gets or sets the child content of the fragment.
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; } = default!;

        /// <inheritdoc />
        public void Attach(RenderHandle renderHandle) { }

        /// <inheritdoc />
        public virtual Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            if (ChildContent is null) throw new InvalidOperationException($"No {nameof(ChildContent)} specified in test component.");
            return Task.CompletedTask;
        }
    }
}
