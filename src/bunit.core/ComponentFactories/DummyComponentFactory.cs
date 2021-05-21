#if NET5_0_OR_GREATER
using System;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;

namespace Bunit.ComponentFactories
{
	internal sealed class DummyComponentFactory : IComponentFactory
	{
		private readonly Predicate<Type> componentTypePredicate;

		public DummyComponentFactory(Predicate<Type> componentTypePredicate)
			=> this.componentTypePredicate = componentTypePredicate;

		public bool CanCreate(Type componentType)
			=> componentTypePredicate.Invoke(componentType);

		public IComponent Create(Type componentType)
			=> ComponentDoubleFactory.CreateDummy(componentType);
	}
}
#endif
