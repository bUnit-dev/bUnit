using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Dom.Events;
using Bunit.TestAssets.BlazorE2E;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Bunit.RenderingV2;

/// <summary>
/// Test in this file is based on the end-2-end tests of the browser renderer in Blazor.
/// The original tests are located here:
/// https://github.dev/dotnet/aspnetcore/blob/e97323601c57c21c6a9c399c220d327e09271d85/src/Components/test/E2ETest/Tests/ComponentRenderingTestBase.cs
/// </summary>
public class RenderedComponentV2Test
{
	private ServiceProvider Services { get; }
	private ILoggerFactory LoggerFactory { get; }
	private TestRendererV2 Renderer { get; }

	public RenderedComponentV2Test(ITestOutputHelper outputHelper)
	{
		var services = new ServiceCollection();
		services.AddXunitLogger(outputHelper);

		Services = services.BuildServiceProvider();
		LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
		Renderer = new TestRendererV2(Services, LoggerFactory);
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
		cut.Find("button").Dispatch(new Event("onclick"));
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
		var be1 = new BunitEvent(new MouseEventArgs(), "onclick");
		cut.Find("#tick").Dispatch(be1);
		Assert.Equal("Started", stateElement.TextContent);

		// Clicking 'tock' completes the task, which updates the state
		cut.Find("#tock").Dispatch(new Event("onclick"));

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
		inputElement.Dispatch(new KeyboardEvent("onkeypress", key: "a"));
		Assert.Collection(cut.FindAll("li"),
			li => Assert.Equal("a", li.TextContent));

		// Typing again adds another element
		inputElement.Dispatch(new KeyboardEvent("onkeypress", key: "b"));
		Assert.Collection(cut.FindAll("li"),
			li => Assert.Equal("a", li.TextContent),
			li => Assert.Equal("b", li.TextContent));

		// Textbox contains typed text
		Assert.Equal("ab", inputElement.GetAttribute("value"));
		Assert.Equal("ab", inputElement.Value);
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
		incrementButton.Dispatch(new Event("onclick"));
		Assert.Equal("Current count: 1", countDisplayElement.TextContent);

		// We can remove an event handler
		toggleClickHandlerCheckbox.Dispatch(new BunitEvent(new ChangeEventArgs() { Value = false }, "onchange"));
		Assert.Empty(cut.FindAll("#listening-message"));
		incrementButton.Dispatch(new Event("onclick"));
		Assert.Equal("Current count: 1", countDisplayElement.TextContent);

		// We can add an event handler
		toggleClickHandlerCheckbox.Dispatch(new BunitEvent(new ChangeEventArgs() { Value = true }, "onchange"));
		cut.Find("#listening-message");
		incrementButton.Dispatch(new Event("onclick"));
		Assert.Equal("Current count: 2", countDisplayElement.TextContent);
	}

	[Fact]
	public void CanRenderChildComponents()
	{
		var cut = Renderer.Render<ParentChildComponent>();
		Assert.Equal("Parent component", cut.Find("fieldset > legend").TextContent);

		var styledElement = cut.Find("fieldset > h1");
		Assert.Equal("Hello, world!", styledElement.TextContent);
		Assert.Equal("color: red;", styledElement.GetAttribute("style"));
		Assert.Equal("somevalue", styledElement.GetAttribute("customattribute"));
	}

	// Verifies we can render HTML content as a single block
	[Fact]
	public void CanRenderChildContent_StaticHtmlBlock()
	{
		var cut = Renderer.Render<HtmlBlockChildContent>();
		Assert.Equal("<p>Some-Static-Text</p>", cut.Find("#foo").InnerHtml);

		// TODO: confirm whether or not this should pass:
		// Assert.Equal("<p>Some-Static-Text</p>", cut.Find("#foo").GetAttribute("InnerHtml"));
	}
}
