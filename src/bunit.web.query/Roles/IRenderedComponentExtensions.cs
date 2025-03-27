using AngleSharp.Dom;
using Bunit.Web;
using System;

namespace Bunit.Roles
{
    /// <summary>
    /// Extension methods for <see cref="IRenderedComponent{TComponent}"/> to find elements by ARIA role.
    /// </summary>
    public static class IRenderedComponentExtensions
    {
        /// <summary>
        /// Finds an element by its ARIA role.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <param name="renderedComponent">The rendered component.</param>
        /// <param name="role">The ARIA role to find.</param>
        /// <param name="options">Additional options for finding the element.</param>
        /// <returns>The first element that matches the role.</returns>
        /// <exception cref="RoleNotFoundException">Thrown when no element with the specified role is found.</exception>
        public static IElement FindByRole<TComponent>(this IRenderedComponent<TComponent> renderedComponent, AriaRole role, FindByRoleOptions? options = null)
            where TComponent : IComponent
        {
            if (renderedComponent == null)
                throw new ArgumentNullException(nameof(renderedComponent));

            return renderedComponent.Nodes.FindByRole(role, options);
        }

        /// <summary>
        /// Gets a list of available roles in the component.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <param name="renderedComponent">The rendered component.</param>
        /// <param name="includeHidden">Whether to include hidden elements.</param>
        /// <returns>A sorted list of available roles.</returns>
        public static System.Collections.Generic.IReadOnlyList<string> GetAvailableRoles<TComponent>(this IRenderedComponent<TComponent> renderedComponent, bool includeHidden = false)
            where TComponent : IComponent
        {
            if (renderedComponent == null)
                throw new ArgumentNullException(nameof(renderedComponent));

            return renderedComponent.Nodes.GetAvailableRoles(includeHidden);
        }
    }
} 