namespace Bunit.Web.Stub;

public class StubTests : TestContext
{
	[Fact]
	public void Stubbed_component_has_same_parameters()
	{
		ComponentFactories.AddGeneratedStub<CounterComponent>();
		
		var cut = RenderComponent<ParentComponent>();

		var child = cut.FindComponent<CounterComponentStub>();
		Assert.Equal(2, child.Instance.Count);
	}

	[Fact]
	public void Stubbed_component_can_be_used_in_multiple_tests()
	{
		ComponentFactories.AddGeneratedStub<CounterComponent>();

		var cut = RenderComponent<ParentComponent>();
		
		Assert.True(cut.HasComponent<CounterComponentStub>());
	}
}