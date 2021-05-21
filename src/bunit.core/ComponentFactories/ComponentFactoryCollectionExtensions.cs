#if NET5_0_OR_GREATER
using System;
using Bunit.ComponentFactories;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// Extension methods for using component doubles.
	/// </summary>
	public static class ComponentFactoryCollectionExtensions
	{
		/// <summary>
		/// Configures bUnit to use replace all components of type <typeparamref name="TComponent"/> (including derived components)
		/// with a <see cref="Dummy{TComponent}"/> component in the render tree.
		/// </summary>
		/// <typeparam name="TComponent">The type of component to replace with a <see cref="Dummy{TComponent}"/> component.</typeparam>
		/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
		/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
		public static ComponentFactoryCollection UseDummyFor<TComponent>(this ComponentFactoryCollection factories)
			where TComponent : IComponent
		{
			return UseDummyFor(factories, CreatePredicate(typeof(TComponent)));

			static Predicate<Type> CreatePredicate(Type componentTypeToStub)
				=> componentType => componentType == componentTypeToStub || componentType.IsAssignableTo(componentTypeToStub);
		}

		/// <summary>
		/// Configures bUnit to use replace all components whose type make the <paramref name="componentTypePredicate"/> predicate return <c>true</c>
		/// with a <see cref="Dummy{TComponent}"/> component in the render tree.
		/// </summary>
		/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
		/// <param name="componentTypePredicate">The predicate which decides if a component should be replaced with a <see cref="Dummy{TComponent}"/> component.</param>
		/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
		public static ComponentFactoryCollection UseDummyFor(this ComponentFactoryCollection factories, Predicate<Type> componentTypePredicate)
		{
			if (factories is null)
				throw new ArgumentNullException(nameof(factories));
			if (componentTypePredicate is null)
				throw new ArgumentNullException(nameof(componentTypePredicate));

			factories.Add(new DummyComponentFactory(componentTypePredicate));
			return factories;
		}

		/// <summary>
		/// Configures bUnit to use replace all components of type <typeparamref name="TComponent"/> (including derived components)
		/// with a <see cref="Stub{TComponent}"/> component in the render tree.
		/// </summary>
		/// <typeparam name="TComponent">The type of component to replace with a <see cref="Stub{TComponent}"/> component.</typeparam>
		/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
		/// <param name="options">Optional options that configures how the <see cref="Stub{TComponent}"/> renders markup.</param>
		/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
		public static ComponentFactoryCollection UseStubFor<TComponent>(this ComponentFactoryCollection factories, StubOptions? options = null)
			where TComponent : IComponent
		{
			return UseStubFor(factories, CreatePredicate(typeof(TComponent)), options);

			static Predicate<Type> CreatePredicate(Type componentTypeToStub)
				=> componentType => componentType == componentTypeToStub || componentType.IsAssignableTo(componentTypeToStub);
		}

		/// <summary>
		/// Configures bUnit to use replace all components whose type make the <paramref name="componentTypePredicate"/> predicate return <c>true</c>
		/// with a <see cref="Stub{TComponent}"/> component in the render tree.
		/// </summary>
		/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
		/// <param name="componentTypePredicate">The predicate which decides if a component should be replaced with a <see cref="Stub{TComponent}"/> component.</param>
		/// <param name="options">Optional options that configures how the <see cref="Stub{TComponent}"/> renders markup.</param>
		/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
		public static ComponentFactoryCollection UseStubFor(this ComponentFactoryCollection factories, Predicate<Type> componentTypePredicate, StubOptions? options = null)
		{
			if (factories is null)
				throw new ArgumentNullException(nameof(factories));
			if (componentTypePredicate is null)
				throw new ArgumentNullException(nameof(componentTypePredicate));

			factories.Add(new StubComponentFactory(componentTypePredicate, options ?? StubOptions.Default));
			return factories;
		}
	}
}
#endif
