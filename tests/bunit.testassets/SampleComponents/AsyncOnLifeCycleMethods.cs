namespace Bunit.TestAssets.SampleComponents;

public class AsyncLifeCycleMethods : ComponentBase
{
	[Parameter] public Task? OnInitializedDelay { get; set; }
	[Parameter] public Task? OnParametersSetDelay { get; set; }
	[Parameter] public Task? OnAfterRenderDelay { get; set; }
	[Parameter] public RenderFragment ChildContent { get; set; }

	protected override Task OnInitializedAsync() => OnInitializedDelay ?? Task.CompletedTask;
	protected override Task OnParametersSetAsync() => OnParametersSetDelay ?? Task.CompletedTask;
	protected override Task OnAfterRenderAsync(bool firstRender) => OnAfterRenderDelay ?? Task.CompletedTask;

	protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(0, ChildContent);
}
