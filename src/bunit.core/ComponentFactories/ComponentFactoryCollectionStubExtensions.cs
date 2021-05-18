#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Components;

namespace Bunit.ComponentFactories
{
	public static class ComponentFactoryCollectionStubExtensions
	{
		public static ComponentFactoryCollection UseStubFor<TComponent>(this ComponentFactoryCollection factories, bool renderParameters = true)
			where TComponent : IComponent
		{
			factories.Add(new StubComponentFactory<TComponent>(renderParameters));
			return factories;
		}
	}
}
#endif
