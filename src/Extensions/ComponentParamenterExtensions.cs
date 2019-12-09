using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing.Extensions
{
    /// <summary>
    /// Helpful extensions for working with <see cref="ComponentParameter"/> and collections of these.
    /// </summary>
    public static class ComponentParamenterExtensions
    {
        /// <summary>
        /// Creates a <see cref="RenderFragment"/> that will render a component of <typeparamref name="TComponent"/> type,
        /// with the provided <paramref name="parameters"/>. If one or more of the <paramref name="parameters"/> include
        /// a cascading values, the <typeparamref name="TComponent"/> will be wrapped in <see cref="Microsoft.AspNetCore.Components.CascadingValue{TValue}"/> 
        /// components.
        /// </summary>
        /// <typeparam name="TComponent">Type of component to render in the render fragment</typeparam>
        /// <param name="parameters">Parameters to pass to the component</param>
        /// <returns>The <see cref="RenderFragment"/>.</returns>
        public static RenderFragment ToComponentRenderFragment<TComponent>(this IReadOnlyList<ComponentParameter> parameters) where TComponent : class, IComponent
        {
            var cascadingParams = new Queue<ComponentParameter>(parameters.Where(x => x.IsCascadingValue));

            if (cascadingParams.Count > 0)
                return CreateCascadingValueRenderFragment(cascadingParams, parameters);
            else
                return CreateComponentRenderFragment(parameters);

            static RenderFragment CreateCascadingValueRenderFragment(Queue<ComponentParameter> cascadingParams, IReadOnlyList<ComponentParameter> parameters)
            {
                var cp = cascadingParams.Dequeue();
                var cascadingValueType = GetCascadingValueType(cp);
                return builder =>
                {
                    builder.OpenComponent(0, cascadingValueType);
                    if (cp.Name is { })
                        builder.AddAttribute(1, nameof(CascadingValue<object>.Name), cp.Name);

                    builder.AddAttribute(2, nameof(CascadingValue<object>.Value), cp.Value);
                    builder.AddAttribute(3, nameof(CascadingValue<object>.IsFixed), true);

                    if (cascadingParams.Count > 0)
                        builder.AddAttribute(4, nameof(CascadingValue<object>.ChildContent), CreateCascadingValueRenderFragment(cascadingParams, parameters));
                    else
                        builder.AddAttribute(4, nameof(CascadingValue<object>.ChildContent), CreateComponentRenderFragment(parameters));

                    builder.CloseComponent();
                };
            }

            static RenderFragment CreateComponentRenderFragment(IReadOnlyList<ComponentParameter> parameters)
            {
                return builder =>
                {
                    builder.OpenComponent(0, typeof(TComponent));

                    foreach (var parameterValue in parameters.Where(x => !x.IsCascadingValue))
                        builder.AddAttribute(1, parameterValue.Name, parameterValue.Value);

                    builder.CloseComponent();
                };
            }
        }

        private static readonly Type CascadingValueType = typeof(CascadingValue<>);

        private static Type GetCascadingValueType(ComponentParameter parameter)
        {
            if (parameter.Value is null) throw new InvalidOperationException("Cannot get the type of a null object");
            var cascadingValueType = parameter.Value.GetType();
            return CascadingValueType.MakeGenericType(cascadingValueType);
        }
    }
}
