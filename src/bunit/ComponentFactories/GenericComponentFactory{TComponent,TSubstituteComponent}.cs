namespace Bunit.ComponentFactories;

internal sealed class GenericComponentFactory<TComponent, TSubstituteComponent> : IComponentFactory
	where TComponent : IComponent
	where TSubstituteComponent : IComponent
{
	public bool CanCreate(Type componentType) => componentType == typeof(TComponent);

	public IComponent Create(Type componentType) => Activator.CreateInstance<TSubstituteComponent>()!;
}
