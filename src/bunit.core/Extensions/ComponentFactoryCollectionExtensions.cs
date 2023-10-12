#if NET5_0_OR_GREATER
using Bunit.ComponentFactories;

namespace Bunit;

/// <summary>
/// Extension methods for using component doubles.
/// </summary>
public static class ComponentFactoryCollectionExtensions
{
	/// <summary>
	/// Configures bUnit to substitute all components of type <typeparamref name="TComponent"/>
	/// with components of type <typeparamref name="TSubstituteComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of component to replace.</typeparam>
	/// <typeparam name="TSubstituteComponent">Type of component to substitute with.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="factories"/> is null.</exception>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection Add<TComponent, TSubstituteComponent>(this ComponentFactoryCollection factories)
		where TComponent : IComponent
		where TSubstituteComponent : IComponent
	{
		Guard.NotNull(factories);

		factories.Add(new GenericComponentFactory<TComponent, TSubstituteComponent>());

		return factories;
	}

	/// <summary>
	/// Configures bUnit to substitute a component of type <typeparamref name="TComponent"/> with the provided <paramref name="instance"/>.
	/// </summary>
	/// <remarks>
	/// Only one <typeparamref name="TComponent"/> component can be substituted with the component (<paramref name="instance"/>).
	/// If there are two or more <typeparamref name="TComponent"/> components in the render tree, an exception is thrown.
	/// </remarks>
	/// <typeparam name="TComponent">Type of component to substitute.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="instance">The instance of the replacement component.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="factories"/> and/or <paramref name="instance"/> is null.</exception>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection Add<TComponent>(this ComponentFactoryCollection factories, TComponent instance)
		where TComponent : IComponent
	{
		Guard.NotNull(factories);
		Guard.NotNull(instance);

		factories.Add(new InstanceComponentFactory<TComponent>(instance));

		return factories;
	}

	/// <summary>
	/// Configures bUnit to substitute components of type <typeparamref name="TComponent"/>
	/// with one created by the provided component <paramref name="factory"/>.
	/// </summary>
	/// <remarks>
	/// The provided <paramref name="factory"/> must return unique instances each time it is called.
	/// Blazor does not allow the same component to exists in multiple places in a render tree.
	/// </remarks>
	/// <typeparam name="TComponent">Type of component to substitute.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="factory">The component factory to use to create substitute components with.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="factories"/> and/or <paramref name="factory"/> is null.</exception>
	public static ComponentFactoryCollection Add<TComponent>(this ComponentFactoryCollection factories, Func<TComponent> factory)
		where TComponent : IComponent
	{
		Guard.NotNull(factories);
		Guard.NotNull(factory);

		factories.Add(new TypeBasedComponentFactory<TComponent>(factory));

		return factories;
	}

	/// <summary>
	/// Configures bUnit to substitute components whose type matches the <paramref name="condition"/>,
	/// with components created by the provided component <paramref name="factory"/>.
	/// </summary>
	/// <remarks>
	/// The provided <paramref name="factory"/> must return unique instances each time it is called.
	/// Blazor does not allow the same component to exists in multiple places in a render tree.
	/// </remarks>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="condition">The condition that must be met for the <paramref name="factory"/> to be used.</param>
	/// <param name="factory">The factory to use to create substitute components with.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="factories"/>, <paramref name="condition"/>, and/or <paramref name="factory"/> is null.</exception>
	public static ComponentFactoryCollection Add(this ComponentFactoryCollection factories, Predicate<Type> condition, Func<Type, IComponent> factory)
	{
		Guard.NotNull(factories);
		Guard.NotNull(condition);
		Guard.NotNull(factory);

		factories.Add(new ConditionalComponentFactory(condition, factory));

		return factories;
	}
}
#endif
