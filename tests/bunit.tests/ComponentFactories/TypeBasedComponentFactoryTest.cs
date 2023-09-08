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
	public void Test010()
	{
		var simple1Mock = Substitute.For<Simple1>();
		ComponentFactories.Add<Simple1>(() => simple1Mock);

		var cut = Render<Wrapper>(ps => ps.AddChildContent<Simple1>());

		cut.FindComponent<Simple1>()
			.Instance.ShouldBeSameAs(simple1Mock);
	}

	[Fact(DisplayName = "Multiple TComponent replaced in render tree with component from factory method")]
	public void Test011()
	{
		ComponentFactories.Add(() => Substitute.For<Simple1>());

		var cut = Render<TwoComponentWrapper>(ps => ps
			   .Add<Simple1>(p => p.First)
			   .Add<Simple1>(p => p.Second));

		foreach (var component in cut.FindComponents<Simple1>())
		{
			Action checkIfSubstitute = () => component.Instance.Received();
			checkIfSubstitute.ShouldNotThrow();
		}
	}
}
