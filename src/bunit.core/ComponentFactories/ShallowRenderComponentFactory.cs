#if NET5_0_OR_GREATER
using System;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;

namespace Bunit.ComponentFactories
{
	/// <summary>
	/// Represents a component factory that will stub out
	/// all components using the <see cref="Stub{TComponent}"/>,
	/// except the first child component of the <see cref="ShallowRenderContainer"/>.
	/// </summary>
	internal class ShallowRenderComponentFactory : IComponentFactory
	{
		private static readonly Type ShallowRenderContainerType = typeof(ShallowRenderContainer);
		private bool shallowRenderContainerSeen;
		private bool hasCreatedComponentUnderTest;

		public bool HasShallowRendered => hasCreatedComponentUnderTest;

		public bool CanCreate(Type componentType)
		{
			if (shallowRenderContainerSeen)
				return true;

			shallowRenderContainerSeen = componentType == ShallowRenderContainerType;

			return false;
		}

		public IComponent Create(Type componentType)
		{
			if (hasCreatedComponentUnderTest)
			{
				var stubType = typeof(Stub<>).MakeGenericType(componentType);
				return (IComponent)Activator.CreateInstance(stubType)!;
			}

			hasCreatedComponentUnderTest = true;
			return (IComponent)Activator.CreateInstance(componentType)!;
		}
	}
}
#endif
