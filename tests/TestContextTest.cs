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
    public class TestContextTest : ComponentTestFixture
    {
        [Fact]
        public void MyTestMethod()
        {
            using var res = new RenderEventSubscriber(Renderer.RenderEvents);
            var sut = RenderComponent<TwoChildren>();
            sut.Find("button").Click();
            res.RenderCount.ShouldBe(2);
        }

    }
}
