#if NET5_0_OR_GREATER
using System;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;

namespace Bunit.ComponentFactories
{
	internal class StubComponentFactory<TComponent> : IComponentFactory
		where TComponent : IComponent
	{
		private readonly Type componentTypeToStub = typeof(TComponent);
		private readonly StubOptions stubOptions;

		public StubComponentFactory(StubOptions stubOptions)
		{
			this.stubOptions = stubOptions;
		}

		public bool CanCreate(Type componentType)
			=> componentType == componentTypeToStub;

		public IComponent Create(Type componentType)
			=> new Stub<TComponent>(stubOptions);
	}
}
#endif
