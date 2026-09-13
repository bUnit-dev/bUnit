namespace Bunit.TestAssets.SampleComponents;

public class MultipleStateHasChangedInOnParametersSet : ComponentBase
{
	[Parameter]
	public int Value { get; set; }

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		StateHasChanged();
		StateHasChanged();
		StateHasChanged();
	}
}

