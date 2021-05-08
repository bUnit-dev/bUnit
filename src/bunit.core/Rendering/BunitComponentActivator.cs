#if NET5_0_OR_GREATER
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Bunit.Rendering
{
	internal class BunitComponentActivator : IComponentActivator
	{
		private readonly ComponentFactoryCollection factories;

		public BunitComponentActivator(ComponentFactoryCollection factories)
		{
			this.factories = factories ?? throw new ArgumentNullException(nameof(factories));
		}

		public IComponent CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type componentType)
		{
			if (!typeof(IComponent).IsAssignableFrom(componentType))
			{
				throw new ArgumentException($"The type {componentType.FullName} does not implement {nameof(IComponent)}.", nameof(componentType));
			}

			for (int i = factories.Count - 1; i >= 0; i--)
			{
				var factory = factories[i];
				if (factory.CanCreate(componentType))
				{
					return factory.Create(componentType);
				}
			}

			return (IComponent)Activator.CreateInstance(componentType)!;
		}
	}
}
#endif
