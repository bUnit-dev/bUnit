using System.Reflection;
using Bunit.Web.Stub.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

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
	public void Stubbed_component_has_same_parameter_values()
	{
		var type = typeof(CounterComponentStub);
		var cascadingParameterValue = type.GetProperty(nameof(CounterComponentStub.CascadingCount))
			?.GetCustomAttribute<CascadingParameterAttribute>()
			?.Name;
		var captureUnmatchedValues = type.GetProperty(nameof(CounterComponentStub.UnmatchedValues))
			?.GetCustomAttribute<ParameterAttribute>()
			?.CaptureUnmatchedValues;
		
		Assert.Equal("Cascading", cascadingParameterValue);
		Assert.True(captureUnmatchedValues);
	}
}