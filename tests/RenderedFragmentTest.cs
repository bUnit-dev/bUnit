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
using Xunit.Sdk;

namespace Bunit
{
    public class RenderedFragmentTest : ComponentTestFixture
    {
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
            var initialValue = cut.Nodes.Find("p").OuterHtml;

            WaitForNextRender(() => testData.SetResult("Steve Sanderson"));

            var steveValue = cut.Nodes.Find("p").OuterHtml;
            steveValue.ShouldNotBe(initialValue);
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

            WaitForNextRender(() => invocation.SetResult("Steve Sanderson"), TimeSpan.FromDays(1));

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


    }

}
