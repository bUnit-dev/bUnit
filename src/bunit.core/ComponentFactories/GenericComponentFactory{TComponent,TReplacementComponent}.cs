using System;
using Microsoft.AspNetCore.Components;

namespace Bunit.ComponentFactories
{
	internal sealed class GenericComponentFactory<TComponent, TReplacementComponent> : IComponentFactory
		where TComponent : IComponent
		where TReplacementComponent : IComponent
	{
		public bool CanCreate(Type componentType) => componentType == typeof(TComponent);
		public IComponent Create(Type componentType) => (IComponent)Activator.CreateInstance(typeof(TReplacementComponent))!;
	}
}
