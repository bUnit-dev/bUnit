using AngleSharp.Dom;
using Bunit.Web.AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bunit.Roles
{
    /// <summary>
    /// Extension methods for <see cref="INodeList"/> to find elements by ARIA role.
    /// </summary>
    public static class INodeListExtensions
    {
        /// <summary>
        /// Finds an element by its ARIA role.
        /// </summary>
        /// <param name="nodes">The node list to search within.</param>
        /// <param name="role">The ARIA role to find.</param>
        /// <param name="options">Additional options for finding the element.</param>
        /// <returns>The first element that matches the role.</returns>
        /// <exception cref="RoleNotFoundException">Thrown when no element with the specified role is found.</exception>
        public static IElement FindByRole(this INodeList nodes, AriaRole role, FindByRoleOptions? options = null)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            // INodeList in bUnit usually contains a single document or document fragment as its root,
            // followed by all the child nodes of that root. We need to create a context that includes all of these
            // nodes for searching.
            var rootNodes = nodes.OfType<IElement>().ToList();
            if (rootNodes.Count == 0)
                throw new RoleNotFoundException(role, options?.Name, Array.Empty<string>());

            // The first node might be the document/fragment itself
            var rootElement = rootNodes[0];
            try
            {
                return rootElement.FindByRole(role, options);
            }
            catch (RoleNotFoundException)
            {
                // If not found in the root element, try to search in the entire node list
                var allElements = rootNodes.Skip(1).ToList();
                if (allElements.Count == 0)
                    throw; // Re-throw if there are no other elements

                // Try to find a match in any of the elements
                foreach (var element in allElements)
                {
                    try
                    {
                        var match = element.FindByRole(role, options);
                        if (match != null)
                            return match;
                    }
                    catch (RoleNotFoundException)
                    {
                        // Continue to the next element
                    }
                }

                // If we get here, no match was found
                var availableRoles = GetAvailableRoles(nodes, options?.Hidden ?? false);
                throw new RoleNotFoundException(role, options?.Name, availableRoles);
            }
        }

        /// <summary>
        /// Gets a list of available roles in the node list.
        /// </summary>
        /// <param name="nodes">The node list to search within.</param>
        /// <param name="includeHidden">Whether to include hidden elements.</param>
        /// <returns>A sorted list of available roles.</returns>
        public static IReadOnlyList<string> GetAvailableRoles(this INodeList nodes, bool includeHidden = false)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            var roles = new List<string>();
            var elements = nodes.OfType<IElement>();

            foreach (var element in elements)
            {
                var elementRoles = RoleQueryExtensions.GetAvailableRoles(element, includeHidden);
                roles.AddRange(elementRoles);
            }

            return roles.Distinct().OrderBy(r => r).ToList();
        }
    }
} 