#if NET5_0_OR_GREATER

namespace Bunit.ComponentFactories;

public class TypeBasedComponentFactoryTest : TestContext
{
	[Fact(DisplayName = "Add throws when factories is null")]
	public void Test001()
		=> Should.Throw<ArgumentNullException>(
			() => ComponentFactoryCollectionExtensions.Add<Simple1>(default, default(Func<Simple1>)));

	[Fact(DisplayName = "Add throws when instance is null")]
	public void Test002()
		=> Should.Throw<ArgumentNullException>(() => ComponentFactories.Add<Simple1>(default(Func<Simple1>)));

	[Fact(DisplayName = "TComponent replaced in render tree with component from factory method")]
	public async Task Test010()
	{
		var simple1Mock = Mock.Of<Simple1>();
		ComponentFactories.Add<Simple1>(() => simple1Mock);

		var cut = await RenderComponent<Wrapper>(ps => ps.AddChildContent<Simple1>());

		cut.FindComponent<Simple1>()
			.Instance.ShouldBeSameAs(simple1Mock);
	}

	[Fact(DisplayName = "Multiple TComponent replaced in render tree with component from factory method")]
	public async Task Test011()
	{
		var mockRepo = new MockRepository(MockBehavior.Loose);
		ComponentFactories.Add<Simple1>(() => mockRepo.Create<Simple1>().Object);

		var cut = await RenderComponent<TwoComponentWrapper>(ps => ps
			.Add<Simple1>(p => p.First)
			.Add<Simple1>(p => p.Second));

		cut.FindComponents<Simple1>()
			.ShouldAllBe(
				x => Mock.Get(x.Instance),
				x => Mock.Get(x.Instance));
	}
}

#endif
