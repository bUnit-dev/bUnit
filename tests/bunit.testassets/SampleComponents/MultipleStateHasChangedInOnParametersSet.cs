namespace Bunit.TestAssets.SampleComponents;

public class MultipleStateHasChangedInOnParametersSet : ComponentBase
{
	[Parameter]
	public int Value { get; set; }

#pragma warning disable BL0012
	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		StateHasChanged();
		StateHasChanged();
		StateHasChanged();
	}
#pragma warning restore BL0012
}

