using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing.Extensions
{
    public static class BlazorExtensions
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
    }
}
