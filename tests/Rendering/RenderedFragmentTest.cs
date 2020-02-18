using Bunit.Extensions.Xunit;
using Bunit.Mocking.JSInterop;
using Bunit.SampleComponents;
using Bunit.SampleComponents.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Bunit
{
    public class RenderedFragmentTest : ComponentTestFixture
    {
        public RenderedFragmentTest(ITestOutputHelper output)
        {
            Services.AddXunitLogger(output);
        }

        [Fact(DisplayName = "Find throws an exception if no element matches the css selector")]
        public void Test001()
        {
            var cut = RenderComponent<Wrapper>();
            Should.Throw<ElementNotFoundException>(() => cut.Find("div"));
        }

        [Fact(DisplayName = "Find returns expected element that matches the css selector")]
        public void Test002()
        {
            var cut = RenderComponent<Wrapper>(ChildContent("<div>"));
            var result = cut.Find("div");
            result.ShouldNotBeNull();
        }

        [Fact(DisplayName = "Nodes should return new instance when " +
                            "async operation during OnInit causes component to re-render")]
        public void Test003()
        {
            var testData = new AsyncNameDep();
            Services.AddSingleton<IAsyncTestDep>(testData);
            var cut = RenderComponent<SimpleWithAyncDeps>();
            var initialValue = cut.Nodes.Find("p").TextContent;
            var expectedValue = "Steve Sanderson";

            WaitForNextRender(() => testData.SetResult(expectedValue));

            var steveValue = cut.Nodes.Find("p").TextContent;
            steveValue.ShouldNotBe(initialValue);
            steveValue.ShouldBe(expectedValue);
        }

        [Fact(DisplayName = "Nodes should return new instance when " +
                    "async operation/StateHasChanged during OnAfterRender causes component to re-render")]
        public void Test004()
        {
            var invocation = Services.AddMockJsRuntime().Setup<string>("getdata");
            var cut = RenderComponent<SimpleWithJsRuntimeDep>();
            var initialValue = cut.Nodes.Find("p").OuterHtml;

            WaitForNextRender(() => invocation.SetResult("Steve Sanderson"));

            var steveValue = cut.Nodes.Find("p").OuterHtml;
            steveValue.ShouldNotBe(initialValue);
        }

        [Fact(DisplayName = "Nodes on a components with child component returns " +
                            "new instance when the child component has changes")]
        public void Test005()
        {
            var invocation = Services.AddMockJsRuntime().Setup<string>("getdata");
            var notcut = RenderComponent<Wrapper>(ChildContent<Simple1>());
            var cut = RenderComponent<Wrapper>(ChildContent<SimpleWithJsRuntimeDep>());
            var initialValue = cut.Nodes;

            WaitForNextRender(() => invocation.SetResult("Steve Sanderson"), TimeSpan.FromSeconds(2));

            Assert.NotSame(initialValue, cut.Nodes);
        }


        [Fact(DisplayName = "Nodes should return new instance " +
                            "when a event handler trigger has caused changes to DOM tree")]
        public void Test006()
        {
            var cut = RenderComponent<ClickCounter>();
            var initialNodes = cut.Nodes;

            cut.Find("button").Click();

            Assert.NotSame(initialNodes, cut.Nodes);
        }

        [Fact(DisplayName = "Nodes should return new instance " +
                            "when a nested component has caused the DOM tree to change")]
        public void Test007()
        {
            var cut = RenderComponent<Wrapper>(
                ChildContent<CascadingValue<string>>(
                    ("Value", "FOO"),
                    ChildContent<ClickCounter>()
                )
            );
            var initialNodes = cut.Nodes;

            cut.Find("button").Click();

            Assert.NotSame(initialNodes, cut.Nodes);
        }

        [Fact(DisplayName = "Nodes should return the same instance " +
                            "when a re-render does not causes the DOM to change")]
        public void Test008()
        {
            var cut = RenderComponent<RenderOnClick>();
            var initialNodes = cut.Nodes;

            cut.Find("button").Click();

            cut.Instance.RenderCount.ShouldBe(2);
            Assert.Same(initialNodes, cut.Nodes);
        }

        [Fact(DisplayName = "Changes to event handler should return a new instance of DOM tree")]
        public void Test009()
        {
            var cut = RenderComponent<ToggleClickHandler>();
            cut.Find("#btn").Click();

            cut.Instance.Counter.ShouldBe(1);

            cut.SetParametersAndRender((nameof(ToggleClickHandler.HandleClicks), false));

            cut.Find("#btn").Click();

            cut.Instance.Counter.ShouldBe(1);
        }

        [Fact(DisplayName = "GetComponent returns first component of requested type using a depth first search")]
        public void Test100()
        {
            var wrapper = RenderComponent<TwoComponentWrapper>(
                RenderFragment<Wrapper>(nameof(TwoComponentWrapper.First),
                    ChildContent<Simple1>((nameof(Simple1.Header), "First")
                )),
                RenderFragment<Simple1>(nameof(TwoComponentWrapper.Second), (nameof(Simple1.Header), "Second"))
            );
            var cut = wrapper.FindComponent<Simple1>();

            cut.Instance.Header.ShouldBe("First");
        }

        [Fact(DisplayName = "GetComponent returns CUT if it is the first component of the requested type")]
        public void Test101()
        {
            var cut = RenderComponent<Simple1>();

            var cutAgain = cut.FindComponent<Simple1>();

            cut.Instance.ShouldBe(cutAgain.Instance);
        }

        [Fact(DisplayName = "GetComponent throws when component of requested type is not in the render tree")]
        public void Test102()
        {
            var wrapper = RenderComponent<Wrapper>();

            Should.Throw<ComponentNotFoundException>(() => wrapper.FindComponent<Simple1>());
        }

        [Fact(DisplayName = "GetComponents returns all components of requested type using a depth first order")]
        public void Test103()
        {
            var wrapper = RenderComponent<TwoComponentWrapper>(
                RenderFragment<Wrapper>(nameof(TwoComponentWrapper.First),
                    ChildContent<Simple1>((nameof(Simple1.Header), "First")
                )),
                RenderFragment<Simple1>(nameof(TwoComponentWrapper.Second), (nameof(Simple1.Header), "Second"))
            );
            var cuts = wrapper.FindComponents<Simple1>();

            cuts.Count.ShouldBe(2);
            cuts[0].Instance.Header.ShouldBe("First");
            cuts[1].Instance.Header.ShouldBe("Second");
        }

        [Fact(DisplayName = "Render events for non-rendered sub components are not emitted")]
        public void Test010()
        {
            var renderSub = new RenderEventSubscriber(Renderer.RenderEvents);
            var wrapper = RenderComponent<TwoComponentWrapper>(
                RenderFragment<Simple1>(nameof(TwoComponentWrapper.First)),
                RenderFragment<Simple1>(nameof(TwoComponentWrapper.Second))
            );
            var cuts = wrapper.FindComponents<Simple1>();
            var wrapperSub = new RenderEventSubscriber(wrapper.RenderEvents);
            var cutSub1 = new RenderEventSubscriber(cuts[0].RenderEvents);
            var cutSub2 = new RenderEventSubscriber(cuts[1].RenderEvents);

            renderSub.RenderCount.ShouldBe(1);

            cuts[0].Render();

            renderSub.RenderCount.ShouldBe(2);
            wrapperSub.RenderCount.ShouldBe(1);
            cutSub1.RenderCount.ShouldBe(1);
            cutSub2.RenderCount.ShouldBe(0);

            cuts[1].Render();

            renderSub.RenderCount.ShouldBe(3);
            wrapperSub.RenderCount.ShouldBe(2);
            cutSub1.RenderCount.ShouldBe(1);
            cutSub2.RenderCount.ShouldBe(1);
        }

        [Fact(DisplayName = "VerifyAsyncChanges can wait for multiple renders and changes to occur")]
        public void Test110()
        {
            // Initial state is stopped
            var cut = RenderComponent<TwoRendersTwoChanges>();
            var stateElement = cut.Find("#state");
            stateElement.TextContent.ShouldBe("Stopped");

            // Clicking 'tick' changes the state, and starts a task
            cut.Find("#tick").Click();
            cut.Find("#state").TextContent.ShouldBe("Started");

            // Clicking 'tock' completes the task, which updates the state
            // This click causes two renders, thus something is needed to await here.
            cut.Find("#tock").Click();
            cut.WaitForAssertion(
                () => cut.Find("#state").TextContent.ShouldBe("Stopped")
            );
        }

        [Fact(DisplayName = "VerifyAyncChanges throws verification exception after timeout")]
        public void Test011()
        {
            var cut = RenderComponent<Simple1>();

            Should.Throw<ShouldAssertException>(() =>
                cut.WaitForAssertion(() => cut.Markup.ShouldBeEmpty(), TimeSpan.FromMilliseconds(100))
            );
        }

        [Fact(DisplayName = "WaitForState throws TimeoutException exception after timeout")]
        public void Test012()
        {
            var cut = RenderComponent<Simple1>();

            Should.Throw<TimeoutException>(() =>
                cut.WaitForState(() => string.IsNullOrEmpty(cut.Markup), TimeSpan.FromMilliseconds(100))
            );
        }

        [Fact(DisplayName = "WaitForState can wait for multiple renders and changes to occur")]
        public void Test013()
        {
            // Initial state is stopped
            var cut = RenderComponent<TwoRendersTwoChanges>();
            var stateElement = cut.Find("#state");
            stateElement.TextContent.ShouldBe("Stopped");

            // Clicking 'tick' changes the state, and starts a task
            cut.Find("#tick").Click();
            cut.Find("#state").TextContent.ShouldBe("Started");

            // Clicking 'tock' completes the task, which updates the state
            // This click causes two renders, thus something is needed to await here.
            cut.Find("#tock").Click();
            cut.WaitForState(() => cut.Find("#state").TextContent == "Stopped");
            cut.Find("#state").TextContent.ShouldBe("Stopped");
        }

    }

}
