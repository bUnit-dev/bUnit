#if NET5_0_OR_GREATER
using System;
using Microsoft.AspNetCore.Components;

namespace Bunit;

/// <summary>
/// Represents a component factory.
/// </summary>
public interface IComponentFactory
{
	/// <summary>
	/// Check if the factory can create a component of type <paramref name="componentType"/> or a replacement for it.
	/// </summary>
	/// <param name="componentType">The type that should be created or replaced.</param>
	/// <returns>True if the factory can create the type; false otherwise.</returns>
	bool CanCreate(Type componentType);

	/// <summary>
	/// Create a component of type <paramref name="componentType"/> or a replacement for it.
	/// </summary>
	/// <param name="componentType">The type of component to create.</param>
	IComponent Create(Type componentType);
}
#endif
