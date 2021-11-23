#if NET5_0_OR_GREATER
using Bunit.TestDoubles;

namespace Bunit.ComponentFactories;

internal sealed class StubComponentFactory : IComponentFactory
{
	private static readonly Type StubType = typeof(Stub<>);

	private readonly Predicate<Type> componentTypePredicate;
	private readonly RenderFragment<IReadOnlyDictionary<string, object>>? replacementTemplate;

	public StubComponentFactory(Predicate<Type> componentTypePredicate, RenderFragment<IReadOnlyDictionary<string, object>>? replacementTemplate = null)
	{
		this.componentTypePredicate = componentTypePredicate;
		this.replacementTemplate = replacementTemplate;
	}

	public bool CanCreate(Type componentType)
		=> componentTypePredicate.Invoke(componentType);

	public IComponent Create(Type componentType)
	{
		var typeToCreate = StubType.MakeGenericType(componentType);
		return (IComponent)Activator.CreateInstance(typeToCreate, new object?[] { replacementTemplate })!;
	}
}
#endif
