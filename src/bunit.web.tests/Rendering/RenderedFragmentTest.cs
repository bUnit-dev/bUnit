using Bunit.Extensions;
using Bunit.Rendering;
using Bunit.Rendering.RenderEvents;
using Bunit.SampleComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Bunit
{
	public class RenderedFragmentTest : ComponentTestFixture
    {
        public RenderedFragmentTest(ITestOutputHelper output)
        {
            Services.AddXunitLogger(output, minimumLogLevel: LogLevel.Debug);
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

        [Fact(DisplayName = "FindComponent returns first component of requested type using a depth first search")]
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
            var renderSub = new ConcurrentRenderEventSubscriber(RenderEvents);
            var wrapper = RenderComponent<TwoComponentWrapper>(
                RenderFragment<Simple1>(nameof(TwoComponentWrapper.First)),
                RenderFragment<Simple1>(nameof(TwoComponentWrapper.Second))
            );
            var cuts = wrapper.FindComponents<Simple1>();
            var wrapperSub = new ConcurrentRenderEventSubscriber(wrapper.RenderEvents);
            var cutSub1 = new ConcurrentRenderEventSubscriber(cuts[0].RenderEvents);
            var cutSub2 = new ConcurrentRenderEventSubscriber(cuts[1].RenderEvents);

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
    }
}
