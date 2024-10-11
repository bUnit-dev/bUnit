using Bunit.Web.Stub.Components;

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
	
	[Fact]
	public void Generated_Stub_respects_base_class_parameters()
	{
		ComponentFactories.AddStub<DerivedComponent>();

		var cut = RenderComponent<ContainerComponent>(c => c.AddChildContent<DerivedComponent>());

		var stub = cut.FindComponent<DerivedComponentStub>();
		Assert.Equal(0, stub.Instance.BaseCount);
	}
	
	[Fact]
	public void Generated_stub_via_attribute_respects_base_class_parameters()
	{
		ComponentFactories.Add<DerivedComponent, DerivedComponentStubViaAttributeAnnotation>();

		var cut = RenderComponent<ContainerComponent>(c => c.AddChildContent<DerivedComponent>());

		var stub = cut.FindComponent<DerivedComponentStubViaAttributeAnnotation>();
		Assert.Equal(0, stub.Instance.BaseCount);
	}
}

[ComponentStub<ButtonComponent>]
public partial class ButtonComponentStub;

public abstract class BaseComponent : ComponentBase
{
	[Parameter] public int BaseCount { get; set; }
}

public class DerivedComponent : BaseComponent
{
	[Parameter] public int DerivedCount { get; set; }
}

[ComponentStub<DerivedComponent>]
public partial class DerivedComponentStubViaAttributeAnnotation;