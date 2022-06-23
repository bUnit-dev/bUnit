namespace Bunit.TestAssets.SampleComponents;
public class SimpleBind : ComponentBase
{
	[Parameter] public string Value { get; set; } = string.Empty;
	[Parameter] public EventCallback<string> ValueChanged { get; set; }
}
