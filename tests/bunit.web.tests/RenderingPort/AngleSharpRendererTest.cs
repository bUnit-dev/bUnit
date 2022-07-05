using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;
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

	[Fact]
	public async Task CanTriggerAsyncEventHandlers()
	{
		// Initial state is stopped
		var cut = Renderer.Render<AsyncEventHandlerComponent>();
		var stateElement = cut.Find("#state");
		Assert.Equal("Stopped", stateElement.TextContent);

		// Clicking 'tick' changes the state, and starts a task
		var be1 = new BunitEvent(new MouseEventArgs(), "click");
		cut.Find("#tick").Dispatch(be1);
		Assert.Equal("Started", stateElement.TextContent);

		// Clicking 'tock' completes the task, which updates the state
		cut.Find("#tock").Dispatch(new MouseEvent("click", bubbles: true));

		// Await the first 'tick' events completion here, since it is
		// setting state to "Stopped".
		await be1.EventDispatchResult;

		Assert.Equal("Stopped", stateElement.TextContent);
	}

	[Fact]
	public void CanTriggerKeyPressEvents()
	{
		// List is initially empty
		var cut = Renderer.Render<KeyPressEventComponent>();
		var inputElement = cut.Find<IHtmlInputElement>("input");
		Assert.Empty(cut.FindAll("li"));

		// Typing adds element
		inputElement.Dispatch(new KeyboardEvent("keypress", key: "a", bubbles: true));
		Assert.Collection(cut.FindAll("li"),
			li => Assert.Equal("a", li.TextContent));

		// Typing again adds another element
		inputElement.Dispatch(new KeyboardEvent("keypress", key: "b", bubbles: true));
		Assert.Collection(cut.FindAll("li"),
			li => Assert.Equal("a", li.TextContent),
			li => Assert.Equal("b", li.TextContent));

		// Textbox contains typed text
		// NOTE: these assertions differ from the original
		//       because we do not track text cursor position,
		//       thus each KeyboardEvent will override whatever
		//       value is currently in the input element.
		Assert.Equal("b", inputElement.GetAttribute("value"));
		Assert.Equal("b", inputElement.Value);
	}

	[Fact]
	public void CanAddAndRemoveEventHandlersDynamically()
	{
		var cut = Renderer.Render<CounterComponent>();
		var countDisplayElement = cut.Find("p");
		var incrementButton = cut.Find("button");
		var toggleClickHandlerCheckbox = cut.Find("[type=checkbox]");

		// Initial count is zero; clicking button increments count
		Assert.Equal("Current count: 0", countDisplayElement.TextContent);
		incrementButton.Dispatch(new MouseEvent("click", bubbles: true));
		Assert.Equal("Current count: 1", countDisplayElement.TextContent);

		// We can remove an event handler
		toggleClickHandlerCheckbox.Dispatch(new BunitEvent(new ChangeEventArgs() { Value = false }, "change"));
		Assert.Empty(cut.FindAll("#listening-message"));
		incrementButton.Dispatch(new MouseEvent("click", bubbles: true));
		Assert.Equal("Current count: 1", countDisplayElement.TextContent);

		// We can add an event handler
		toggleClickHandlerCheckbox.Dispatch(new BunitEvent(new ChangeEventArgs() { Value = true }, "change"));
		cut.Find("#listening-message");
		incrementButton.Dispatch(new MouseEvent("click", bubbles: true));
		Assert.Equal("Current count: 2", countDisplayElement.TextContent);
	}
}