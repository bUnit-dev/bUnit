#if NET5_0_OR_GREATER
using System;
using Microsoft.AspNetCore.Components;

namespace Bunit.ComponentFactories;

internal sealed class GenericComponentFactory<TComponent, TReplacementComponent> : IComponentFactory
	where TComponent : IComponent
	where TReplacementComponent : IComponent
{
	public bool CanCreate(Type componentType) => componentType == typeof(TComponent);
	public IComponent Create(Type componentType) => Activator.CreateInstance<TReplacementComponent>()!;
}
#endif
