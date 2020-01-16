using Egil.RazorComponents.Testing.Extensions;
using Egil.RazorComponents.Testing.SampleComponents;
using Egil.RazorComponents.Testing.SampleComponents.Data;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Egil.RazorComponents.Testing
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
            Services.AddService<IAsyncTestDep>(testData);
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
    }

}
