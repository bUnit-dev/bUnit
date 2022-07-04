using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom.Events;
using Bunit.TestAssets.BlazorE2E;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Bunit.RenderingPort;

public class AngleSharpRendererTest
{
	private ServiceProvider Services { get; }
	private ILoggerFactory LoggerFactory { get; }
	private BunitRenderer Renderer { get; }

	public AngleSharpRendererTest(ITestOutputHelper outputHelper)
	{
		var services = new ServiceCollection();
		services.AddXunitLogger(outputHelper);

		Services = services.BuildServiceProvider();
		LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
		Renderer = new BunitRenderer(Services, LoggerFactory);
	}

	[Fact]
	public void CanRenderTextOnlyComponent()
	{
		var cut = Renderer.Render<TextOnlyComponent>();
		Assert.Equal("Hello from TextOnlyComponent", cut.Nodes.Single().TextContent);
	}

	[Fact]
	public void CanRenderComponentWithDataDash()
	{
		var cut = Renderer.Render<DataDashComponent>();
		var element = cut.Find("#cool_beans");
		Assert.Equal("17", element.GetAttribute("data-tab"));
		Assert.Equal("17", element.TextContent);
	}

	[Fact]
	public void CanRenderComponentWithAttributes()
	{
		var cut = Renderer.Render<RedTextComponent>();
		var styledElement = cut.Find("h1");
		Assert.Equal("Hello, world!", styledElement.TextContent);
		Assert.Equal("color: red;", styledElement.GetAttribute("style"));
		Assert.Equal("somevalue", styledElement.GetAttribute("customattribute"));
	}

	[Fact]
	public void CanTriggerEvents()
	{
		// Initial count is zero
		var cut = Renderer.Render<CounterComponent>();
		var countDisplayElement = cut.Find("p");
		Assert.Equal("Current count: 0", countDisplayElement.TextContent);

		// Clicking button increments count
		cut.Find("button").Dispatch(new MouseEvent("click", bubbles: true));
		Assert.Equal("Current count: 1", countDisplayElement.TextContent);
	}
}
