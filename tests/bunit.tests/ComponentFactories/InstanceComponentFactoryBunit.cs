namespace Bunit.ComponentFactories;

public class InstanceComponentFactoryBunit : BunitContext
{
	[Fact(DisplayName = "Add throws when factories is null")]
	public void Test001()
		=> Should.Throw<ArgumentNullException>(() => ComponentFactoryCollectionExtensions.Add<Simple1>(default, default(Simple1)));

	[Fact(DisplayName = "Add throws when instance is null")]
	public void Test002()
		=> Should.Throw<ArgumentNullException>(() => ComponentFactories.Add<Simple1>(default(Simple1)));

	[Fact(DisplayName = "Factory replaces one TComponent with instance in the render tree")]
	public void Test010()
	{
		var simple1Mock = Substitute.For<Simple1>();
		ComponentFactories.Add(simple1Mock);

		var cut = Render<Wrapper>(ps => ps.AddChildContent<Simple1>());

		cut.FindComponent<Simple1>()
			.Instance.ShouldBeSameAs(simple1Mock);
	}

	[Fact(DisplayName = "Factory throws if component instance is requested twice for TComponent that inherits from ComponentBase")]
	public void Test020()
	{
		var simple1Mock = Substitute.For<Simple1>();
		ComponentFactories.Add(simple1Mock);

		Should.Throw<InvalidOperationException>(() => Render<TwoComponentWrapper>(ps => ps
			   .Add<Simple1>(p => p.First)
			   .Add<Simple1>(p => p.Second)));
	}

	[Fact(DisplayName = "Factory throws if component instance is requested twice for TComponent that implements from IComponent")]
	public void Test021()
	{
		var simple1Mock = Substitute.For<BasicComponent>();
		ComponentFactories.Add(simple1Mock);

		Should.Throw<InvalidOperationException>(() => Render<TwoComponentWrapper>(ps => ps
			.Add<BasicComponent>(p => p.First)
			.Add<BasicComponent>(p => p.Second)));
	}
}
