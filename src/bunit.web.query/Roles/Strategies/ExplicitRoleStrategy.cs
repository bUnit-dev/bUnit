using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bunit.Roles.Strategies
{
    /// <summary>
    /// Strategy for finding elements with explicitly declared ARIA roles.
    /// </summary>
    public class ExplicitRoleStrategy : IRoleStrategy
    {
        /// <summary>
        /// Finds all elements with the specified explicit role.
        /// </summary>
        /// <param name="context">The element to search within.</param>
        /// <param name="role">The ARIA role to find.</param>
        /// <returns>A collection of elements that match the role.</returns>
        public IEnumerable<IElement> FindAll(IElement context, AriaRole role)
        {
            var roleString = role.ToString().ToLowerInvariant();
            var result = new List<IElement>();

            try
            {
                // First try to find exact match with the case-sensitive selector
                var exactMatches = context.QuerySelectorAll($"[role='{roleString}']");
                result.AddRange(exactMatches);

                if (result.Count == 0)
                {
                    // If no exact match, find all elements with a role attribute
                    var elementsWithRole = context.QuerySelectorAll("[role]");
                    
                    // Then find case-insensitive matches
                    foreach (var element in elementsWithRole)
                    {
                        var elementRole = element.GetAttribute("role");
                        if (string.Equals(elementRole, roleString, StringComparison.OrdinalIgnoreCase))
                        {
                            result.Add(element);
                        }
                    }
                }
            }
            catch
            {
                // If the selector fails, return an empty collection
            }

            return result;
        }
    }
} 