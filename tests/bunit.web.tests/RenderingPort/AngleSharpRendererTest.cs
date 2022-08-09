// Tests in this file is ported from ComponentRenderingTestBase.cs in the ASP.NET core repo.
// Version ported: https://github.dev/dotnet/aspnetcore/blob/e97323601c57c21c6a9c399c220d327e09271d85/src/Components/test/E2ETest/Tests/ComponentRenderingTestBase.cs

using System.Numerics;
using AngleSharp.Html.Dom;
using Bunit.TestAssets.BlazorE2E;
using Bunit.TestAssets.BlazorE2E.HierarchicalImportsTest.Subdir;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Bunit.RenderingPort;

public class AngleSharpRendererTest
{
	private ServiceProvider Services { get; }
	private ILoggerFactory LoggerFactory { get; }
	private BunitRenderer Renderer { get; }
	private BunitJSInterop JSInterop { get; } = new() { Mode = JSRuntimeMode.Loose };

	public AngleSharpRendererTest(ITestOutputHelper outputHelper)
	{
		var services = new ServiceCollection();
		services.AddXunitLogger(outputHelper);
		services.AddSingleton<IJSRuntime>(JSInterop.JSRuntime);
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

	[Fact]
	public void CanAddAndRemoveChildComponentsDynamically()
	{
		// Initially there are zero child components
		var cut = Renderer.Render<AddRemoveChildComponents>();
		var addButton = cut.Find(".addChild");
		var removeButton = cut.Find(".removeChild");
		var childComponentWrappers = () => cut.FindAll("p");
		Assert.Empty(childComponentWrappers());

		// Click to add/remove some child components
		addButton.Click();
		Assert.Collection(childComponentWrappers(),
			elem => Assert.Equal("Child 1", elem.Find(".message").TextContent));

		addButton.Click();
		Assert.Collection(childComponentWrappers(),
			elem => Assert.Equal("Child 1", elem.Find(".message").TextContent),
			elem => Assert.Equal("Child 2", elem.Find(".message").TextContent));

		removeButton.Click();
		Assert.Collection(childComponentWrappers(),
			elem => Assert.Equal("Child 1", elem.Find(".message").TextContent));

		addButton.Click();
		Assert.Collection(childComponentWrappers(),
			elem => Assert.Equal("Child 1", elem.Find(".message").TextContent),
			elem => Assert.Equal("Child 3", elem.Find(".message").TextContent));
	}

	[Fact]
	public void ChildComponentsNotifiedWhenPropertiesChanged()
	{
		// Child component receives notification that lets it compute a property before first render
		var cut = Renderer.Render<PropertiesChangedHandlerParent>();
		var suppliedValueElement = cut.Find(".supplied");
		var computedValueElement = cut.Find(".computed");
		var incrementButton = cut.Find("button");
		Assert.Equal("You supplied: 100", suppliedValueElement.TextContent);
		Assert.Equal("I computed: 200", computedValueElement.TextContent);

		// When property changes, child is renotified before rerender
		incrementButton.Click();
		Assert.Equal("You supplied: 101", suppliedValueElement.TextContent);
		Assert.Equal("I computed: 202", computedValueElement.TextContent);
	}

	[Fact]
	public void CanRenderFragmentsWhilePreservingSurroundingElements()
	{
		// Initially, the region isn't shown
		var cut = Renderer.Render<RenderFragmentToggler>();
		var originalButton = cut.Find("button");
		var fragmentElements = () => cut.FindAll("p[name=fragment-element]");
		Assert.Empty(fragmentElements());

		// The JS-side DOM builder handles regions correctly, placing elements
		// after the region after the corresponding elements
		Assert.Equal("The end", cut.FindAll("div > *:last-child").Single().TextContent);

		// When we click the button, the region is shown
		originalButton.Click();
		Assert.Single(fragmentElements());

		// The button itself was preserved, so we can click it again and see the effect
		originalButton.Click();
		Assert.Empty(fragmentElements());
	}

	[Fact]
	public void CanUseViewImportsHierarchically()
	{
		// The component is able to compile and output these type names only because
		// of the _ViewImports.cshtml files at the same and ancestor levels
		var cut = Renderer.Render<ComponentUsingImports>();

		Assert.Collection(cut.FindAll("p"),
			elem => Assert.Equal(typeof(Complex).FullName, elem.TextContent),
			elem => Assert.Equal(typeof(System.Configuration.Assemblies.AssemblyHashAlgorithm).FullName, elem.TextContent));
	}

	// Not relevant in a bUnit context.
	//[Fact]
	//public void CanUseComponentAndStaticContentFromExternalNuGetPackage()
	//{
	//	var appElement = Browser.MountTestComponent<ExternalContentPackage>();

	//	// NuGet packages can use JS interop features to provide
	//	// .NET code access to browser APIs
	//	var showPromptButton = appElement.FindElements(By.TagName("button")).First();
	//	showPromptButton.Click();

	//	var modal = Browser.Exists(() => Browser.SwitchTo().Alert(), TimeSpan.FromSeconds(3));
	//	modal.SendKeys("Some value from test");
	//	modal.Accept();
	//	var promptResult = appElement.FindElement(By.TagName("strong"));
	//	Browser.Equal("Some value from test", () => promptResult.Text);

	//	// NuGet packages can also embed entire components (themselves
	//	// authored as Razor files), including static content. The CSS value
	//	// here is in a .css file, so if it's correct we know that static content
	//	// file was loaded.
	//	var specialStyleDiv = appElement.FindElement(By.ClassName("special-style"));
	//	Assert.Equal("50px", specialStyleDiv.GetCssValue("padding"));

	//	// This style is isolated to the component and comes from the bundle that gets generated for BasicTestApp
	//	// and that includes the @import for the TestContentPackage.bundle.scp.css file
	//	Assert.Equal("20px", specialStyleDiv.GetCssValue("font-size"));

	//	// The external components are fully functional, not just static HTML
	//	var externalComponentButton = specialStyleDiv.FindElement(By.TagName("button"));
	//	Assert.Equal("Click me", externalComponentButton.Text);
	//	externalComponentButton.Click();
	//	Browser.Equal("It works", () => externalComponentButton.Text);
	//}

	[Fact]
	public void LogicalElementInsertionWorksHierarchically()
	{
		var cut = Renderer.Render<LogicalElementInsertionCases>();
		Assert.Equal("First Second Third", cut.Markup);
	}

	[Fact]
	public void CanUseJsInteropToReferenceElements()
	{
		var cut = Renderer.Render<ElementRefComponent>();
		var inputElement = cut.Find<IHtmlInputElement>("#capturedElement");
		var buttonElement = cut.Find("button");

		Assert.Equal(string.Empty, inputElement.Value);

		buttonElement.Click();

		// Changed to verify correct element reference is passed to JSInterop
		//Assert.Equal("Clicks: 1", inputElement.Value);
		//buttonElement.Click();
		//Assert.Equal("Clicks: 2", inputElement.Value);
		JSInterop.VerifyInvoke("setElementValue")
			.Arguments[0]
			.ShouldBeElementReferenceTo(inputElement);
	}

	// Test not relevant in bUnit context.
	//[Fact]
	//public void CanUseFocusExtensionToFocusElement()
	//{
	//	Browser.Manage().Window.Size = new System.Drawing.Size(100, 300);
	//	var appElement = Browser.MountTestComponent<ElementFocusComponent>();

	//	// y scroll position before click
	//	var pageYOffsetBefore = getPageYOffset();

	//	var buttonElement = appElement.FindElement(By.Id("focus-button"));

	//	// Make sure the input element isn't focused when the test begins; we don't want
	//	// the test to pass just because the input started as the focused element
	//	Browser.NotEqual("focus-input", getFocusedElementId);

	//	// Click the button whose callback focuses the input element
	//	buttonElement.Click();

	//	// Verify that the input element is focused
	//	Browser.Equal("focus-input", getFocusedElementId);

	//	// y scroll position ater click
	//	var pageYOffsetAfter = getPageYOffset();

	//	//  Verify that scroll became
	//	Assert.True(pageYOffsetAfter > pageYOffsetBefore);

	//	// A local helper that gets the ID of the focused element.
	//	string getFocusedElementId() => Browser.SwitchTo().ActiveElement().GetAttribute("id");

	//	// A local helper that gets window.PageYOffset
	//	long getPageYOffset() => (long)((IJavaScriptExecutor)Browser).ExecuteScript("return window.pageYOffset");
	//}

	// Test not relevant in bUnit context.
	//[Fact]
	//public void CanUseFocusExtensionToFocusSvgElement()
	//{
	//	Browser.Manage().Window.Size = new System.Drawing.Size(100, 300);
	//	var appElement = Browser.MountTestComponent<SvgFocusComponent>();

	//	var buttonElement = appElement.FindElement(By.Id("focus-button"));

	//	// Make sure the circle isn't focused when the test begins; we don't want
	//	// the test to pass just because the circle started as the focused element
	//	Browser.NotEqual("focus-circle", getFocusedElementId);

	//	// Click the button whose callback focuses the SVG element
	//	buttonElement.Click();

	//	// Verify that the circle is focused
	//	Browser.Equal("focus-circle", getFocusedElementId);

	//	// A local helper that gets the ID of the focused element.
	//	string getFocusedElementId() => Browser.SwitchTo().ActiveElement().GetAttribute("id");
	//}

	// Test not relevant in bUnit context.
	//[Fact]
	//public void CanUseFocusExtensionToFocusElementPreventScroll()
	//{
	//	Browser.Manage().Window.Size = new System.Drawing.Size(100, 300);
	//	var appElement = Browser.MountTestComponent<ElementFocusComponent>();

	//	// y scroll position before click
	//	var pageYOffsetBefore = getPageYOffset();

	//	var buttonElement = appElement.FindElement(By.Id("focus-button-prevented"));

	//	// Make sure the input element isn't focused when the test begins; we don't want
	//	// the test to pass just because the input started as the focused element
	//	Browser.NotEqual("focus-input", getFocusedElementId);

	//	// Click the button whose callback focuses the input element
	//	buttonElement.Click();

	//	// Verify that the input element is focused
	//	Browser.Equal("focus-input", getFocusedElementId);

	//	// y scroll position ater click
	//	var pageYOffsetAfter = getPageYOffset();

	//	//  Verify that not scrolled
	//	Assert.Equal(pageYOffsetAfter, pageYOffsetBefore);

	//	// A local helper that gets the ID of the focused element.
	//	string getFocusedElementId() => Browser.SwitchTo().ActiveElement().GetAttribute("id");

	//	// A local helper that gets window.PageYOffset
	//	long getPageYOffset() => (long)((IJavaScriptExecutor)Browser).ExecuteScript("return Math.round(window.pageYOffset)");
	//}

	// Test not relevant in bUnit context.
	//[Theory]
	//[InlineData("focus-button-onafterrender-invoke")]
	//[InlineData("focus-button-onafterrender-await")]
	//public void CanFocusDuringOnAfterRenderAsyncWithFocusInEvent(string triggerButton)
	//{
	//	// Represents https://github.com/dotnet/aspnetcore/issues/30070, plus a more complicated
	//	// variant where the initial rendering doesn't start from a JS interop call and hence
	//	// isn't automatically part of the WebAssemblyCallQueue.

	//	var appElement = Renderer.Render<ElementFocusComponent>();
	//	var didReceiveFocusLabel = appElement.FindElement(By.Id("focus-event-received"));
	//	Browser.Equal("False", () => didReceiveFocusLabel.Text);

	//	appElement.FindElement(By.Id(triggerButton)).Click();
	//	Browser.Equal("True", () => didReceiveFocusLabel.Text);
	//	Browser.Equal("focus-input-onafterrender", () => Browser.SwitchTo().ActiveElement().GetAttribute("id"));

	//	// As well as actually focusing and triggering the onfocusin event, we should not be seeing any errors
	//	var log = Browser.Manage().Logs.GetLog(LogType.Browser).ToArray();
	//	Assert.DoesNotContain(log, entry => entry.Level == LogLevel.Severe);
	//}

	// Test depends on JavaScript changing the DOM.
	// Could be changed to verify that that correct node was
	// referenced via JSInterop call capture.
	//[Fact]
	//public void CanCaptureReferencesToDynamicallyAddedElements()
	//{
	//	var cut = Renderer.Render<ElementRefComponent>();
	//	var buttonElement = cut.Find("button");
	//	var checkbox = cut.Find("input[type=checkbox]");

	//	// We're going to remove the input. But first, put in some contents
	//	// so we can observe it's not the same instance later
	//	cut.Find<IHtmlInputElement>("#capturedElement").Value = "some text";

	//	// Remove the captured element
	//	checkbox.Change(false);
	//	Assert.Empty(cut.FindAll("capturedElement"));

	//	// Re-add it; observe it starts empty again
	//	checkbox.Change(true);
	//	var inputElement = cut.Find<IHtmlInputElement>("#capturedElement");
	//	Assert.Equal(string.Empty, inputElement.Value);

	//	// See that the capture variable was automatically updated to reference the new instance
	//	buttonElement.Click();
	//	Assert.Equal("Clicks: 1", inputElement.GetAttribute("value"));
	//}

	[Fact]
	public void CanCaptureReferencesToDynamicallyAddedComponents()
	{
		var cut = Renderer.Render<ComponentRefComponent>();
		var incrementButtonSelector = "#child-component button";
		var currentCountTextSelector = "#child-component p:first-of-type";
		var resetButton = cut.Find("#reset-child");
		var toggleChildCheckbox = cut.Find("#toggle-child");
		Func<string> currentCountText = () => cut.Find(currentCountTextSelector).TextContent;

		// Verify the reference was captured initially
		cut.Find(incrementButtonSelector).Click();
		Assert.Equal("Current count: 1", currentCountText());
		resetButton.Click();
		Assert.Equal("Current count: 0", currentCountText());
		cut.Find(incrementButtonSelector).Click();
		Assert.Equal("Current count: 1", currentCountText());

		// Remove and re-add a new instance of the child, checking the text was reset
		toggleChildCheckbox.Change(false);
		Assert.Empty(cut.FindAll(incrementButtonSelector));
		toggleChildCheckbox.Change(true);
		Assert.Equal("Current count: 0", currentCountText());

		// Verify we have a new working reference
		cut.Find(incrementButtonSelector).Click();
		Assert.Equal("Current count: 1", currentCountText());
		resetButton.Click();
		Assert.Equal("Current count: 0", currentCountText());
	}

	//// Test not relevant in a bUnit context
	//[Fact]
	//public void CanUseJsInteropForRefElementsDuringOnAfterRender()
	//{
	//	var appElement = Browser.MountTestComponent<AfterRenderInteropComponent>();
	//	Browser.Equal("Value set after render", () => Browser.Exists(By.TagName("input")).GetAttribute("value"));
	//}

	[Fact]
	public void CanRenderMarkupBlocks()
	{
		var cut = Renderer.Render<MarkupBlockComponent>();

		// Static markup
		Assert.Equal(
			"attributes",
			cut.Find("p span#attribute-example").TextContent);

		// Dynamic markup (from a custom RenderFragment)
		Assert.Equal(
			"[Here is an example. We support multiple-top-level nodes.]",
			cut.Find("#dynamic-markup-block").TextContent.Trim());
		Assert.Equal(
			"example",
			cut.Find("#dynamic-markup-block strong#dynamic-element em").TextContent);

		// Dynamic markup (from a MarkupString)
		Assert.Equal(
			"This is a markup string.",
			cut.Find(".markup-string-value").TextContent);
		Assert.Equal(
			"markup string",
			cut.Find(".markup-string-value em").TextContent);

		// Updating markup blocks
		cut.Find("button").Click();
		Assert.Equal(
			"[The output was changed completely.]",
			cut.Find("#dynamic-markup-block").TextContent.Trim());
		Assert.Equal(
			"changed",
			cut.Find("#dynamic-markup-block span em").TextContent);
	}

	[Fact]
	public void CanRenderRazorTemplates()
	{
		var cut = Renderer.Render<RazorTemplates>();

		// code block template (component parameter)
		var element = cut.Find("div#codeblocktemplate ol");
		Assert.Collection(
			element.FindAll("li"),
			e => Assert.Equal("#1 - a", e.TextContent),
			e => Assert.Equal("#2 - b", e.TextContent),
			e => Assert.Equal("#3 - c", e.TextContent));
	}

	[Fact]
	public void CanRenderMultipleChildContent()
	{
		var cut = Renderer.Render<MultipleChildContent>();

		var table = cut.Find("table");

		var thead = table.Find("thead");
		Assert.Collection(
			thead.FindAll("th"),
			e => Assert.Equal("Col1", e.TextContent),
			e => Assert.Equal("Col2", e.TextContent),
			e => Assert.Equal("Col3", e.TextContent));

		var tfoot = table.Find("tfoot");
		Assert.Empty(tfoot.FindAll("td"));

		var toggle = cut.Find("#toggle");
		toggle.Change(true);

		Assert.Collection(
			tfoot.FindAll("td"),
			e => Assert.Equal("The", e.TextContent),
			e => Assert.Equal("", e.TextContent),
			e => Assert.Equal("End", e.TextContent));
	}

	[Fact]
	public async Task CanAcceptSimultaneousRenderRequests()
	{
		var expectedOutput = string.Join(
			string.Empty,
			Enumerable.Range(0, 100).Select(_ => "😊"));

		// NOTE: Awaiting the render completion makes the Task.Delay
		//       below unnecessary since RenderAsync only completes
		//       when the life cycle methods of the CUT is done initially.
		var cut = await Renderer.RenderAsync<ConcurrentRenderParent>();

		// It's supposed to pause the rendering for this long. The WaitAssert below
		// allows it to take up extra time if needed.
		//await Task.Delay(1000);

		var outputElement = cut.Find("#concurrent-render-output");
		Assert.Equal(expectedOutput, outputElement.TextContent);
	}

	[Fact]
	public async Task CanDispatchRenderToSyncContext()
	{
		var cut = Renderer.Render<DispatchingComponent>();
		var result = cut.Find("#result");

		// NOTE: awaiting the click handler ensures the test does not
		//       continue before the click handler has completed.
		await cut.Find("#run-with-dispatch").ClickAsync();

		Assert.Equal("Success (completed synchronously)", result.TextContent);
	}

	[Fact]
	public async Task CanDoubleDispatchRenderToSyncContext()
	{
		var cut = Renderer.Render<DispatchingComponent>();
		var result = cut.Find("#result");

		// NOTE: awaiting the click handler ensures the test does not
		//       continue before the click handler has completed.
		await cut.Find("#run-with-double-dispatch").ClickAsync();

		Assert.Equal("Success (completed synchronously)", result.TextContent);
	}

	// This test does not make sense in a bUnit context
	//[Fact]
	//public void CanPerformInteropImmediatelyOnComponentInsertion()
	//{
	//	var cut = Renderer.Render<InteropOnInitializationComponent>();
	//	Assert.Equal("Hello from interop call", () => cut.FindElement(By.Id("val-get-by-interop")).Text);
	//	Assert.Equal("Hello from interop call", () => cut.FindElement(By.Id("val-set-by-interop")).GetAttribute("value"));
	//}

	[Fact]
	public void CanUseAddMultipleAttributes()
	{
		var cut = Renderer.Render<DuplicateAttributesComponent>();

		var selector = "#duplicate-on-element > div";

		var element = cut.Find(selector);
		Assert.Equal(string.Empty, element.GetAttribute("bool")); // attribute is present
		Assert.Equal("middle-value", element.GetAttribute("string"));
		Assert.Equal("unmatched-value", element.GetAttribute("unmatched"));

		selector = "#duplicate-on-element-override > div";
		element = cut.Find(selector);
		Assert.Null(element.GetAttribute("bool")); // attribute is not present
		Assert.Equal("other-text", element.GetAttribute("string"));
		Assert.Equal("unmatched-value", element.GetAttribute("unmatched"));
	}

	[Fact]
	public void CanPatchRenderTreeToMatchLatestDOMState()
	{
		var cut = Renderer.Render<MovingCheckboxesComponent>();
		var incompleteItemsSelector = ".incomplete-items li";
		var completeItemsSelector = ".complete-items li";
		cut.Find(incompleteItemsSelector); // throws if not exists

		// Mark first item as done; observe the remaining incomplete item appears unchecked
		// because the diff algorithm explicitly unchecks it
		cut.Find(".incomplete-items .item-isdone").Change(true);
		var incompleteLIs = cut.FindAll(incompleteItemsSelector);
		Assert.Equal(1, incompleteLIs.Length);
		Assert.False(incompleteLIs[0].Find<IHtmlInputElement>(".item-isdone").IsChecked);

		// Mark first done item as not done; observe the remaining complete item appears checked
		// because the diff algorithm explicitly re-checks it
		cut.Find(".complete-items .item-isdone").Change(false);
		var completeLIs = cut.FindAll(completeItemsSelector);
		Assert.Equal(2, completeLIs.Length);
		Assert.True(completeLIs[0].Find<IHtmlInputElement>(".item-isdone").IsChecked);
	}

	// The test does rely on a real browser to use the contenteditable attribute
	//[Fact]
	//public void CanHandleClearedChild()
	//{
	//	var cut = Renderer.Render<ContentEditable>();
	//	var input = cut.Find("#editable-div");
	//	var clickable = cut.Find("#clickable");

	//	input.TextContent = "";
	//	clickable.Click();

	//	// Original test checked browser logs
	//	// var log = Browser.Manage().Logs.GetLog(LogType.Browser);
	//	// Assert.DoesNotContain(log, entry => entry.Level == LogLevel.Severe);
	//	Assert.Equal("", input.TextContent);
	//}

	[Fact]
	public async Task CanDispatchAsyncWorkToSyncContext()
	{
		var cut = Renderer.Render<DispatchingComponent>();
		var result = cut.Find("#result");

		await cut.Find("#run-async-with-dispatch").ClickAsync();

		Assert.Equal("First Second Third Fourth Fifth", result.TextContent);
	}
}
