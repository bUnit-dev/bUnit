namespace Bunit.ComponentFactories;

internal sealed class InstanceComponentFactory<TComponent> : IComponentFactory
	where TComponent : IComponent
{
	private readonly TComponent instance;
	private int createCount;

	public InstanceComponentFactory(TComponent instance)
		=> this.instance = instance;

	public bool CanCreate(Type componentType)
		=> componentType == typeof(TComponent);

	public IComponent Create(Type componentType)
	{
		if (createCount == 1)
		{
			throw new InvalidOperationException(
				$"The instance object passed to the" +
				$"{nameof(TestContext.ComponentFactories)}.{nameof(ComponentFactoryCollectionExtensions.Add)}<{typeof(TComponent).Name}>(instance) method can only be used to replace " +
				$"one {typeof(TComponent)} component in the render tree.");
		}

		createCount++;

		return instance;
	}
}
