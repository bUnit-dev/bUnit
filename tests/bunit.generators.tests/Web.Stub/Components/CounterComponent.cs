using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Bunit.Web.Stub.Components;

public class CounterComponent : ComponentBase
{
	[Parameter] public int Count { get; set; }
	[CascadingParameter(Name = "Cascading")] public int CascadingCount { get; set; }

	[Parameter(CaptureUnmatchedValues = true)]
	private Dictionary<string, object>? UnmatchedValues { get; set; }

	[SuppressMessage("Design", "CS0246: The type or namespace name could not be found",
		Justification = "This is on purpose")]
	[Required]
	public string Unused { get; set; } = default!;
}
