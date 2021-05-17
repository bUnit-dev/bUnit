using System;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Bunit.ComponentFactories
{
	internal class ShallowRenderComponentFactory : IComponentFactory
	{
		private static readonly Type FragmentContainerType = typeof(FragmentContainer);
		private bool fragmentContainerSeen;
		private bool hasCreatedComponentUnderTest;

		public bool CanCreate(Type componentType)
		{
			if (fragmentContainerSeen)
				return true;

			if (componentType == FragmentContainerType)
			{
				fragmentContainerSeen = true;
			}

			return false;
		}

		public IComponent Create(Type componentType)
		{
			if (hasCreatedComponentUnderTest)
			{
				// create the stub component
				var stubType = typeof(Stub<>).MakeGenericType(componentType);
				return (IComponent)Activator.CreateInstance(stubType)!;
			}

			hasCreatedComponentUnderTest = true;
			return (IComponent)Activator.CreateInstance(componentType)!;
		}
	}
}
