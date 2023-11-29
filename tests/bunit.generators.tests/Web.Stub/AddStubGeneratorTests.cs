using Bunit.Web.Stub.Components;
using Microsoft.AspNetCore.Components;

namespace Bunit.Web.Stub;

public class AddStubGeneratorTests : TestContext
{
	[Fact]
	public void Stubbed_component_has_same_parameters()
	{
		ComponentFactories.AddStub<CounterComponent>();
		
		var cut = RenderComponent<ParentComponent>();

		var child = cut.FindComponent<CounterComponentStub>();
		Assert.Equal(2, child.Instance.Count);
	}

	[Fact]
	public void Generated_stub_can_handle_cascading_parameters()
	{
		ComponentFactories.AddStub<CounterComponent>();

		var cut = RenderComponent<ParentComponent>(p => p.AddCascadingValue("Cascading", 3));

		var stub = cut.FindComponent<CounterComponentStub>();
		Assert.Equal(3, stub.Instance.CascadingCount);
	}

	[Fact]
	public void Generated_stub_can_handle_unmatched_parameters()
	{
		ComponentFactories.AddStub<CounterComponent>();

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
	
	[Fact]
	public void Generated_stub_via_attribute_has_same_parameters()
	{
		ComponentFactories.Add<ButtonComponent, ButtonComponentStub>();

		var cut = Render(builder =>
		{
			builder.OpenComponent<ButtonComponent>(1);
			builder.AddAttribute(2, "Text", "test");
			builder.CloseComponent();
		});

		var stub = cut.FindComponent<ButtonComponentStub>();
		Assert.Equal("test", stub.Instance.Text);
	}
}

[Stub(typeof(ButtonComponent))]
public partial class ButtonComponentStub
{
}