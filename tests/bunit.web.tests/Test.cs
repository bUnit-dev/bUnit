using System.Diagnostics;
using Bunit.Rendering;

namespace Bunit;

public class Test : TestContext
{
	[Fact]
	public void MyTestMethod_async()
	{
		var renderer = (TestRenderer)Renderer;
		var cut = RenderComponent<ConstantAsyncRerender>();
		cut.WaitForAssertion(() =>
		{
			Thread.Sleep(500);
			renderer.WaitingRender?.SetResult(null!);
			cut.Markup.ShouldBe("19");
		}, TimeSpan.FromSeconds(20));
	}

	[Fact]
	public void MyTestMethod_2_async()
	{
		var renderer = (TestRenderer)Renderer;
		var cut = RenderComponent<ConstantAsyncRerender2>();
		cut.WaitForAssertion(() =>
		{
			Thread.Sleep(500);
			renderer.WaitingRender?.SetResult(null!);
			cut.Markup.ShouldBe("3");
		}, TimeSpan.FromSeconds(10));
	}

	[Fact]
	public void MyTestMethod_2()
	{
		var renderer = (TestRenderer)Renderer;
		var cut = RenderComponent<ConstantRerenderInit>();
		cut.WaitForAssertion(() =>
		{
			Thread.Sleep(500);
			renderer.WaitingRender?.SetResult(null!);
			cut.Markup.ShouldBe("19");
		}, TimeSpan.FromSeconds(20));
	}
}

public class ConstantRerender : ComponentBase
{
	private readonly Stopwatch stopwatch = Stopwatch.StartNew();

	public int RenderCount { get; set; }

	public List<TimeSpan> RenderOffset { get; } = new();

	protected override void OnAfterRender(bool firstRender)
	{
		RenderCount++;
		RenderOffset.Add(stopwatch.Elapsed);

		if (RenderCount < 20)
		{
			StateHasChanged();
		}
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
		=> builder.AddMarkupContent(0, $"{RenderCount}");
}

public class ConstantAsyncRerender : ComponentBase
{
	private readonly Stopwatch stopwatch = Stopwatch.StartNew();

	public int RenderCount { get; set; }

	public List<TimeSpan> RenderOffset { get; } = new();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		RenderCount++;
		RenderOffset.Add(stopwatch.Elapsed);

		if (RenderCount < 20)
		{
			await Task.Delay(1);
			StateHasChanged();
		}
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
		=> builder.AddMarkupContent(0, $"{RenderCount}");
}

public class ConstantAsyncRerender2 : ComponentBase
{
	private readonly Stopwatch stopwatch = Stopwatch.StartNew();

	public int RenderCount { get; set; }

	public List<TimeSpan> RenderOffset { get; } = new();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		RenderCount++;
		RenderOffset.Add(stopwatch.Elapsed);

		if (firstRender)
		{
			await Task.Delay(1);
			StateHasChanged();
			await Task.Delay(1);
			StateHasChanged();
			await Task.Delay(1);
			StateHasChanged();
		}
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
		=> builder.AddMarkupContent(0, $"{RenderCount}");
}

public class ConstantRerenderInit : ComponentBase
{
	private readonly Stopwatch stopwatch = Stopwatch.StartNew();

	public int RenderCount { get; set; }

	public List<TimeSpan> RenderOffset { get; } = new();

	protected override async Task OnInitializedAsync()
	{
		while (RenderCount < 20)
		{
			RenderCount++;
			RenderOffset.Add(stopwatch.Elapsed);
			await Task.Delay(1);
			StateHasChanged();
		}
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
		=> builder.AddMarkupContent(0, $"{RenderCount}");
}
