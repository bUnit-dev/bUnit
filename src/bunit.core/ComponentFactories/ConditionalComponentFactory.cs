#if NET5_0_OR_GREATER
namespace Bunit.ComponentFactories;

internal sealed class ConditionalComponentFactory : IComponentFactory
{
	private readonly Predicate<Type> condition;
	private readonly Func<Type, IComponent> factory;

	public ConditionalComponentFactory(Predicate<Type> condition, Func<Type, IComponent> factory)
	{
		this.condition = condition;
		this.factory = factory;
	}

	public bool CanCreate(Type componentType)
		=> condition(componentType);

	public IComponent Create(Type componentType)
		=> factory(componentType);
}
#endif
