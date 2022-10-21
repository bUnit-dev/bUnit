namespace Bunit.TestAssets.SampleComponents;

public class ThrowsOnParameterSet : ComponentBase
{
	private readonly string value = string.Empty;

	[Parameter]
	public string Value
	{
		get => value;
		set => throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} is invalid");
	}
}