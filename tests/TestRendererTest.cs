using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.SampleComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Shouldly;
using Xunit;

namespace Bunit
{
    public class TestRendererTest : ComponentTestFixture
    {
        [Fact(DisplayName = "Renderer pushes render events to subscribers when renders occur")]
        public void Test001()
        {
            var res = new ConcurrentRenderEventSubscriber(Renderer.RenderEvents);
            var sut = RenderComponent<TwoChildren>();
            
            res.RenderCount.ShouldBe(1);

            sut.Find("button").Click();
            
            res.RenderCount.ShouldBe(2);
        }

        [Fact(DisplayName = "Can pass reference to elements to JsInterop")]
        public void Test010()
        {
            //var cut = RenderComponent<ElementRefComponent>();
            //var inputElement = cut.Find("#capturedElement");
            //var buttonElement = cut.Find("button");

            //Assert.Equal(string.Empty, inputElement.GetAttribute("value"));

            //buttonElement.Click();
            //Assert.Equal("Clicks: 1", inputElement.GetAttribute("value"));
            //buttonElement.Click();
            //Assert.Equal("Clicks: 2", inputElement.GetAttribute("value"));
        }

        [Fact(DisplayName = "Can capture reference to dynamically added elements and pass to JsInterop")]
        public void Test011()
        {
            //var cut = RenderComponent<ElementRefComponent>();
            //var buttonElement = cut.Find("button");
            //var checkbox = cut.Find("input[type=checkbox]");

            //// We're going to remove the input. But first, put in some contents
            //// so we can observe it's not the same instance later
            //cut.Find("#capturedElement").SendKeys("some text");

            //// Remove the captured element
            //checkbox.Click();
            //Browser.Empty(() => cut.FindAll("#capturedElement"));

            //// Re-add it; observe it starts empty again
            //checkbox.Click();
            //var inputElement = cut.Find("#capturedElement");
            //Assert.Equal(string.Empty, inputElement.GetAttribute("value"));

            //// See that the capture variable was automatically updated to reference the new instance
            //buttonElement.Click();
            //Assert.Equal("Clicks: 1", () => inputElement.GetAttribute("value"));
        }

    }
}
