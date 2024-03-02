using Bunit.TestDoubles;

namespace Bunit.ComponentFactories;

internal sealed class StubComponentFactory : IComponentFactory
{
	private static readonly Type StubType = typeof(Stub<>);

	private readonly Predicate<Type> componentTypePredicate;
	private readonly object? replacementContent;

	public StubComponentFactory(Predicate<Type> componentTypePredicate, object? replacementContent)
	{
		this.componentTypePredicate = componentTypePredicate;
		this.replacementContent = replacementContent;
	}

	public bool CanCreate(Type componentType)
		=> componentTypePredicate.Invoke(componentType);

	public IComponent Create(Type componentType)
	{
		var typeToCreate = StubType.MakeGenericType(componentType);
		return (IComponent)Activator.CreateInstance(typeToCreate, replacementContent)!;
	}
}
