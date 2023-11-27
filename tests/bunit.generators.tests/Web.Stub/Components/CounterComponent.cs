using Microsoft.AspNetCore.Components;

namespace Bunit.Web.Stub.Components;

public class CounterComponent : ComponentBase
{
	[Parameter] public int Count { get; set; }
	[CascadingParameter(Name = "Cascading")] public int CascadingCount { get; set; }
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> UnmatchedValues { get; set; }
}
