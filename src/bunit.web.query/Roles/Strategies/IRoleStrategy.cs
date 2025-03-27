using AngleSharp.Dom;
using System.Collections.Generic;

namespace Bunit.Roles.Strategies
{
    /// <summary>
    /// Interface for strategies that find elements by their ARIA role.
    /// </summary>
    public interface IRoleStrategy
    {
        /// <summary>
        /// Finds all elements with the specified role.
        /// </summary>
        /// <param name="context">The element to search within.</param>
        /// <param name="role">The ARIA role to find.</param>
        /// <returns>A collection of elements that match the role.</returns>
        IEnumerable<IElement> FindAll(IElement context, AriaRole role);
    }
} 