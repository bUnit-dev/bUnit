#if NET5_0_OR_GREATER

namespace Bunit.ComponentFactories;

public class ConditionalComponentFactoryTest : TestContext
{
	[Fact(DisplayName = "Add throws when factories is null")]
	public void Test001()
		=> Should.Throw<ArgumentNullException>(
			() => ComponentFactoryCollectionExtensions.Add(default, default(Predicate<Type>), default(Func<Type, IComponent>)));

	[Fact(DisplayName = "Add throws when condition is null")]
	public void Test002()
		=> Should.Throw<ArgumentNullException>(
			() => ComponentFactories.Add(default(Predicate<Type>), default(Func<Type, IComponent>)));

	[Fact(DisplayName = "Add throws when factory is null")]
	public void Test003()
	=> Should.Throw<ArgumentNullException>(
		() => ComponentFactories.Add(_ => true, default(Func<Type, IComponent>)));

	[Fact(DisplayName = "Component is replaced in render tree with component from factory when matches returns true")]
	public void Test010()
	{
		var mockComponent = Substitute.For<Simple1>();
		ComponentFactories.Add(type => type == typeof(Simple1), _ => mockComponent);

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<Simple1>());

		cut.FindComponent<Simple1>()
			.Instance.ShouldBeSameAs(mockComponent);
	}

	[Fact(DisplayName = "Component is replaced in render tree with component from factory when matches returns true")]
	public void Test011()
	{
		var mockSimple1 = Substitute.For<Simple1>();
		var mockNoArgs = Substitute.For<NoArgs>();
		ComponentFactories.Add(
			type => type != typeof(TwoComponentWrapper),
			type => CreateComponent(type, mockSimple1, mockNoArgs)
		);

		// Act
		var cut = RenderComponent<TwoComponentWrapper>(ps => ps
			.Add<Simple1>(p => p.First)
			.Add<NoArgs>(p => p.Second));

		// Assert
		cut.FindComponents<Simple1>().ShouldAllBe(x => x.Instance == mockSimple1);
		cut.FindComponents<NoArgs>().ShouldAllBe(x => x.Instance == mockNoArgs);
	}

	[Fact(DisplayName = "When matches returns false, factory is never called")]
	public void Test012()
	{
		ComponentFactories.Add(_ => false, _ => throw new NotImplementedException("DONT CALL FACTORY"));

		Should.NotThrow(() => RenderComponent<Wrapper>(ps => ps.AddChildContent<Simple1>()));
	}
	
	private static IComponent CreateComponent(Type type, Simple1 simple1, NoArgs noArgs)
	{
		if (type == typeof(Simple1)) return simple1;
		if (type == typeof(NoArgs)) return noArgs;
		throw new NotImplementedException($"No mock implementation provided for type {type.FullName}");
	}
}
#endif
