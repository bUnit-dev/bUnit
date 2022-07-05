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
		cut.Find("button").Click();
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
		var tickClick = cut.Find("#tick").ClickAsync();
		Assert.Equal("Started", stateElement.TextContent);

		// Clicking 'tock' completes the task, which updates the state
		cut.Find("#tock").Click();

		// Await the first 'tick' events completion here, since it is
		// setting state to "Stopped".
		await tickClick;

		Assert.Equal("Stopped", stateElement.TextContent);
	}

	[Fact]
	public void CanTriggerKeyPressEvents()
	{
		// List is initially empty
		var cut = Renderer.Render<KeyPressEventComponent>();
		var inputElement = cut.Find("input");
		Assert.Empty(cut.FindAll("li"));

		// Typing adds element
		inputElement.KeyPress(new KeyboardEventArgs { Key = "a" });
		Assert.Collection(cut.FindAll("li"),
			li => Assert.Equal("a", li.TextContent));

		// Typing again adds another element
		inputElement.KeyPress(new KeyboardEventArgs { Key = "b" });
		Assert.Collection(cut.FindAll("li"),
			li => Assert.Equal("a", li.TextContent),
			li => Assert.Equal("b", li.TextContent));

		// Textbox contains typed text
		// NOTE: these assertions differ from the original
		//       because we do not track text cursor position,
		//       thus each KeyboardEvent will override whatever
		//       value is currently in the input element.
		Assert.Equal("b", inputElement.GetAttribute("value"));
		Assert.Equal("b", ((IHtmlInputElement)inputElement).Value);
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
		incrementButton.Click();
		Assert.Equal("Current count: 1", countDisplayElement.TextContent);

		// We can remove an event handler
		toggleClickHandlerCheckbox.Change(false);
		Assert.Empty(cut.FindAll("#listening-message"));
		incrementButton.Click();
		Assert.Equal("Current count: 1", countDisplayElement.TextContent);

		// We can add an event handler
		toggleClickHandlerCheckbox.Change(true);
		cut.Find("#listening-message");
		incrementButton.Click();
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

		// Original test included this assertion. Its does not make sense in AngleSharp,
		// where InnerHtml is a property, not an attribute.
		//Assert.Equal("<p>Some-Static-Text</p>", cut.Find("#foo").GetAttribute("InnerHtml"));
	}

	// Verifies we can rewite more complex HTML content into blocks
	[Fact]
	public void CanRenderChildContent_MixedHtmlBlock()
	{
		var cut = Renderer.Render<HtmlMixedChildContent>();

		var one = cut.Find("#one");
		Assert.Equal("<p>Some-Static-Text</p>", one.InnerHtml);

		var two = cut.Find("#two");
		Assert.Equal("<span>More-Static-Text</span>", two.InnerHtml);

		var three = cut.Find("#three");
		Assert.Equal("Some-Dynamic-Text", three.InnerHtml);

		var four = cut.Find("#four");
		Assert.Equal("But this is static", four.InnerHtml);
	}

	// Verifies we can rewrite HTML blocks with encoded HTML
	[Fact]
	public void CanRenderChildContent_EncodedHtmlInBlock()
	{
		var cut = Renderer.Render<HtmlEncodedChildContent>();

		var one = cut.Find("#one");
		Assert.Equal("<p>Some-Static-Text</p>", one.InnerHtml);

		var two = cut.Find("#two");
		Assert.Equal("&lt;span&gt;More-Static-Text&lt;/span&gt;", two.InnerHtml);

		var three = cut.Find("#three");
		Assert.Equal("Some-Dynamic-Text", three.InnerHtml);

		var four = cut.Find("#four");
		Assert.Equal("But this is static", four.InnerHtml);
	}

	[Fact]
	public void CanTriggerEventsOnChildComponents()
	{
		// Counter is displayed as child component. Initial count is zero.
		var cut = Renderer.Render<CounterComponentWrapper>();
		var counterDisplay = cut
			.FindAll("p")
			.Single(element => element.TextContent == "Current count: 0");

		// Clicking increments count in child component
		cut.Find("button").Click();
		Assert.Equal("Current count: 1", counterDisplay.TextContent);
	}

	[Fact]
	public void ChildComponentsRerenderWhenPropertiesChanged()
	{
		// Count value is displayed in child component with initial value zero
		var cut = Renderer.Render<CounterComponentUsingChild>();
		var wholeCounterElement = cut.Find("p");
		var messageElementInChild = wholeCounterElement.Find(".message");
		Assert.Equal("Current count: 0", wholeCounterElement.TextContent);
		Assert.Equal("0", messageElementInChild.TextContent);

		// Clicking increments count in child element
		cut.Find("button").Click();
		Assert.Equal("1", messageElementInChild.TextContent);
	}
}
