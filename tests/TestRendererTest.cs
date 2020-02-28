using System;
using System.Linq;
using Bunit.BlazorE2E.BasicTestApp;
using Bunit.Extensions;
using Bunit.Mocking.JSInterop;
using Bunit.SampleComponents;
using Microsoft.AspNetCore.Components;
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
    }
}
