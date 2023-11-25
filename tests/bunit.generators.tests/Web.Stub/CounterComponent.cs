using Microsoft.AspNetCore.Components;

namespace Bunit.Web.Stub;

public class CounterComponent : ComponentBase
{
	[Parameter] public int Count { get; set; }
	[CascadingParameter] public int CascadingCount { get; set; }
	[Parameter] public EventCallback IncrementCount { get; set; }
}
