using System;
using System.Configuration.Assemblies;
using System.Linq;
using System.Numerics;
using Bunit.TestAssets.BlazorE2E;
using Bunit.TestAssets.BlazorE2E.HierarchicalImportsTest.Subdir;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit.BlazorE2E
{
	/// <summary>
	/// This tests are based on the tests from the following AspNetCore tests class.
	/// The aim is to only modify the original tests to not use Selenium, and instead use the
	/// <see cref="TextContext" />.
	/// https://github.com/dotnet/aspnetcore/blob/master/src/Components/test/E2ETest/Tests/ComponentRenderingTest.cs.
	/// </summary>
	public class ComponentRenderingTest : TestContext
	{
		public ComponentRenderingTest()
		{
			JSInterop.Mode = JSRuntimeMode.Loose;
		}

		[Fact]
		public void CanRenderTextOnlyComponent()
		{
			var cut = RenderComponent<TextOnlyComponent>();
			Assert.Equal("Hello from TextOnlyComponent", cut.Markup);
		}

		// This verifies that we've correctly configured the Razor language version via MSBuild.
		// See #974
		[Fact]
		public void CanRenderComponentWithDataDash()
		{
			var cut = RenderComponent<DataDashComponent>();
			var element = cut.Find("#cool_beans");
			Assert.Equal("17", element.GetAttribute("data-tab"));
			Assert.Equal("17", element.TextContent);
		}

		[Fact]
		public void CanRenderComponentWithAttributes()
		{
			var cut = RenderComponent<RedTextComponent>();
			var styledElement = cut.Find("h1");
			Assert.Equal("Hello, world!", styledElement.TextContent);
			Assert.Equal("color: red;", styledElement.GetAttribute("style"));
			Assert.Equal("somevalue", styledElement.GetAttribute("customattribute"));
		}

		[Fact]
		public void CanTriggerEvents()
		{
			// Initial count is zero
			var cut = RenderComponent<CounterComponent>();
			var countDisplayElement = cut.Find("p");
			Assert.Equal("Current count: 0", countDisplayElement.TextContent);

			// Clicking button increments count
			cut.Find("button").Click();
			Assert.Equal("Current count: 1", countDisplayElement.TextContent);
		}

		[Fact]
		public void CanTriggerAsyncEventHandlers()
		{
			// Initial state is stopped
			var cut = RenderComponent<AsyncEventHandlerComponent>();
			var stateElement = cut.Find("#state");
			Assert.Equal("Stopped", stateElement.TextContent);

			// Clicking 'tick' changes the state, and starts a task
			cut.Find("#tick").Click();
			Assert.Equal("Started", stateElement.TextContent);

			cut.Find("#tock").Click();
			cut.WaitForAssertion(() => Assert.Equal("Stopped", stateElement.TextContent));
		}

		[Fact]
		public void CanTriggerKeyPressEvents()
		{
			// List is initially empty
			var cut = RenderComponent<KeyPressEventComponent>();
			var inputElement = cut.Find("input");
			var liElements = cut.FindAll("li", enableAutoRefresh: true);
			liElements.ShouldBeEmpty();

			// Typing adds element
			inputElement.KeyPress("a");
			liElements.ShouldAllBe(li => Assert.Equal("a", li.TextContent));

			// Typing again adds another element
			inputElement.KeyPress("b");
			liElements.ShouldAllBe(
				li => Assert.Equal("a", li.TextContent),
				li => Assert.Equal("b", li.TextContent));
		}

		[Fact]
		public void CanAddAndRemoveEventHandlersDynamically()
		{
			var cut = RenderComponent<CounterComponent>();
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
			cut.Find("#listening-message"); // throws if non is found.
			incrementButton.Click();
			Assert.Equal("Current count: 2", countDisplayElement.TextContent);
		}

		[Fact]
		public void CanRenderChildComponents()
		{
			var cut = RenderComponent<ParentChildComponent>();
			Assert.Equal("Parent component", cut.Find("fieldset > legend").TextContent);

			var styledElement = cut.Find("fieldset > h1");
			Assert.Equal("Hello, world!", styledElement.TextContent);
			Assert.Equal("color: red;", styledElement.GetAttribute("style"));
			Assert.Equal("somevalue", styledElement.GetAttribute("customattribute"));
		}

		[Fact(DisplayName = "Verifies we can render HTML content as a single block")]
		public void CanRenderChildContent_StaticHtmlBlock()
		{
			var cut = RenderComponent<HtmlBlockChildContent>();
			Assert.Equal("<p>Some-Static-Text</p>", cut.Find("#foo").InnerHtml);
		}

		[Fact(DisplayName = "Verifies we can rewite more complex HTML content into blocks")]
		public void CanRenderChildContent_MixedHtmlBlock()
		{
			var cut = RenderComponent<HtmlMixedChildContent>();

			var one = cut.Find("#one");
			Assert.Equal("<p>Some-Static-Text</p>", one.InnerHtml);

			var two = cut.Find("#two");
			Assert.Equal("<span>More-Static-Text</span>", two.InnerHtml);

			var three = cut.Find("#three");
			Assert.Equal("Some-Dynamic-Text", three.InnerHtml);

			var four = cut.Find("#four");
			Assert.Equal("But this is static", four.InnerHtml);
		}

		[Fact(DisplayName = "Verifies we can rewrite HTML blocks with encoded HTML")]
		public void CanRenderChildContent_EncodedHtmlInBlock()
		{
			var cut = RenderComponent<HtmlEncodedChildContent>();

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
			var cut = RenderComponent<CounterComponentWrapper>();

			// Clicking increments count in child component
			cut.Find("button").Click();

			Assert.Equal("Current count: 1", cut.Find("h1+p").TextContent);
		}

		[Fact]
		public void ChildComponentsRerenderWhenPropertiesChanged()
		{
			// Count value is displayed in child component with initial value zero
			var cut = RenderComponent<CounterComponentUsingChild>();
			var wholeCounterElement = cut.Find("p");
			var messageElementInChild = cut.Find("p .message");
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
			var cut = RenderComponent<AddRemoveChildComponents>();
			var addButton = cut.Find(".addChild");
			var removeButton = cut.Find(".removeChild");
			Assert.Empty(cut.FindAll("p"));

			// Click to add/remove some child components
			addButton.Click();
			Assert.Collection(
				cut.FindAll("p .message"),
				msg => Assert.Equal("Child 1", msg.TextContent));

			addButton.Click();
			Assert.Collection(
				cut.FindAll("p .message"),
				msg => Assert.Equal("Child 1", msg.TextContent),
				msg => Assert.Equal("Child 2", msg.TextContent));

			removeButton.Click();
			Assert.Collection(
				cut.FindAll("p .message"),
				msg => Assert.Equal("Child 1", msg.TextContent));

			addButton.Click();
			Assert.Collection(
				cut.FindAll("p .message"),
				msg => Assert.Equal("Child 1", msg.TextContent),
				msg => Assert.Equal("Child 3", msg.TextContent));
		}

		[Fact]
		public void ChildComponentsNotifiedWhenPropertiesChanged()
		{
			// Child component receives notification that lets it compute a property before first render
			var cut = RenderComponent<PropertiesChangedHandlerParent>();
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
			var cut = RenderComponent<RenderFragmentToggler>();
			var originalButton = cut.Find("button");

			var fragmentElements = cut.FindAll("p[name=fragment-element]", enableAutoRefresh: true);
			Assert.Empty(fragmentElements);

			// The JS-side DOM builder handles regions correctly, placing elements
			// after the region after the corresponding elements
			Assert.Equal("The end", cut.Find("div > *:last-child").TextContent);

			// When we click the button, the region is shown
			originalButton.Click();
			fragmentElements.Single().ShouldNotBeNull();

			// The button itself was preserved, so we can click it again and see the effect
			originalButton.Click();
			Assert.Empty(fragmentElements);
		}

		[Fact]
		public void CanUseViewImportsHierarchically()
		{
			// The component is able to compile and output these type names only because
			// of the _ViewImports.cshtml files at the same and ancestor levels
			var cut = RenderComponent<ComponentUsingImports>();
			Assert.Collection(
				cut.FindAll("p"),
				elem => Assert.Equal(typeof(Complex).FullName, elem.TextContent),
				elem => Assert.Equal(typeof(AssemblyHashAlgorithm).FullName, elem.TextContent));
		}

		[Fact]
		public void CanRenderSvgWithCorrectNamespace()
		{
			var cut = RenderComponent<SvgComponent>();

			var svgElement = cut.Find("svg");
			Assert.NotNull(svgElement);

			var svgCircleElement = cut.Find("svg circle");
			Assert.NotNull(svgCircleElement);
			Assert.Equal("10", svgCircleElement.GetAttribute("r"));

			cut.Find("button").Click();
			Assert.Equal("20", svgCircleElement.GetAttribute("r"));
		}

		[Fact]
		public void CanRenderSvgChildComponentWithCorrectNamespace()
		{
			var cut = RenderComponent<SvgWithChildComponent>();

			var svgElement = cut.Find("svg");
			Assert.NotNull(svgElement);

			var svgCircleElement = cut.Find("svg circle");
			Assert.NotNull(svgCircleElement);
		}

		[Fact]
		public void LogicalElementInsertionWorksHierarchically()
		{
			var cut = RenderComponent<LogicalElementInsertionCases>();
			cut.MarkupMatches("First Second Third");
		}

		[Fact]
		public void CanUseJSInteropToReferenceElements()
		{
			// NOTE: This test required JS to modify the DOM. Test rewritten to use MockJSRuntime
			//       The original test code is here:
			// var cut = RenderComponent<ElementRefComponent>();
			// var inputElement = cut.Find("#capturedElement");
			// var buttonElement = cut.Find("button");

			// Assert.Equal(string.Empty, inputElement.GetAttribute("value"));

			// buttonElement.Click();
			// Assert.Equal("Clicks: 1", inputElement.GetAttribute("value"));
			// buttonElement.Click();
			// Assert.Equal("Clicks: 2", inputElement.GetAttribute("value"));

			var cut = RenderComponent<ElementRefComponent>();
			var inputElement = cut.Find("#capturedElement");
			var refId = inputElement.GetAttribute(Htmlizer.ElementReferenceAttrName);
			var buttonElement = cut.Find("button");

			buttonElement.Click();
			JSInterop.VerifyInvoke("setElementValue")
				.Arguments[0]
				.ShouldBeOfType<ElementReference>()
				.Id.ShouldBe(refId);
		}

		[Fact]
		public void CanCaptureReferencesToDynamicallyAddedElements()
		{
			// NOTE: This test required JS to modify the DOM. Test rewritten to use MockJSRuntime
			//       The original test code is here:
			// var cut = RenderComponent<ElementRefComponent>();
			// var buttonElement = cut.Find("button");
			// var checkbox = cut.Find("input[type=checkbox]");
			//
			// // We're going to remove the input. But first, put in some contents
			// // so we can observe it's not the same instance later
			// cut.Find("#capturedElement").SendKeys("some text");
			//
			// // Remove the captured element
			// checkbox.Click();
			// Browser.Empty(() => cut.FindAll("#capturedElement"));
			//
			// // Re-add it; observe it starts empty again
			// checkbox.Click();
			// var inputElement = cut.Find("#capturedElement");
			// Assert.Equal(string.Empty, inputElement.GetAttribute("value"));
			//
			// // See that the capture variable was automatically updated to reference the new instance
			// buttonElement.Click();
			// Assert.Equal("Clicks: 1", () => inputElement.GetAttribute("value"));

			var cut = RenderComponent<ElementRefComponent>();
			var buttonElement = cut.Find("button");
			var checkbox = cut.Find("input[type=checkbox]");

			// We're going to remove the input. But first, put in some contents
			// so we can observe it's not the same instance later
			cut.Find("#capturedElement");

			// Remove the captured element
			checkbox.Change(false);
			Should.Throw<ElementNotFoundException>(() => cut.Find("#capturedElement"));

			// Re-add it; observe it starts empty again
			checkbox.Change(true);
			var inputElement = cut.Find("#capturedElement");
			var refId = inputElement.GetAttribute(Htmlizer.ElementReferenceAttrName);

			// See that the capture variable was automatically updated to reference the new instance
			buttonElement.Click();

			JSInterop.VerifyInvoke("setElementValue")
				.Arguments[0]
				.ShouldBeOfType<ElementReference>()
				.Id.ShouldBe(refId);
		}

		[Fact]
		public void CanCaptureReferencesToDynamicallyAddedComponents()
		{
			var cut = RenderComponent<ComponentRefComponent>();
			var incrementButton = cut.Find("#child-component button");
			var resetButton = cut.Find("#reset-child");
			var toggleChildCheckbox = cut.Find("#toggle-child");
			var currentCountText = cut.Find("#child-component p:first-of-type");

			// Verify the reference was captured initially
			incrementButton.Click();
			Assert.Equal("Current count: 1", currentCountText.TextContent);
			resetButton.Click();
			Assert.Equal("Current count: 0", currentCountText.TextContent);
			incrementButton.Click();
			Assert.Equal("Current count: 1", currentCountText.TextContent);

			// Remove and re-add a new instance of the child, checking the text was reset
			toggleChildCheckbox.Change(false);
			Assert.Empty(cut.FindAll("#child-component button"));
			toggleChildCheckbox.Change(true);
			Assert.Equal("Current count: 0", currentCountText.TextContent);

			// Verify we have a new working reference
			incrementButton.Click();
			Assert.Equal("Current count: 1", currentCountText.TextContent);
			resetButton.Click();
			Assert.Equal("Current count: 0", currentCountText.TextContent);
		}

		// Test depends on javascript changing the DOM, thus doesnt make sense in this context.
		// [Fact]
		// public void CanUseJSInteropForRefElementsDuringOnAfterRender()
		// {
		//     var cut = RenderComponent<AfterRenderInteropComponent>();
		//     Assert.Equal("Value set after render", () => Browser.Find("input").GetAttribute("value"));
		// }

		[Fact]
		public void CanRenderMarkupBlocks()
		{
			var cut = RenderComponent<MarkupBlockComponent>();

			// Static markup
			Assert.Equal("attributes", cut.Find("p span#attribute-example").TextContent);

			// Dynamic markup (from a custom RenderFragment)
			Assert.Equal("[Here is an example. We support multiple-top-level nodes.]", cut.Find("#dynamic-markup-block").TextContent.Trim());
			Assert.Equal("example", cut.Find("#dynamic-markup-block strong#dynamic-element em").TextContent);

			// Dynamic markup (from a MarkupString)
			Assert.Equal("This is a markup string.", cut.Find(".markup-string-value").TextContent);
			Assert.Equal("markup string", cut.Find(".markup-string-value em").TextContent);

			// Updating markup blocks
			cut.Find("button").Click();
			Assert.Equal("[The output was changed completely.]", cut.Find("#dynamic-markup-block").TextContent.Trim());
			Assert.Equal("changed", cut.Find("#dynamic-markup-block span em").TextContent);
		}

		[Fact]
		public void CanRenderRazorTemplates()
		{
			var cut = RenderComponent<RazorTemplates>();

			// code block template (component parameter)
			var element = cut.Find("div#codeblocktemplate ol");
			Assert.Collection(
				element.QuerySelectorAll("li"),
				e => Assert.Equal("#1 - a", e.TextContent),
				e => Assert.Equal("#2 - b", e.TextContent),
				e => Assert.Equal("#3 - c", e.TextContent));
		}

		[Fact]
		public void CanRenderMultipleChildContent()
		{
			var cut = RenderComponent<MultipleChildContent>();

			var table = cut.Find("table");

			var thead = table.QuerySelector("thead");
			Assert.Collection(
				thead.QuerySelectorAll("th"),
				e => Assert.Equal("Col1", e.TextContent),
				e => Assert.Equal("Col2", e.TextContent),
				e => Assert.Equal("Col3", e.TextContent));

			var tfootElements = cut.FindAll("table tfoot td", enableAutoRefresh: true);
			Assert.Empty(tfootElements);
			var toggle = cut.Find("#toggle");
			toggle.Change(true);

			Assert.Collection(
				tfootElements,
				e => Assert.Equal("The", e.TextContent),
				e => Assert.Equal(string.Empty, e.TextContent),
				e => Assert.Equal("End", e.TextContent));
		}

		[Fact]
		public void CanAcceptSimultaneousRenderRequests()
		{
			var expectedOutput = string.Join(
				string.Empty,
				Enumerable.Range(0, 100).Select(_ => "😊"));

			var cut = RenderComponent<ConcurrentRenderParent>();

			// It's supposed to pause the rendering for this long. The WaitAssert below
			// allows it to take up extra time if needed.
			// await Task.Delay(1000);

			var outputElement = cut.Find("#concurrent-render-output");

			cut.WaitForAssertion(
				() => Assert.Equal(expectedOutput, outputElement.TextContent.Trim()),
				timeout: TimeSpan.FromMilliseconds(2000));
		}

		[Fact]
		public void CanDispatchRenderToSyncContext()
		{
			var cut = RenderComponent<DispatchingComponent>();
			var result = cut.Find("#result");

			cut.Find("#run-with-dispatch").Click();

			cut.WaitForAssertion(() => Assert.Equal("Success (completed synchronously)", result.TextContent.Trim()));
		}

		[Fact]
		public void CanDoubleDispatchRenderToSyncContext()
		{
			var cut = RenderComponent<DispatchingComponent>();
			var result = cut.Find("#result");

			cut.Find("#run-with-double-dispatch").Click();

			cut.WaitForAssertion(() => Assert.Equal("Success (completed synchronously)", result.TextContent.Trim()));
		}

		[Fact]
		public void CanDispatchAsyncWorkToSyncContext()
		{
			var cut = RenderComponent<DispatchingComponent>();
			var result = cut.Find("#result");

			cut.Find("#run-async-with-dispatch").Click();

			cut.WaitForAssertion(
				() =>
				{
					// In some cases, the original assert wont work, since the sync context might not be idle,
					// which results in this order: First Third Second Fourth Fifth
					Assert.Equal("First Second Third Fourth Fifth", result.TextContent.Trim());

					Assert.Contains("First", result.TextContent, StringComparison.Ordinal);
					Assert.Contains("Second", result.TextContent, StringComparison.Ordinal);
					Assert.Contains("Third", result.TextContent, StringComparison.Ordinal);
					Assert.Contains("Fourth", result.TextContent, StringComparison.Ordinal);
					Assert.Contains("Fifth", result.TextContent, StringComparison.Ordinal);
				},
				timeout: TimeSpan.FromSeconds(2));
		}

		// Test removed since it does not have any value in this context.
		// [Fact]
		// public void CanPerformInteropImmediatelyOnComponentInsertion()
		// {
		//     var cut = RenderComponent<InteropOnInitializationComponent>();
		//     Assert.Equal("Hello from interop call", () => cut.Find("#val-get-by-interop").TextContent);
		//     Assert.Equal("Hello from interop call", () => cut.Find("#val-set-by-interop").GetAttribute("value"));
		// }

		[Fact]
		public void CanUseAddMultipleAttributes()
		{
			var cut = RenderComponent<DuplicateAttributesComponent>();

			var element = cut.Find("#duplicate-on-element > div");
			Assert.True(element.HasAttribute("bool")); // attribute is present
			Assert.Equal("middle-value", element.GetAttribute("string"));
			Assert.Equal("unmatched-value", element.GetAttribute("unmatched"));

			element = cut.Find("#duplicate-on-element-override > div");
			Assert.False(element.HasAttribute("bool")); // attribute is not present
			Assert.Equal("other-text", element.GetAttribute("string"));
			Assert.Equal("unmatched-value", element.GetAttribute("unmatched"));
		}

		[Fact]
		public void CanPatchRenderTreeToMatchLatestDOMState()
		{
			var cut = RenderComponent<MovingCheckboxesComponent>();
			var incompleteItemsSelector = ".incomplete-items li";
			var completeItemsSelector = ".complete-items li";

			// Mark first item as done; observe the remaining incomplete item appears unchecked
			// because the diff algorithm explicitly unchecks it
			cut.Find(".incomplete-items .item-isdone").Change(true);
			var incompleteLIs = cut.FindAll(incompleteItemsSelector);
			Assert.Equal(1, incompleteLIs.Count);
			Assert.False(incompleteLIs[0].QuerySelector(".item-isdone").HasAttribute("checked"));

			// Mark first done item as not done; observe the remaining complete item appears checked
			// because the diff algorithm explicitly re-checks it
			cut.Find(".complete-items .item-isdone").Change(false);
			var completeLIs = cut.FindAll(completeItemsSelector);
			Assert.Equal(2, completeLIs.Count);
			Assert.True(completeLIs[0].QuerySelector(".item-isdone").HasAttribute("checked"));
		}
	}
}
