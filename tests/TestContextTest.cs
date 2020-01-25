using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using BasicTestApp;
using DeepEqual.Syntax;
using Egil.RazorComponents.Testing.EventDispatchExtensions;
using Egil.RazorComponents.Testing.Extensions;
using Egil.RazorComponents.Testing.TestUtililities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
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
            Assert.Equal("Current count: 1", cut.Find("p").TextContent);
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
            cut.WaitForNextUpdate(() => cut.Find("#tock").Click());
            Assert.Equal("Stopped", cut.Find("#state").TextContent);
        }

        [Fact]
        public void CanTriggerKeyPressEvents()
        {
            // List is initially empty
            var cut = RenderComponent<KeyPressEventComponent>();
            var inputElement = cut.Find("input");
            var liElements = cut.FindAll("li");
            Assert.Empty(liElements);

            // Typing adds element
            inputElement.KeyPress("a");
            Assert.Collection(cut.FindAll("li"), li => Assert.Equal("a", li.TextContent));

            // Typing again adds another element
            inputElement.KeyPress("b");
            Assert.Collection(cut.FindAll("li"),
                li => Assert.Equal("a", li.TextContent),
                li => Assert.Equal("b", li.TextContent)
            );
        }

        [Fact(DisplayName = "After KeyPress event is triggered, <input value=''> contains keys passed to KeyPress", Skip = "Issue #46")]
        public void Test001()
        {
            var cut = RenderComponent<KeyPressEventComponent>();

            cut.Find("input").KeyPress("abc");

            cut.Find("input").GetAttribute("value").ShouldBe("abc");
        }

        [Fact]
        public void CanAddAndRemoveEventHandlersDynamically()
        {
            var cut = RenderComponent<CounterComponent>();

            // Initial count is zero; clicking button increments count
            Assert.Equal("Current count: 0", cut.Find("p").TextContent);
            cut.Find("button").Click();
            Assert.Equal("Current count: 1", cut.Find("p").TextContent);

            // We can remove an event handler
            cut.Find("[type=checkbox]").Change(false);
            cut.FindAll("#listening-message").ShouldBeEmpty();

            cut.Find("button").Click();
            Assert.Equal("Current count: 1", cut.Find("p").TextContent);

            // We can add an event handler
            cut.Find("[type=checkbox]").Change(true);
            cut.Find("#listening-message");
            cut.Find("button").Click();
            Assert.Equal("Current count: 2", cut.Find("p").TextContent);
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
            Assert.Equal("Current count: 0", cut.Find("p").TextContent);
            Assert.Equal("0", cut.Find("p .message").TextContent);

            // Clicking increments count in child element
            cut.Find("button").Click();
            Assert.Equal("1", cut.Find("p .message").TextContent);
        }

        [Fact]
        public void CanAddAndRemoveChildComponentsDynamically()
        {
            // Initially there are zero child components
            var cut = RenderComponent<AddRemoveChildComponents>();
            Assert.Empty(cut.FindAll("p"));

            // Click to add/remove some child components
            cut.Find(".addChild").Click();
            Assert.Collection(cut.FindAll("p"), elem => Assert.Equal("Child 1", elem.Find(".message").TextContent));

            cut.Find(".addChild").Click();
            Assert.Collection(cut.FindAll("p"),
                elem => Assert.Equal("Child 1", elem.Find(".message").TextContent),
                elem => Assert.Equal("Child 2", elem.Find(".message").TextContent));

            cut.Find(".removeChild").Click();
            Assert.Collection(cut.FindAll("p"),
                elem => Assert.Equal("Child 1", elem.Find(".message").TextContent));

            cut.Find(".addChild").Click();
            Assert.Collection(cut.FindAll("p"),
                elem => Assert.Equal("Child 1", elem.Find(".message").TextContent),
                elem => Assert.Equal("Child 3", elem.Find(".message").TextContent));
        }

        //[Fact]
        //public void ChildComponentsNotifiedWhenPropertiesChanged()
        //{
        //    // Child component receives notification that lets it compute a property before first render
        //    var cut = RenderComponent<PropertiesChangedHandlerParent>();
        //    var suppliedValueElement = cut.Find(".supplied");
        //    var computedValueElement = cut.Find(".computed");
        //    var incrementButton = cut.Find("button");
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
        //    var cut = RenderComponent<RenderFragmentToggler>();
        //    var originalButton = cut.Find("button");
        //    Func<IEnumerable<IWebElement>> fragmentElements = () => cut.FindAll("p[name=fragment-element]");
        //    Assert.Empty(fragmentElements());

        //    // The JS-side DOM builder handles regions correctly, placing elements
        //    // after the region after the corresponding elements
        //    Assert.Equal("The end", cut.FindElements(By.CssSelector("div > *:last-child")).Single().TextContent);

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
        //    var cut = RenderComponent<ComponentUsingImports>();
        //    Assert.Collection(cut.FindAll("p"),
        //        elem => Assert.Equal(typeof(Complex).FullName, elem.TextContent),
        //        elem => Assert.Equal(typeof(AssemblyHashAlgorithm).FullName, elem.TextContent));
        //}

        //[Fact]
        //public void CanUseComponentAndStaticContentFromExternalNuGetPackage()
        //{
        //    var cut = RenderComponent<ExternalContentPackage>();

        //    // NuGet packages can use JS interop features to provide
        //    // .NET code access to browser APIs
        //    var showPromptButton = cut.FindAll("button").First();
        //    showPromptButton.Click();

        //    var modal = new WebDriverWait(Browser, TimeSpan.FromSeconds(3))
        //        .Until(SwitchToAlert);
        //    modal.SendKeys("Some value from test");
        //    modal.Accept();
        //    var promptResult = cut.Find("strong");
        //    Assert.Equal("Some value from test", () => promptResult.TextContent);

        //    // NuGet packages can also embed entire components (themselves
        //    // authored as Razor files), including static content. The CSS value
        //    // here is in a .css file, so if it's correct we know that static content
        //    // file was loaded.
        //    var specialStyleDiv = cut.FindElement(By.ClassName("special-style"));
        //    Assert.Equal("50px", specialStyleDiv.GetCssValue("padding"));

        //    // The external components are fully functional, not just static HTML
        //    var externalComponentButton = specialStyleDiv.Find("button");
        //    Assert.Equal("Click me", externalComponentButton.TextContent);
        //    externalComponentButton.Click();
        //    Assert.Equal("It works", () => externalComponentButton.TextContent);
        //}

        //[Fact]
        //public void CanRenderSvgWithCorrectNamespace()
        //{
        //    var cut = RenderComponent<SvgComponent>();

        //    var svgElement = cut.FindElement(By.XPath("//*[local-name()='svg' and namespace-uri()='http://www.w3.org/2000/svg']"));
        //    Assert.NotNull(svgElement);

        //    var svgCircleElement = cut.FindElement(By.XPath("//*[local-name()='circle' and namespace-uri()='http://www.w3.org/2000/svg']"));
        //    Assert.NotNull(svgCircleElement);
        //    Assert.Equal("10", svgCircleElement.GetAttribute("r"));

        //    cut.Find("button").Click();
        //    Assert.Equal("20", () => svgCircleElement.GetAttribute("r"));
        //}

        //[Fact]
        //public void CanRenderSvgChildComponentWithCorrectNamespace()
        //{
        //    var cut = RenderComponent<SvgWithChildComponent>();

        //    var svgElement = cut.FindElement(By.XPath("//*[local-name()='svg' and namespace-uri()='http://www.w3.org/2000/svg']"));
        //    Assert.NotNull(svgElement);

        //    var svgCircleElement = cut.FindElement(By.XPath("//*[local-name()='circle' and namespace-uri()='http://www.w3.org/2000/svg']"));
        //    Assert.NotNull(svgCircleElement);
        //}

        //[Fact]
        //public void LogicalElementInsertionWorksHierarchically()
        //{
        //    var cut = RenderComponent<LogicalElementInsertionCases>();
        //    Assert.Equal("First Second Third", () => cut.TextContent);
        //}

        //[Fact]
        //public void CanUseJsInteropToReferenceElements()
        //{
        //    var cut = RenderComponent<ElementRefComponent>();
        //    var inputElement = cut.Find("#capturedElement");
        //    var buttonElement = cut.Find("button");

        //    Assert.Equal(string.Empty, inputElement.GetAttribute("value"));

        //    buttonElement.Click();
        //    Assert.Equal("Clicks: 1", () => inputElement.GetAttribute("value"));
        //    buttonElement.Click();
        //    Assert.Equal("Clicks: 2", () => inputElement.GetAttribute("value"));
        //}

        //[Fact]
        //public void CanCaptureReferencesToDynamicallyAddedElements()
        //{
        //    var cut = RenderComponent<ElementRefComponent>();
        //    var buttonElement = cut.Find("button");
        //    var checkbox = cut.Find("input[type=checkbox]");

        //    // We're going to remove the input. But first, put in some contents
        //    // so we can observe it's not the same instance later
        //    cut.Find("#capturedElement").SendKeys("some text");

        //    // Remove the captured element
        //    checkbox.Click();
        //    Browser.Empty(() => cut.FindAll("#capturedElement"));

        //    // Re-add it; observe it starts empty again
        //    checkbox.Click();
        //    var inputElement = cut.Find("#capturedElement");
        //    Assert.Equal(string.Empty, inputElement.GetAttribute("value"));

        //    // See that the capture variable was automatically updated to reference the new instance
        //    buttonElement.Click();
        //    Assert.Equal("Clicks: 1", () => inputElement.GetAttribute("value"));
        //}

        //[Fact]
        //public void CanCaptureReferencesToDynamicallyAddedComponents()
        //{
        //    var cut = RenderComponent<ComponentRefComponent>();
        //    var incrementButtonSelector = By.CssSelector("#child-component button");
        //    var currentCountTextSelector = By.CssSelector("#child-component p:first-of-type");
        //    var resetButton = cut.Find("#reset-child");
        //    var toggleChildCheckbox = cut.Find("#toggle-child");
        //    Func<string> currentCountText = () => cut.FindElement(currentCountTextSelector).Text;

        //    // Verify the reference was captured initially
        //    cut.FindElement(incrementButtonSelector).Click();
        //    Assert.Equal("Current count: 1", currentCountText);
        //    resetButton.Click();
        //    Assert.Equal("Current count: 0", currentCountText);
        //    cut.FindElement(incrementButtonSelector).Click();
        //    Assert.Equal("Current count: 1", currentCountText);

        //    // Remove and re-add a new instance of the child, checking the text was reset
        //    toggleChildCheckbox.Click();
        //    Browser.Empty(() => cut.FindElements(incrementButtonSelector));
        //    toggleChildCheckbox.Click();
        //    Assert.Equal("Current count: 0", currentCountText);

        //    // Verify we have a new working reference
        //    cut.FindElement(incrementButtonSelector).Click();
        //    Assert.Equal("Current count: 1", currentCountText);
        //    resetButton.Click();
        //    Assert.Equal("Current count: 0", currentCountText);
        //}

        //[Fact]
        //public void CanUseJsInteropForRefElementsDuringOnAfterRender()
        //{
        //    var cut = RenderComponent<AfterRenderInteropComponent>();
        //    Assert.Equal("Value set after render", () => Browser.Find("input").GetAttribute("value"));
        //}

        //[Fact]
        //public void CanRenderMarkupBlocks()
        //{
        //    var cut = RenderComponent<MarkupBlockComponent>();

        //    // Static markup
        //    Assert.Equal(
        //        "attributes",
        //        cut.FindElement(By.CssSelector("p span#attribute-example")).TextContent);

        //    // Dynamic markup (from a custom RenderFragment)
        //    Assert.Equal(
        //        "[Here is an example. We support multiple-top-level nodes.]",
        //        cut.Find("#dynamic-markup-block").TextContent);
        //    Assert.Equal(
        //        "example",
        //        cut.FindElement(By.CssSelector("#dynamic-markup-block strong#dynamic-element em")).TextContent);

        //    // Dynamic markup (from a MarkupString)
        //    Assert.Equal(
        //        "This is a markup string.",
        //        cut.FindElement(By.ClassName("markup-string-value")).TextContent);
        //    Assert.Equal(
        //        "markup string",
        //        cut.Find(".markup-string-value em").TextContent);

        //    // Updating markup blocks
        //    cut.Find("button").Click();
        //    Browser.Equal(
        //        "[The output was changed completely.]",
        //        () => cut.Find("#dynamic-markup-block").TextContent);
        //    Assert.Equal(
        //        "changed",
        //        cut.FindElement(By.CssSelector("#dynamic-markup-block span em")).TextContent);
        //}

        //[Fact]
        //public void CanRenderRazorTemplates()
        //{
        //    var cut = RenderComponent<RazorTemplates>();

        //    // code block template (component parameter)
        //    var element = cut.FindElement(By.CssSelector("div#codeblocktemplate ol"));
        //    Assert.Collection(
        //        element.FindAll("li"),
        //        e => Assert.Equal("#1 - a", e.TextContent),
        //        e => Assert.Equal("#2 - b", e.TextContent),
        //        e => Assert.Equal("#3 - c", e.TextContent));
        //}

        //[Fact]
        //public void CanRenderMultipleChildContent()
        //{
        //    var cut = RenderComponent<MultipleChildContent>();

        //    var table = cut.Find("table");

        //    var thead = table.Find("thead");
        //    Assert.Collection(
        //        thead.FindAll("th"),
        //        e => Assert.Equal("Col1", e.TextContent),
        //        e => Assert.Equal("Col2", e.TextContent),
        //        e => Assert.Equal("Col3", e.TextContent));

        //    var tfoot = table.Find("tfoot");
        //    Assert.Empty(tfoot.FindAll("td"));

        //    var toggle = cut.Find("#toggle");
        //    toggle.Click();

        //    Browser.Collection(
        //        () => tfoot.FindAll("td"),
        //        e => Assert.Equal("The", e.TextContent),
        //        e => Assert.Equal("", e.TextContent),
        //        e => Assert.Equal("End", e.TextContent));
        //}

        //[Fact]
        //public async Task CanAcceptSimultaneousRenderRequests()
        //{
        //    var expectedOutput = string.Join(
        //        string.Empty,
        //        Enumerable.Range(0, 100).Select(_ => "😊"));

        //    var cut = RenderComponent<ConcurrentRenderParent>();

        //    // It's supposed to pause the rendering for this long. The WaitAssert below
        //    // allows it to take up extra time if needed.
        //    await Task.Delay(1000);

        //    var outputElement = cut.Find("#concurrent-render-output");
        //    Assert.Equal(expectedOutput, () => outputElement.TextContent);
        //}

        //[Fact]
        //public void CanDispatchRenderToSyncContext()
        //{
        //    var cut = RenderComponent<DispatchingComponent>();
        //    var result = cut.Find("#result");

        //    cut.Find("#run-with-dispatch").Click();

        //    Assert.Equal("Success (completed synchronously)", () => result.TextContent);
        //}

        //[Fact]
        //public void CanDoubleDispatchRenderToSyncContext()
        //{
        //    var cut = RenderComponent<DispatchingComponent>();
        //    var result = cut.Find("#result");

        //    cut.Find("#run-with-double-dispatch").Click();

        //    Assert.Equal("Success (completed synchronously)", () => result.TextContent);
        //}

        //[Fact]
        //public void CanDispatchAsyncWorkToSyncContext()
        //{
        //    var cut = RenderComponent<DispatchingComponent>();
        //    var result = cut.Find("#result");

        //    cut.Find("#run-async-with-dispatch").Click();

        //    Assert.Equal("First Second Third Fourth Fifth", () => result.TextContent);
        //}

        //[Fact]
        //public void CanPerformInteropImmediatelyOnComponentInsertion()
        //{
        //    var cut = RenderComponent<InteropOnInitializationComponent>();
        //    Assert.Equal("Hello from interop call", () => cut.Find("#val-get-by-interop").TextContent);
        //    Assert.Equal("Hello from interop call", () => cut.Find("#val-set-by-interop").GetAttribute("value"));
        //}

        //[Fact]
        //public void CanUseAddMultipleAttributes()
        //{
        //    var cut = RenderComponent<DuplicateAttributesComponent>();

        //    var selector = By.CssSelector("#duplicate-on-element > div");
        //    Browser.Exists(selector);

        //    var element = cut.FindElement(selector);
        //    Assert.Equal(string.Empty, element.GetAttribute("bool")); // attribute is present
        //    Assert.Equal("middle-value", element.GetAttribute("string"));
        //    Assert.Equal("unmatched-value", element.GetAttribute("unmatched"));

        //    selector = By.CssSelector("#duplicate-on-element-override > div");
        //    element = cut.FindElement(selector);
        //    Assert.Null(element.GetAttribute("bool")); // attribute is not present
        //    Assert.Equal("other-text", element.GetAttribute("string"));
        //    Assert.Equal("unmatched-value", element.GetAttribute("unmatched"));
        //}

        //[Fact]
        //public void CanPatchRenderTreeToMatchLatestDOMState()
        //{
        //    var cut = RenderComponent<MovingCheckboxesComponent>();
        //    var incompleteItemsSelector = By.CssSelector(".incomplete-items li");
        //    var completeItemsSelector = By.CssSelector(".complete-items li");
        //    Browser.Exists(incompleteItemsSelector);

        //    // Mark first item as done; observe the remaining incomplete item appears unchecked
        //    // because the diff algorithm explicitly unchecks it
        //    cut.Find(".incomplete-items .item-isdone").Click();
        //    Browser.True(() =>
        //    {
        //        var incompleteLIs = cut.FindElements(incompleteItemsSelector);
        //        return incompleteLIs.Count == 1
        //            && !incompleteLIs[0].Find(".item-isdone").Selected;
        //    });

        //    // Mark first done item as not done; observe the remaining complete item appears checked
        //    // because the diff algorithm explicitly re-checks it
        //    cut.Find(".complete-items .item-isdone").Click();
        //    Browser.True(() =>
        //    {
        //        var completeLIs = cut.FindElements(completeItemsSelector);
        //        return completeLIs.Count == 2
        //            && completeLIs[0].Find(".item-isdone").Selected;
        //    });
        //}

    }
}
