#if NET5_0_OR_GREATER
using System;
using Bunit.ComponentFactories;
using Microsoft.AspNetCore.Components;

namespace Bunit;

/// <summary>
/// Extension methods for using component doubles.
/// </summary>
public static class ComponentFactoryCollectionExtensions
{
	/// <summary>
	/// Configures bUnit to replace all components of type <typeparamref name="TComponent"/> with a component
	/// of type <typeparamref name="TReplacementComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of component to replace.</typeparam>
	/// <typeparam name="TReplacementComponent">Type of component to replace with.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection Add<TComponent, TReplacementComponent>(this ComponentFactoryCollection factories)
		where TComponent : IComponent
		where TReplacementComponent : IComponent
	{
		if (factories is null)
			throw new ArgumentNullException(nameof(factories));

		factories.Add(new GenericComponentFactory<TComponent, TReplacementComponent>());

		return factories;
	}

	/// <summary>
	/// Configures bUnit to replace a <typeparamref name="TComponent"/> component in the render tree
	/// with the provided <paramref name="instance"/>.
	/// </summary>
	/// <remarks>
	/// Only one <typeparamref name="TComponent"/> component can be replaced with the replacement component (<paramref name="instance"/>).
	/// If there are two or more <typeparamref name="TComponent"/> components in the render tree, an exception is thrown.
	/// </remarks>
	/// <typeparam name="TComponent">Type of component to replace.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="instance">The instance of the replacement component.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="factories"/> and/or <paramref name="instance"/> is null.</exception>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection Add<TComponent>(this ComponentFactoryCollection factories, TComponent instance)
		where TComponent : IComponent
	{
		if (factories is null)
			throw new ArgumentNullException(nameof(factories));
		if (instance is null)
			throw new ArgumentNullException(nameof(instance));

		factories.Add(new InstanceComponentFactory<TComponent>(instance));

		return factories;
	}
}
#endif
