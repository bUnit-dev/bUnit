using System;

namespace Bunit
{
    /// <summary>
    /// Represents an exception that is thrown when a search for a component did not succeed.
    /// </summary>
    public class ComponentNotFoundException : Exception
    {
        /// <summary>
        /// Creates an instance of the <see cref="ComponentNotFoundException"/> type.
        /// </summary>
        /// <param name="componentType">The type of component that was not found.</param>
        public ComponentNotFoundException(Type componentType) : base($"A component of type {componentType?.Name} was not found in the render tree.")
        {
        }
    }
}
