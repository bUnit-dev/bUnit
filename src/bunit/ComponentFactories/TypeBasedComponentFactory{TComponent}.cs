namespace Bunit.ComponentFactories;

internal sealed class TypeBasedComponentFactory<TComponent> : IComponentFactory
	where TComponent : IComponent
{
	private readonly Func<TComponent> componentFactory;

	public TypeBasedComponentFactory(Func<TComponent> componentFactory)
		=> this.componentFactory = componentFactory;

	public bool CanCreate(Type componentType)
		=> componentType == typeof(TComponent);

	public IComponent Create(Type componentType)
		=> componentFactory.Invoke();
}
