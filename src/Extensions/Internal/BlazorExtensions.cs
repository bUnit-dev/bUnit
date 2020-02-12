using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <summary>
    /// Extensions for Blazor types.
    /// </summary>
    internal static class BlazorExtensions
    {
        private static readonly Type CascadingValueType = typeof(CascadingValue<>);

        /// <summary>
        /// Checks whether the <paramref name="component"/> is a <see cref="CascadingValue{TValue}"/> component.
        /// </summary>
        /// <param name="component"></param>
        public static bool IsCascadingValueComponent(this IComponent component)
        {
            if (component is null) return false;
            var type = component.GetType();
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(CascadingValueType);
        }

        /// <summary>
        /// Creates a <see cref="RenderFragment"/> that will render the <paramref name="markup"/>.
        /// </summary>
        /// <param name="markup">Markup to render</param>
        /// <returns>The <see cref="RenderFragment"/>.</returns>
        public static RenderFragment ToMarkupRenderFragment(this string markup)
        {
            return builder => builder.AddMarkupContent(0, markup);
        }

        /// <summary>
        /// Creates a <see cref="RenderFragment"/> that will render a component of <typeparamref name="TComponent"/> type,
        /// with the provided <paramref name="parameters"/>.
        /// </summary>
        /// <typeparam name="TComponent">Type of component to render in the render fragment</typeparam>
        /// <param name="parameters">Parameters to pass to the component</param>
        /// <returns>The <see cref="RenderFragment"/>.</returns>
        public static RenderFragment ToComponentRenderFragment<TComponent>(this ParameterView parameters) where TComponent : class, IComponent
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(TComponent));

                foreach (var parameterValue in parameters)
                    builder.AddAttribute(1, parameterValue.Name, parameterValue.Value);

                builder.CloseComponent();
            };
        }
    }
}
