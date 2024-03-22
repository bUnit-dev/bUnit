namespace Bunit.Rendering;

internal class BunitComponentActivator : IComponentActivator
{
	private readonly ComponentFactoryCollection factories;
	private readonly IComponentActivator componentActivator;

	public BunitComponentActivator(ComponentFactoryCollection factories, IComponentActivator? externalComponentActivator)
	{
		this.factories = factories ?? throw new ArgumentNullException(nameof(factories));
		this.componentActivator = externalComponentActivator ?? DefaultComponentActivator.Instance;
	}

	public IComponent CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type componentType)
	{
		if (!typeof(IComponent).IsAssignableFrom(componentType))
		{
			throw new ArgumentException($"The type {componentType.FullName} does not implement {nameof(IComponent)}.", nameof(componentType));
		}

		// The FragmentContainer is a bUnit component added to the
		// render tree to separate the components from the BunitContext.RenderTree
		// and the components in the render fragment being rendered.
		// It should never be replaced by another component, as
		// this would break bUnits ability to detect the start
		// of the component under test.
		if (typeof(BunitRootComponent) == componentType)
			return new BunitRootComponent();

		for (int i = factories.Count - 1; i >= 0; i--)
		{
			var factory = factories[i];
			if (factory.CanCreate(componentType))
			{
				return factory.Create(componentType);
			}
		}

		return componentActivator.CreateInstance(componentType);
	}

	private sealed class DefaultComponentActivator : IComponentActivator
	{
		public static IComponentActivator Instance { get; } = new DefaultComponentActivator();

		/// <inheritdoc />
		public IComponent CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type componentType)
		{
			return (IComponent)Activator.CreateInstance(componentType)!;
		}
	}
}
