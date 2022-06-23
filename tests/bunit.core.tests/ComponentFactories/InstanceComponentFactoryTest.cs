#if NET5_0_OR_GREATER

namespace Bunit.ComponentFactories;

public class InstanceComponentFactoryTest : TestContext
{
	[Fact(DisplayName = "Add throws when factories is null")]
	public void Test001()
		=> Should.Throw<ArgumentNullException>(() => ComponentFactoryCollectionExtensions.Add<Simple1>(default, default(Simple1)));

	[Fact(DisplayName = "Add throws when instance is null")]
	public void Test002()
		=> Should.Throw<ArgumentNullException>(() => ComponentFactories.Add<Simple1>(default(Simple1)));

	[Fact(DisplayName = "Factory replaces one TComponent with instance in the render tree")]
	public async Task Test010()
	{
		var simple1Mock = new Mock<Simple1>();
		ComponentFactories.Add<Simple1>(simple1Mock.Object);

		var cut = await RenderComponent<Wrapper>(ps => ps.AddChildContent<Simple1>());

		cut.FindComponent<Simple1>()
			.Instance.ShouldBeSameAs(simple1Mock.Object);
	}

	[Fact(DisplayName = "Factory throws if component instance is requested twice for TComponent that inherits from ComponentBase")]
	public void Test020()
	{
		var simple1Mock = new Mock<Simple1>();
		ComponentFactories.Add<Simple1>(simple1Mock.Object);

		Should.Throw<InvalidOperationException>(() => RenderComponent<TwoComponentWrapper>(ps => ps
			.Add<Simple1>(p => p.First)
			.Add<Simple1>(p => p.Second)));
	}

	[Fact(DisplayName = "Factory throws if component instance is requested twice for TComponent that implements from IComponent")]
	public void Test021()
	{
		var simple1Mock = new Mock<BasicComponent>();
		ComponentFactories.Add<BasicComponent>(simple1Mock.Object);

		Should.Throw<InvalidOperationException>(() => RenderComponent<TwoComponentWrapper>(ps => ps
			.Add<BasicComponent>(p => p.First)
			.Add<BasicComponent>(p => p.Second)));
	}
}
#endif
