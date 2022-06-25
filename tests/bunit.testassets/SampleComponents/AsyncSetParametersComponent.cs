namespace Bunit.TestAssets.SampleComponents;

public class AsyncSetParametersComponent : ComponentBase
{
	[Parameter] public Task? SetParameterDelay { get; set; }

	public override Task SetParametersAsync(ParameterView parameters)
		=> parameters.GetValueOrDefault<Task>(nameof(SetParameterDelay)) ?? Task.CompletedTask;
}
