#if NET5_0_OR_GREATER

namespace Bunit.Rendering;

internal class BunitComponentActivator : IComponentActivator
{
	private readonly ComponentFactoryCollection factories;
	private readonly IComponentActivator componentActivator;

	public BunitComponentActivator(
		IServiceProvider serviceProvider,
		ComponentFactoryCollection factories,
		IComponentActivator? externalComponentActivator)
	{
		this.factories = factories ?? throw new ArgumentNullException(nameof(factories));
		this.componentActivator = externalComponentActivator ?? new DefaultComponentActivator(serviceProvider);
	}

	public IComponent CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type componentType)
	{
		if (!typeof(IComponent).IsAssignableFrom(componentType))
		{
			throw new ArgumentException($"The type {componentType.FullName} does not implement {nameof(IComponent)}.", nameof(componentType));
		}

		// The FragmentContainer is a bUnit component added to the
		// render tree to separate the components from the TestContextBase.RenderTree
		// and the components in the render fragment being rendered.
		// It should never be replaced by another component, as
		// this would break bUnits ability to detect the start
		// of the component under test.
		if (typeof(FragmentContainer) == componentType)
			return new FragmentContainer();

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
		private readonly IServiceProvider serviceProvider;

		public DefaultComponentActivator(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IComponent CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type componentType)
		{
			return (IComponent)ActivatorUtilities.CreateInstance(serviceProvider, componentType);
		}
	}
}
#endif
