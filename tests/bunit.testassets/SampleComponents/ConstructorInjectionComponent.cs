using Microsoft.JSInterop;

namespace Bunit.TestAssets.SampleComponents;

public class ConstructorInjectionComponent : ComponentBase
{
	public IJSRuntime JSRuntime { get; }

	public ConstructorInjectionComponent(IJSRuntime jsRuntime)
	{
		JSRuntime = jsRuntime;
	}
}
