using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Bunit.Web.Stub;

public class StubTests
{
	[Fact]
	public void Stubbed_component_has_same_parameters()
	{
		var counterComponentStubProperties = typeof(CounterComponentStub).GetProperties();

		foreach (var prop in typeof(CounterComponent).GetProperties())
		{
			var matchingProp = counterComponentStubProperties.FirstOrDefault(p => p.Name == prop.Name);

			Assert.NotNull(matchingProp);

			var isParameter = prop.GetCustomAttribute(typeof(ParameterAttribute)) is not null;
			var stubIsParameter = matchingProp.GetCustomAttribute(typeof(ParameterAttribute)) is not  null;
			Assert.Equal(isParameter, stubIsParameter);

			var isCascadingParameter = prop.GetCustomAttribute(typeof(CascadingParameterAttribute)) is not  null;
			var stubIsCascadingParameter = matchingProp.GetCustomAttribute(typeof(CascadingParameterAttribute)) is not  null;
			Assert.Equal(isCascadingParameter, stubIsCascadingParameter);
		}
	}
}

[Stub(typeof(CounterComponent))]
public partial class CounterComponentStub : ComponentBase
{
}
