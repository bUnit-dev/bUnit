using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.EventDispatchExtensions;
using Egil.RazorComponents.Testing.Library.SampleApp.Pages;
using Shouldly;
using Xunit;

namespace Egil.RazorComponents.Testing.Library.SampleApp.CodeOnlyTests
{
    public class CounterTest : ComponentTestFixture
    {
        [Fact]
        public void InitialHtmlIsCorrect()
        {
            var cut = RenderComponent<Counter>();
            var expectedHtml = @"<h1>Counter</h1>
                                 <p>Current count: 0</p>
                                 <button class=""btn btn-primary"">Click me</button>";

            cut.ShouldBe(expectedHtml);
        }

        [Fact]
        public void CounterStagesTest()
        {
            // Initial rendering
            var cut = RenderComponent<Counter>();
            var expectedHtml = @"<h1>Counter</h1>
                                 <p>Current count: 0</p>
                                 <button class=""btn btn-primary"">Click me</button>";
            cut.ShouldBe(expectedHtml);

            // After first click
            cut.Find("button").Click();
            cut.GetChangesSinceFirstRender().ShouldHaveSingleTextChange("Current count: 1");

            // After second click
            cut.Find("button").Click();
            cut.GetChangesSinceFirstRender().ShouldHaveSingleTextChange("Current count: 2");
        }
    }
}
