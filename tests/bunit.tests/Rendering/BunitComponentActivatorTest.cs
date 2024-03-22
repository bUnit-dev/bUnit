using Bunit.TestDoubles;

namespace Bunit.Rendering;

public class BunitComponentActivatorTest : BunitContext
{
	[Fact(DisplayName = "Default activator")]
	public void Test001()
	{
		var activator = new CustomComponentActivator();
		Services.AddSingleton<IComponentActivator>(activator);

		var cut = Render<Simple1>();

		cut.Instance.ShouldBeOfType<Simple1>();
	}

	[Fact(DisplayName = "Custom singleton IComponentActivator registered in Services")]
	public void Test002()
	{
		var activator = new CustomComponentActivator();
		Services.AddSingleton<IComponentActivator>(activator);

		Render<Simple1>();

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

		var cut = Render<Wrapper>(ps => ps.AddChildContent<ClickCounter>());

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
