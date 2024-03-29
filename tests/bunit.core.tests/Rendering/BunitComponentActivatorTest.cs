#if NET5_0_OR_GREATER

using Bunit.TestDoubles;

namespace Bunit.Rendering;

public class BunitComponentActivatorTest : TestContext
{
	[Fact(DisplayName = "Default activator")]
	public void Test001()
	{
		var activator = new CustomComponentActivator();
		Services.AddSingleton<IComponentActivator>(activator);

		var cut = RenderComponent<Simple1>();

		cut.Instance.ShouldBeOfType<Simple1>();
	}

	[Fact(DisplayName = "Custom singleton IComponentActivator registered in Services")]
	public void Test002()
	{
		var activator = new CustomComponentActivator();
		Services.AddSingleton<IComponentActivator>(activator);

		RenderComponent<Simple1>();

		activator.RequestedComponentTypes
			.ShouldHaveSingleItem()
			.ShouldBe(typeof(Simple1));
	}

	[Fact(DisplayName = "Custom singleton IComponentActivator registered in Services with ComponentFactories in use")]
	public void Test003()
	{
		var activator = new CustomComponentActivator();
		Services.AddSingleton<IComponentActivator>(activator);
		ComponentFactories.AddStub<ClickCounter>();

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<ClickCounter>());

		activator.RequestedComponentTypes
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
