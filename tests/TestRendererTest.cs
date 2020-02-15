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
            var res = new RenderEventSubscriber(Renderer.RenderEvents);
            var sut = RenderComponent<TwoChildren>();
            
            res.RenderCount.ShouldBe(1);

            sut.Find("button").Click();
            
            res.RenderCount.ShouldBe(2);
        }

    }
}
