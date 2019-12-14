using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.SampleApp.Components;
using Shouldly;
using Xunit;

namespace Egil.RazorComponents.Testing.SampleApp.CodeOnlyTests.Components
{
    public class PassingChildContentTest : ComponentTestFixture
    {
        [Fact(DisplayName = "Markup passed to child content is rendered as expected")]
        public void Test001()
        {
            var markup = "<h1>HELLO WORLD</h1>";

            var cut = RenderComponent<ThemedButton>(ChildContent(markup));

            cut.Find("button").ChildNodes.ShouldBe(markup);
        }

        [Fact(DisplayName = "A component render fragment passed to child content is rendered as expected")]
        public void Test002()
        {
            // arrange
            var expectedChildContent = RenderComponent<ThemedButton>(("id", "test"));

            // act
            var cut = RenderComponent<ThemedElement>(
                // creates child content parameter that renders a 
                // fragment that creates a ThemedButton component with the provided params
                ChildContent<ThemedButton>(("id", "test")) 
            );

            // assert by going through ThemedElement's root DIV
            cut.Find("div").ChildNodes.ShouldBe(expectedChildContent);
            // assert by finding the child content by its ID tag and comparing to the expected
            cut.Find("#test").ShouldBe(expectedChildContent);
        }
    }
}
