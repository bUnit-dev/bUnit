using Egil.RazorComponents.Testing.EventDispatchExtensions;
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

        [Fact(DisplayName = "GetNodes should return new instance when " +
                            "async operation during OnInit causes component to re-render")]
        public void Test003()
        {
            var testData = new AsyncNameDep();
            Services.AddService<IAsyncTestDep>(testData);
            var cut = RenderComponent<SimpleWithAyncDeps>();
            var initialValue = cut.GetNodes().Find("p").OuterHtml;

            WaitForNextRender(() => testData.SetResult("Steve Sanderson"));

            var steveValue = cut.GetNodes().Find("p").OuterHtml;
            steveValue.ShouldNotBe(initialValue);
        }

        [Fact(DisplayName = "GetNodes should return new instance when " +
                    "async operation/StateHasChanged during OnAfterRender causes component to re-render")]
        public void Test004()
        {
            var invocation = Services.AddMockJsRuntime().Setup<string>("getdata");
            var cut = RenderComponent<SimpleWithJsRuntimeDep>();
            var initialValue = cut.GetNodes().Find("p").OuterHtml;

            WaitForNextRender(() => invocation.SetResult("Steve Sanderson"));

            var steveValue = cut.GetNodes().Find("p").OuterHtml;
            steveValue.ShouldNotBe(initialValue);
        }

        [Fact(DisplayName = "GetNodes on a components with child component returns " +
                            "new instance when the child component has changes")]
        public void Test005()
        {
            var invocation = Services.AddMockJsRuntime().Setup<string>("getdata");
            var notcut = RenderComponent<Wrapper>(ChildContent<Simple1>());
            var cut = RenderComponent<Wrapper>(ChildContent<SimpleWithJsRuntimeDep>());
            var initialValue = cut.GetNodes();

            WaitForNextRender(() => invocation.SetResult("Steve Sanderson"), TimeSpan.FromDays(1));

            Assert.NotSame(initialValue, cut.GetNodes());
        }

    }

    public class RenderComponentTest : ComponentTestFixture
    {
        [Fact(DisplayName = "GetNodes should return the same instance " +
                            "when a render has not resulted in any changes")]
        public void Test003()
        {
            var cut = RenderComponent<Wrapper>(ChildContent("<div>"));
            var initialNodes = cut.GetNodes();

            cut.Render();
            cut.SetParametersAndRender(ChildContent("<div>"));

            Assert.Same(initialNodes, cut.GetNodes());
        }

        [Fact(DisplayName = "GetNodes should return new instance " +
                            "when a SetParametersAndRender has caused changes to DOM tree")]
        public void Tets004()
        {
            var cut = RenderComponent<Wrapper>(ChildContent("<div>"));
            var initialNodes = cut.GetNodes();

            cut.SetParametersAndRender(ChildContent("<p>"));

            Assert.NotSame(initialNodes, cut.GetNodes());
            cut.Find("p").ShouldNotBeNull();
        }

        [Fact(DisplayName = "GetNodes should return new instance " +
                            "when a Render has caused changes to DOM tree")]
        public void Tets005()
        {
            var cut = RenderComponent<RenderCounter>();
            var initialNodes = cut.GetNodes();

            cut.Render();

            Assert.NotSame(initialNodes, cut.GetNodes());
        }

        [Fact(DisplayName = "GetNodes should return new instance " +
                            "when a event handler trigger has caused changes to DOM tree")]
        public void Tets006()
        {
            var cut = RenderComponent<ClickCounter>();
            var initialNodes = cut.GetNodes();

            cut.Find("button").Click();

            Assert.NotSame(initialNodes, cut.GetNodes());
        }


    }

}
