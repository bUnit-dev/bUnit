#if NET5_0_OR_GREATER
using System;
using Bunit.ComponentFactories;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// Extension methods for using component doubles.
	/// </summary>
	public static class ComponentFactoryCollectionCoreExtensions
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
	}
}
#endif
