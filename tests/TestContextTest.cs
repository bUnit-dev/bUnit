using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BasicTestApp;
using Egil.RazorComponents.Testing.EventDispatchExtensions;
using Egil.RazorComponents.Testing.TestUtililities;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Egil.RazorComponents.Testing
{
    // FindElements\(By\.Id\("([\.\>\s\d\w-=\[\]]+)"\)\)
    public class TestContextTest : ComponentTestFixture
    {
        public TestContextTest(ITestOutputHelper output)
        {
            Services.AddXunitLogger(output);
        }

        [Fact]
        public void CanRenderTextOnlyComponent()
        {
            var appElement = RenderComponent<TextOnlyComponent>();
            Assert.Equal("Hello from TextOnlyComponent", appElement.Markup);
        }

        // This verifies that we've correctly configured the Razor language version via MSBuild.
        // See #974
        [Fact]
        public void CanRenderComponentWithDataDash()
        {
            var appElement = RenderComponent<DataDashComponent>();
            var element = appElement.Find("#cool_beans");
            Assert.Equal("17", element.GetAttribute("data-tab"));
            Assert.Equal("17", element.TextContent);
        }

        [Fact]
        public void CanRenderComponentWithAttributes()
        {
            var appElement = RenderComponent<RedTextComponent>();
            var styledElement = appElement.Find("h1");
            Assert.Equal("Hello, world!", styledElement.TextContent);
            Assert.Equal("color: red;", styledElement.GetAttribute("style"));
            Assert.Equal("somevalue", styledElement.GetAttribute("customattribute"));
        }

        [Fact]
        public void CanTriggerEvents()
        {
            // Initial count is zero
            var appElement = RenderComponent<CounterComponent>();
            var countDisplayElement = appElement.Find("p");
            Assert.Equal("Current count: 0", countDisplayElement.TextContent);

            // Clicking button increments count
            appElement.Find("button").Click();
            Assert.Equal("Current count: 1", appElement.Find("p").TextContent);
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
            Assert.Equal("Started", cut.Find("#state").TextContent);

            // Clicking 'tock' completes the task, which updates the state
            // This click causes two renders, thus WaitForNextUpdate is needed
            // to wait for the markup to be updated.
            //cut.Find("#tock").Click();
            cut.WaitForNextUpdate(() => cut.Find("#tock").Click());
            Assert.Equal("Stopped", cut.Find("#state").TextContent);
        }

        //[Fact]
        //public void CanTriggerKeyPressEvents()
        //{
        //    // List is initially empty
        //    var appElement = RenderComponent<KeyPressEventComponent>();
        //    var inputElement = appElement.Find("input");
        //    var liElements = appElement.FindAll("li");
        //    Assert.Empty(liElements);

        //    // Typing adds element
        //    inputElement.Keypress("a");
        //    Assert.All(appElement.FindAll("li"), li => Assert.Equal("a", li.TextContent));

        //    // Typing again adds another element
        //    inputElement.Keypress("b");
        //    Assert.All(appElement.FindAll("li"),
        //        li => Assert.Equal("a", li.Text),
        //        li => Assert.Equal("b", li.Text));

        //    // Textbox contains typed text
        //    Assert.Equal("ab", appElement.Find("input").GetAttribute("value"));
        //}

        //[Fact]
        //public void CanAddAndRemoveEventHandlersDynamically()
        //{
        //    var appElement = RenderComponent<CounterComponent>();
        //    var countDisplayElement = appElement.Find("p");
        //    var incrementButton = appElement.Find("button");
        //    var toggleClickHandlerCheckbox = appElement.Find("[type=checkbox]");

        //    // Initial count is zero; clicking button increments count
        //    Assert.Equal("Current count: 0", countDisplayElement.TextContent);
        //    incrementButton.Click();
        //    Assert.Equal("Current count: 1", () => countDisplayElement.TextContent);

        //    // We can remove an event handler
        //    toggleClickHandlerCheckbox.Click();
        //    Browser.Empty(() => appElement.FindAll("#listening-message"));
        //    incrementButton.Click();
        //    Assert.Equal("Current count: 1", () => countDisplayElement.TextContent);

        //    // We can add an event handler
        //    toggleClickHandlerCheckbox.Click();
        //    appElement.Find("#listening-message");
        //    incrementButton.Click();
        //    Assert.Equal("Current count: 2", () => countDisplayElement.TextContent);
        //}

        //[Fact]
        //public void CanRenderChildComponents()
        //{
        //    var appElement = RenderComponent<ParentChildComponent>();
        //    Assert.Equal("Parent component",
        //        appElement.Find("fieldset > legend").Text);

        //    var styledElement = appElement.Find("fieldset > h1");
        //    Assert.Equal("Hello, world!", styledElement.TextContent);
        //    Assert.Equal("color: red;", styledElement.GetAttribute("style"));
        //    Assert.Equal("somevalue", styledElement.GetAttribute("customattribute"));
        //}

        //// Verifies we can render HTML content as a single block
        //[Fact]
        //public void CanRenderChildContent_StaticHtmlBlock()
        //{
        //    var appElement = RenderComponent<HtmlBlockChildContent>();
        //    Assert.Equal("<p>Some-Static-Text</p>",
        //        appElement.Find("#foo").GetAttribute("innerHTML"));
        //}

        //// Verifies we can rewite more complex HTML content into blocks
        //[Fact]
        //public void CanRenderChildContent_MixedHtmlBlock()
        //{
        //    var appElement = RenderComponent<HtmlMixedChildContent>();

        //    var one = appElement.Find("#one");
        //    Assert.Equal("<p>Some-Static-Text</p>", one.GetAttribute("innerHTML"));

        //    var two = appElement.Find("#two");
        //    Assert.Equal("<span>More-Static-Text</span>", two.GetAttribute("innerHTML"));

        //    var three = appElement.Find("#three");
        //    Assert.Equal("Some-Dynamic-Text", three.GetAttribute("innerHTML"));

        //    var four = appElement.Find("#four");
        //    Assert.Equal("But this is static", four.GetAttribute("innerHTML"));
        //}

        //// Verifies we can rewrite HTML blocks with encoded HTML
        //[Fact]
        //public void CanRenderChildContent_EncodedHtmlInBlock()
        //{
        //    var appElement = RenderComponent<HtmlEncodedChildContent>();

        //    var one = appElement.Find("#one");
        //    Assert.Equal("<p>Some-Static-Text</p>", one.GetAttribute("innerHTML"));

        //    var two = appElement.Find("#two");
        //    Assert.Equal("&lt;span&gt;More-Static-Text&lt;/span&gt;", two.GetAttribute("innerHTML"));

        //    var three = appElement.Find("#three");
        //    Assert.Equal("Some-Dynamic-Text", three.GetAttribute("innerHTML"));

        //    var four = appElement.Find("#four");
        //    Assert.Equal("But this is static", four.GetAttribute("innerHTML"));
        //}

        //[Fact]
        //public void CanTriggerEventsOnChildComponents()
        //{
        //    // Counter is displayed as child component. Initial count is zero.
        //    var appElement = RenderComponent<CounterComponentWrapper>();
        //    var counterDisplay = appElement
        //        .FindAll("p")
        //        .Single(element => element.TextContent == "Current count: 0");

        //    // Clicking increments count in child component
        //    appElement.Find("button").Click();
        //    Assert.Equal("Current count: 1", () => counterDisplay.Text);
        //}

        //[Fact]
        //public void ChildComponentsRerenderWhenPropertiesChanged()
        //{
        //    // Count value is displayed in child component with initial value zero
        //    var appElement = RenderComponent<CounterComponentUsingChild>();
        //    var wholeCounterElement = appElement.Find("p");
        //    var messageElementInChild = wholeCounterElement.FindElement(By.ClassName("message"));
        //    Assert.Equal("Current count: 0", wholeCounterElement.TextContent);
        //    Assert.Equal("0", messageElementInChild.Text);

        //    // Clicking increments count in child element
        //    appElement.Find("button").Click();
        //    Assert.Equal("1", () => messageElementInChild.Text);
        //}

        //[Fact]
        //public void CanAddAndRemoveChildComponentsDynamically()
        //{
        //    // Initially there are zero child components
        //    var appElement = RenderComponent<AddRemoveChildComponents>();
        //    var addButton = appElement.FindElement(By.ClassName("addChild"));
        //    var removeButton = appElement.FindElement(By.ClassName("removeChild"));
        //    Func<IEnumerable<IWebElement>> childComponentWrappers = () => appElement.FindAll("p");
        //    Assert.Empty(childComponentWrappers());

        //    // Click to add/remove some child components
        //    addButton.Click();
        //    Browser.Collection(childComponentWrappers,
        //        elem => Assert.Equal("Child 1", elem.FindElement(By.ClassName("message")).Text));

        //    addButton.Click();
        //    Browser.Collection(childComponentWrappers,
        //        elem => Assert.Equal("Child 1", elem.FindElement(By.ClassName("message")).Text),
        //        elem => Assert.Equal("Child 2", elem.FindElement(By.ClassName("message")).Text));

        //    removeButton.Click();
        //    Browser.Collection(childComponentWrappers,
        //        elem => Assert.Equal("Child 1", elem.FindElement(By.ClassName("message")).Text));

        //    addButton.Click();
        //    Browser.Collection(childComponentWrappers,
        //        elem => Assert.Equal("Child 1", elem.FindElement(By.ClassName("message")).Text),
        //        elem => Assert.Equal("Child 3", elem.FindElement(By.ClassName("message")).Text));
        //}

        //[Fact]
        //public void ChildComponentsNotifiedWhenPropertiesChanged()
        //{
        //    // Child component receives notification that lets it compute a property before first render
        //    var appElement = RenderComponent<PropertiesChangedHandlerParent>();
        //    var suppliedValueElement = appElement.FindElement(By.ClassName("supplied"));
        //    var computedValueElement = appElement.FindElement(By.ClassName("computed"));
        //    var incrementButton = appElement.Find("button");
        //    Assert.Equal("You supplied: 100", suppliedValueElement.TextContent);
        //    Assert.Equal("I computed: 200", computedValueElement.TextContent);

        //    // When property changes, child is renotified before rerender
        //    incrementButton.Click();
        //    Assert.Equal("You supplied: 101", () => suppliedValueElement.TextContent);
        //    Assert.Equal("I computed: 202", computedValueElement.TextContent);
        //}

        //[Fact]
        //public void CanRenderFragmentsWhilePreservingSurroundingElements()
        //{
        //    // Initially, the region isn't shown
        //    var appElement = RenderComponent<RenderFragmentToggler>();
        //    var originalButton = appElement.Find("button");
        //    Func<IEnumerable<IWebElement>> fragmentElements = () => appElement.FindAll("p[name=fragment-element]");
        //    Assert.Empty(fragmentElements());

        //    // The JS-side DOM builder handles regions correctly, placing elements
        //    // after the region after the corresponding elements
        //    Assert.Equal("The end", appElement.FindElements(By.CssSelector("div > *:last-child")).Single().Text);

        //    // When we click the button, the region is shown
        //    originalButton.Click();
        //    Browser.Single(fragmentElements);

        //    // The button itself was preserved, so we can click it again and see the effect
        //    originalButton.Click();
        //    Browser.Empty(fragmentElements);
        //}

        //[Fact]
        //public void CanUseViewImportsHierarchically()
        //{
        //    // The component is able to compile and output these type names only because
        //    // of the _ViewImports.cshtml files at the same and ancestor levels
        //    var appElement = RenderComponent<ComponentUsingImports>();
        //    Assert.Collection(appElement.FindAll("p"),
        //        elem => Assert.Equal(typeof(Complex).FullName, elem.Text),
        //        elem => Assert.Equal(typeof(AssemblyHashAlgorithm).FullName, elem.Text));
        //}

        //[Fact]
        //public void CanUseComponentAndStaticContentFromExternalNuGetPackage()
        //{
        //    var appElement = RenderComponent<ExternalContentPackage>();

        //    // NuGet packages can use JS interop features to provide
        //    // .NET code access to browser APIs
        //    var showPromptButton = appElement.FindAll("button").First();
        //    showPromptButton.Click();

        //    var modal = new WebDriverWait(Browser, TimeSpan.FromSeconds(3))
        //        .Until(SwitchToAlert);
        //    modal.SendKeys("Some value from test");
        //    modal.Accept();
        //    var promptResult = appElement.Find("strong");
        //    Assert.Equal("Some value from test", () => promptResult.Text);

        //    // NuGet packages can also embed entire components (themselves
        //    // authored as Razor files), including static content. The CSS value
        //    // here is in a .css file, so if it's correct we know that static content
        //    // file was loaded.
        //    var specialStyleDiv = appElement.FindElement(By.ClassName("special-style"));
        //    Assert.Equal("50px", specialStyleDiv.GetCssValue("padding"));

        //    // The external components are fully functional, not just static HTML
        //    var externalComponentButton = specialStyleDiv.Find("button");
        //    Assert.Equal("Click me", externalComponentButton.Text);
        //    externalComponentButton.Click();
        //    Assert.Equal("It works", () => externalComponentButton.Text);
        //}

        //[Fact]
        //public void CanRenderSvgWithCorrectNamespace()
        //{
        //    var appElement = RenderComponent<SvgComponent>();

        //    var svgElement = appElement.FindElement(By.XPath("//*[local-name()='svg' and namespace-uri()='http://www.w3.org/2000/svg']"));
        //    Assert.NotNull(svgElement);

        //    var svgCircleElement = appElement.FindElement(By.XPath("//*[local-name()='circle' and namespace-uri()='http://www.w3.org/2000/svg']"));
        //    Assert.NotNull(svgCircleElement);
        //    Assert.Equal("10", svgCircleElement.GetAttribute("r"));

        //    appElement.Find("button").Click();
        //    Assert.Equal("20", () => svgCircleElement.GetAttribute("r"));
        //}

        //[Fact]
        //public void CanRenderSvgChildComponentWithCorrectNamespace()
        //{
        //    var appElement = RenderComponent<SvgWithChildComponent>();

        //    var svgElement = appElement.FindElement(By.XPath("//*[local-name()='svg' and namespace-uri()='http://www.w3.org/2000/svg']"));
        //    Assert.NotNull(svgElement);

        //    var svgCircleElement = appElement.FindElement(By.XPath("//*[local-name()='circle' and namespace-uri()='http://www.w3.org/2000/svg']"));
        //    Assert.NotNull(svgCircleElement);
        //}

        //[Fact]
        //public void LogicalElementInsertionWorksHierarchically()
        //{
        //    var appElement = RenderComponent<LogicalElementInsertionCases>();
        //    Assert.Equal("First Second Third", () => appElement.TextContent);
        //}

        //[Fact]
        //public void CanUseJsInteropToReferenceElements()
        //{
        //    var appElement = RenderComponent<ElementRefComponent>();
        //    var inputElement = appElement.Find("#capturedElement");
        //    var buttonElement = appElement.Find("button");

        //    Assert.Equal(string.Empty, inputElement.GetAttribute("value"));

        //    buttonElement.Click();
        //    Assert.Equal("Clicks: 1", () => inputElement.GetAttribute("value"));
        //    buttonElement.Click();
        //    Assert.Equal("Clicks: 2", () => inputElement.GetAttribute("value"));
        //}

        //[Fact]
        //public void CanCaptureReferencesToDynamicallyAddedElements()
        //{
        //    var appElement = RenderComponent<ElementRefComponent>();
        //    var buttonElement = appElement.Find("button");
        //    var checkbox = appElement.Find("input[type=checkbox]");

        //    // We're going to remove the input. But first, put in some contents
        //    // so we can observe it's not the same instance later
        //    appElement.Find("#capturedElement").SendKeys("some text");

        //    // Remove the captured element
        //    checkbox.Click();
        //    Browser.Empty(() => appElement.FindAll("#capturedElement"));

        //    // Re-add it; observe it starts empty again
        //    checkbox.Click();
        //    var inputElement = appElement.Find("#capturedElement");
        //    Assert.Equal(string.Empty, inputElement.GetAttribute("value"));

        //    // See that the capture variable was automatically updated to reference the new instance
        //    buttonElement.Click();
        //    Assert.Equal("Clicks: 1", () => inputElement.GetAttribute("value"));
        //}

        //[Fact]
        //public void CanCaptureReferencesToDynamicallyAddedComponents()
        //{
        //    var appElement = RenderComponent<ComponentRefComponent>();
        //    var incrementButtonSelector = By.CssSelector("#child-component button");
        //    var currentCountTextSelector = By.CssSelector("#child-component p:first-of-type");
        //    var resetButton = appElement.Find("#reset-child");
        //    var toggleChildCheckbox = appElement.Find("#toggle-child");
        //    Func<string> currentCountText = () => appElement.FindElement(currentCountTextSelector).Text;

        //    // Verify the reference was captured initially
        //    appElement.FindElement(incrementButtonSelector).Click();
        //    Assert.Equal("Current count: 1", currentCountText);
        //    resetButton.Click();
        //    Assert.Equal("Current count: 0", currentCountText);
        //    appElement.FindElement(incrementButtonSelector).Click();
        //    Assert.Equal("Current count: 1", currentCountText);

        //    // Remove and re-add a new instance of the child, checking the text was reset
        //    toggleChildCheckbox.Click();
        //    Browser.Empty(() => appElement.FindElements(incrementButtonSelector));
        //    toggleChildCheckbox.Click();
        //    Assert.Equal("Current count: 0", currentCountText);

        //    // Verify we have a new working reference
        //    appElement.FindElement(incrementButtonSelector).Click();
        //    Assert.Equal("Current count: 1", currentCountText);
        //    resetButton.Click();
        //    Assert.Equal("Current count: 0", currentCountText);
        //}

        //[Fact]
        //public void CanUseJsInteropForRefElementsDuringOnAfterRender()
        //{
        //    var appElement = RenderComponent<AfterRenderInteropComponent>();
        //    Assert.Equal("Value set after render", () => Browser.Find("input").GetAttribute("value"));
        //}

        //[Fact]
        //public void CanRenderMarkupBlocks()
        //{
        //    var appElement = RenderComponent<MarkupBlockComponent>();

        //    // Static markup
        //    Assert.Equal(
        //        "attributes",
        //        appElement.FindElement(By.CssSelector("p span#attribute-example")).Text);

        //    // Dynamic markup (from a custom RenderFragment)
        //    Assert.Equal(
        //        "[Here is an example. We support multiple-top-level nodes.]",
        //        appElement.Find("#dynamic-markup-block").Text);
        //    Assert.Equal(
        //        "example",
        //        appElement.FindElement(By.CssSelector("#dynamic-markup-block strong#dynamic-element em")).Text);

        //    // Dynamic markup (from a MarkupString)
        //    Assert.Equal(
        //        "This is a markup string.",
        //        appElement.FindElement(By.ClassName("markup-string-value")).Text);
        //    Assert.Equal(
        //        "markup string",
        //        appElement.Find(".markup-string-value em").Text);

        //    // Updating markup blocks
        //    appElement.Find("button").Click();
        //    Browser.Equal(
        //        "[The output was changed completely.]",
        //        () => appElement.Find("#dynamic-markup-block").Text);
        //    Assert.Equal(
        //        "changed",
        //        appElement.FindElement(By.CssSelector("#dynamic-markup-block span em")).Text);
        //}

        //[Fact]
        //public void CanRenderRazorTemplates()
        //{
        //    var appElement = RenderComponent<RazorTemplates>();

        //    // code block template (component parameter)
        //    var element = appElement.FindElement(By.CssSelector("div#codeblocktemplate ol"));
        //    Assert.Collection(
        //        element.FindAll("li"),
        //        e => Assert.Equal("#1 - a", e.Text),
        //        e => Assert.Equal("#2 - b", e.Text),
        //        e => Assert.Equal("#3 - c", e.Text));
        //}

        //[Fact]
        //public void CanRenderMultipleChildContent()
        //{
        //    var appElement = RenderComponent<MultipleChildContent>();

        //    var table = appElement.Find("table");

        //    var thead = table.Find("thead");
        //    Assert.Collection(
        //        thead.FindAll("th"),
        //        e => Assert.Equal("Col1", e.Text),
        //        e => Assert.Equal("Col2", e.Text),
        //        e => Assert.Equal("Col3", e.Text));

        //    var tfoot = table.Find("tfoot");
        //    Assert.Empty(tfoot.FindAll("td"));

        //    var toggle = appElement.Find("#toggle");
        //    toggle.Click();

        //    Browser.Collection(
        //        () => tfoot.FindAll("td"),
        //        e => Assert.Equal("The", e.Text),
        //        e => Assert.Equal("", e.Text),
        //        e => Assert.Equal("End", e.Text));
        //}

        //[Fact]
        //public async Task CanAcceptSimultaneousRenderRequests()
        //{
        //    var expectedOutput = string.Join(
        //        string.Empty,
        //        Enumerable.Range(0, 100).Select(_ => "😊"));

        //    var appElement = RenderComponent<ConcurrentRenderParent>();

        //    // It's supposed to pause the rendering for this long. The WaitAssert below
        //    // allows it to take up extra time if needed.
        //    await Task.Delay(1000);

        //    var outputElement = appElement.Find("#concurrent-render-output");
        //    Assert.Equal(expectedOutput, () => outputElement.TextContent);
        //}

        //[Fact]
        //public void CanDispatchRenderToSyncContext()
        //{
        //    var appElement = RenderComponent<DispatchingComponent>();
        //    var result = appElement.Find("#result");

        //    appElement.Find("#run-with-dispatch").Click();

        //    Assert.Equal("Success (completed synchronously)", () => result.Text);
        //}

        //[Fact]
        //public void CanDoubleDispatchRenderToSyncContext()
        //{
        //    var appElement = RenderComponent<DispatchingComponent>();
        //    var result = appElement.Find("#result");

        //    appElement.Find("#run-with-double-dispatch").Click();

        //    Assert.Equal("Success (completed synchronously)", () => result.Text);
        //}

        //[Fact]
        //public void CanDispatchAsyncWorkToSyncContext()
        //{
        //    var appElement = RenderComponent<DispatchingComponent>();
        //    var result = appElement.Find("#result");

        //    appElement.Find("#run-async-with-dispatch").Click();

        //    Assert.Equal("First Second Third Fourth Fifth", () => result.Text);
        //}

        //[Fact]
        //public void CanPerformInteropImmediatelyOnComponentInsertion()
        //{
        //    var appElement = RenderComponent<InteropOnInitializationComponent>();
        //    Assert.Equal("Hello from interop call", () => appElement.Find("#val-get-by-interop").Text);
        //    Assert.Equal("Hello from interop call", () => appElement.Find("#val-set-by-interop").GetAttribute("value"));
        //}

        //[Fact]
        //public void CanUseAddMultipleAttributes()
        //{
        //    var appElement = RenderComponent<DuplicateAttributesComponent>();

        //    var selector = By.CssSelector("#duplicate-on-element > div");
        //    Browser.Exists(selector);

        //    var element = appElement.FindElement(selector);
        //    Assert.Equal(string.Empty, element.GetAttribute("bool")); // attribute is present
        //    Assert.Equal("middle-value", element.GetAttribute("string"));
        //    Assert.Equal("unmatched-value", element.GetAttribute("unmatched"));

        //    selector = By.CssSelector("#duplicate-on-element-override > div");
        //    element = appElement.FindElement(selector);
        //    Assert.Null(element.GetAttribute("bool")); // attribute is not present
        //    Assert.Equal("other-text", element.GetAttribute("string"));
        //    Assert.Equal("unmatched-value", element.GetAttribute("unmatched"));
        //}

        //[Fact]
        //public void CanPatchRenderTreeToMatchLatestDOMState()
        //{
        //    var appElement = RenderComponent<MovingCheckboxesComponent>();
        //    var incompleteItemsSelector = By.CssSelector(".incomplete-items li");
        //    var completeItemsSelector = By.CssSelector(".complete-items li");
        //    Browser.Exists(incompleteItemsSelector);

        //    // Mark first item as done; observe the remaining incomplete item appears unchecked
        //    // because the diff algorithm explicitly unchecks it
        //    appElement.Find(".incomplete-items .item-isdone").Click();
        //    Browser.True(() =>
        //    {
        //        var incompleteLIs = appElement.FindElements(incompleteItemsSelector);
        //        return incompleteLIs.Count == 1
        //            && !incompleteLIs[0].Find(".item-isdone").Selected;
        //    });

        //    // Mark first done item as not done; observe the remaining complete item appears checked
        //    // because the diff algorithm explicitly re-checks it
        //    appElement.Find(".complete-items .item-isdone").Click();
        //    Browser.True(() =>
        //    {
        //        var completeLIs = appElement.FindElements(completeItemsSelector);
        //        return completeLIs.Count == 2
        //            && completeLIs[0].Find(".item-isdone").Selected;
        //    });
        //}

    }
}
