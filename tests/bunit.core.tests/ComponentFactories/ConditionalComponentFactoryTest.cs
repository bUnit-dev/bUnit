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
		var mockComponent = Mock.Of<Simple1>();
		ComponentFactories.Add(type => type == typeof(Simple1), _ => mockComponent);

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<Simple1>());

		cut.FindComponent<Simple1>()
			.Instance.ShouldBeSameAs(mockComponent);
	}

	[Fact(DisplayName = "Component is replaced in render tree with component from factory when matches returns true")]
	public void Test011()
	{
		var mockRepo = new MockRepository(MockBehavior.Loose);
		ComponentFactories.Add(
			type => type != typeof(TwoComponentWrapper),
			mockRepo.CreateComponent);

		var cut = RenderComponent<TwoComponentWrapper>(ps => ps
		   .Add<Simple1>(p => p.First)
		   .Add<NoArgs>(p => p.Second));

		cut.FindComponents<Simple1>().ShouldAllBe(x => Mock.Get(x.Instance));
		cut.FindComponents<NoArgs>().ShouldAllBe(x => Mock.Get(x.Instance));
	}

	[Fact(DisplayName = "When matches returns false, factory is never called")]
	public void Test012()
	{
		ComponentFactories.Add(_ => false, _ => throw new NotImplementedException("DONT CALL FACTORY"));

		Should.NotThrow(() => RenderComponent<Wrapper>(ps => ps.AddChildContent<Simple1>()));
	}
}

internal static class MockRepositoryExtensions
{
	private static readonly MethodInfo CreateMethodInfo = typeof(MockRepository)
		.GetMethod(nameof(MockRepository.Create), Array.Empty<Type>());

	public static IComponent CreateComponent(this MockRepository repository, Type type)
	{
		var genericCreateMethod = CreateMethodInfo.MakeGenericMethod(type);
		var mock = (Mock)genericCreateMethod.Invoke(repository, null);
		return (IComponent)mock.Object;
	}
}
