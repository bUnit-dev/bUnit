#if NET5_0_OR_GREATER
using System;
using Bunit.TestDoubles;
using Bunit.TestDoubles.Components;
using Microsoft.AspNetCore.Components;

namespace Bunit.ComponentFactories
{
	internal sealed class StubComponentFactory : IComponentFactory
	{
		private readonly Predicate<Type> componentTypePredicate;
		private readonly StubOptions options;

		public StubComponentFactory(Predicate<Type> componentTypePredicate, StubOptions options)
		{
			this.componentTypePredicate = componentTypePredicate;
			this.options = options;
		}

		public bool CanCreate(Type componentType)
			=> componentTypePredicate.Invoke(componentType);

		public IComponent Create(Type componentType)
			=> ComponentDoubleFactory.CreateStub(componentType, options);
	}
}
#endif
