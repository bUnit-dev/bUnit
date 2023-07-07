#if NET5_0_OR_GREATER

using Bunit.TestDoubles;

namespace Bunit.Rendering;

public class BunitComponentActivatorTest : TestContext
{
	public static TheoryData<Action<IServiceCollection, IComponentActivator>> CustomActivatorRegistrations { get; } = new()
	{
		static (services, activator) => services.AddSingleton(activator),
		static (services, activator) => services.AddScoped(_ => activator),
		static (services, activator) => services.AddTransient(_ => activator),
	};

	[Fact(DisplayName = "Default bUnit activator")]
	public void Test001()
	{
		var cut = RenderComponent<Simple1>();

		cut.Instance.ShouldBeOfType<Simple1>();
	}

	[Theory(DisplayName = "Custom IComponentActivator registered in Services")]
	[MemberData(nameof(CustomActivatorRegistrations))]
	public void Test002(Action<IServiceCollection, IComponentActivator> registerCustomActivator)
	{
		var activator = new CustomComponentActivator();
		registerCustomActivator(Services, activator);

		RenderComponent<Simple1>();

		activator
			.RequestedComponentTypes
			.ShouldHaveSingleItem()
			.ShouldBe(typeof(Simple1));
	}

	[Theory(DisplayName = "Custom singleton IComponentActivator registered in Services with ComponentFactories in use")]
	[MemberData(nameof(CustomActivatorRegistrations))]
	public void Test003(Action<IServiceCollection, IComponentActivator> registerCustomActivator)
	{
		var activator = new CustomComponentActivator();
		registerCustomActivator(Services, activator);
		
		ComponentFactories.AddStub<ClickCounter>();

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<ClickCounter>());

		activator
			.RequestedComponentTypes
			.ShouldHaveSingleItem()
			.ShouldBe(typeof(Wrapper));
		cut.HasComponent<ClickCounter>().ShouldBeFalse();
		cut.HasComponent<Stub<ClickCounter>>().ShouldBeTrue();
	}

	private sealed class CustomComponentActivator : IComponentActivator
	{
		public List<Type> RequestedComponentTypes { get; } = new();

		public IComponent CreateInstance(Type componentType)
		{
			RequestedComponentTypes.Add(componentType);
			return (IComponent)Activator.CreateInstance(componentType)!;
		}
	}
}

#endif
