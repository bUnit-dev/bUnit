using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Bunit.RenderingV2;

public class TestRendererV2Test
{
	public ServiceProvider Services { get; }
	public ILoggerFactory LoggerFactory { get; }

	public TestRendererV2Test(ITestOutputHelper outputHelper)
	{
		var services = new ServiceCollection();
		services.AddXunitLogger(outputHelper);

		Services = services.BuildServiceProvider();
		LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
	}

	[Fact]
	public void Renders_RenderFragment()
	{
		var sut = new TestRendererV2(Services, LoggerFactory);
		var wasCalled = false;

		sut.Render(b => { wasCalled = true; });

		wasCalled.ShouldBeTrue();
	}

	[Fact]
	public void Renders_completes_before_async_life_cycle_methods_finish()
	{
		var sut = new TestRendererV2(Services, LoggerFactory);
		var taskSource = new TaskCompletionSource();

		var renderedComponent = sut.Render<Wrapper>(ps => ps
			.AddChildContent<AsyncSetParametersComponent>(cp => cp.Add(p => p.SetParameterDelay, taskSource.Task))
			.AddChildContent<AsyncOnLifeCycleMethods>(cp => cp
				.Add(p => p.OnInitializedDelay, taskSource.Task)
				.AddChildContent<AsyncOnLifeCycleMethods>(ccp => ccp.Add(p => p.OnInitializedDelay, taskSource.Task)))
			.AddChildContent<AsyncOnLifeCycleMethods>(cp => cp
				.Add(p => p.OnParametersSetDelay, taskSource.Task)
			.AddChildContent<AsyncOnLifeCycleMethods>(cp => cp
				.Add(p => p.OnAfterRenderDelay, taskSource.Task))));

		renderedComponent.ShouldNotBeNull();
	}

	[Fact]
	public async Task RendersAsync_blocks_until_all_life_cycle_methods_are_completed()
	{
		var sut = new TestRendererV2(Services, LoggerFactory);
		var taskSource = new TaskCompletionSource();		
		var tcsOnAfterRender = new TaskCompletionSource();

		var renderTask = sut.RenderAsync<Wrapper>(ps => ps
			.AddChildContent<AsyncSetParametersComponent>(cp => cp.Add(p => p.SetParameterDelay, taskSource.Task))
			.AddChildContent<AsyncOnLifeCycleMethods>(cp => cp
				.Add(p => p.OnInitializedDelay, taskSource.Task)
				.AddChildContent<AsyncOnLifeCycleMethods>(ccp => ccp.Add(p => p.OnInitializedDelay, taskSource.Task)))
			.AddChildContent<AsyncOnLifeCycleMethods>(cp => cp
				.Add(p => p.OnParametersSetDelay, taskSource.Task)
			.AddChildContent<AsyncOnLifeCycleMethods>(cp => cp
				.Add(p => p.OnAfterRenderDelay, tcsOnAfterRender.Task))));

		renderTask.IsCompleted.ShouldBeFalse();
		taskSource.SetResult();
		await renderTask;
		renderTask.IsCompleted.ShouldBeTrue();
		tcsOnAfterRender.Task.IsCompleted.ShouldBeFalse();
	}

	class AsyncSetParametersComponent : ComponentBase
	{
		[Parameter] public Task? SetParameterDelay { get; set; }

		public override Task SetParametersAsync(ParameterView parameters)
			=> parameters.GetValueOrDefault<Task>(nameof(SetParameterDelay)) ?? Task.CompletedTask;
	}

	class AsyncOnLifeCycleMethods : ComponentBase
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
}
