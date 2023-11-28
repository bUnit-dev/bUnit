using Bunit.Web.Stub.Components;

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
	public void Generated_stub_can_handle_cascading_parameters()
	{
		ComponentFactories.AddGeneratedStub<CounterComponent>();

		var cut = RenderComponent<ParentComponent>(p => p.AddCascadingValue("Cascading", 3));

		var stub = cut.FindComponent<CounterComponentStub>();
		Assert.Equal(3, stub.Instance.CascadingCount);
	}

	[Fact]
	public void Generated_stub_can_handle_unmatched_parameters()
	{
		ComponentFactories.AddGeneratedStub<CounterComponent>();

		var cut = Render(builder =>
		{
			builder.OpenComponent<CounterComponent>(1);
			builder.AddAttribute(2, "Class", "test");
			builder.AddAttribute(3, "Data", 3);
			builder.CloseComponent();
		});

		var stub = cut.FindComponent<CounterComponentStub>();
		var captured = stub.Instance.UnmatchedValues;
		Assert.Equal(2, captured.Count);
		Assert.Equal("test", captured.GetValueOrDefault("Class", null));
		Assert.Equal(3, captured.GetValueOrDefault("Data", 0));
	}
}