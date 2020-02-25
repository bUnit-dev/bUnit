using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.SampleApp.Components;
using Bunit.Mocking.JSInterop;
using Shouldly;
using Xunit;

namespace Bunit.SampleApp.CodeOnlyTests.Components
{
    public class WikiSearchTest : ComponentTestFixture
    {
        [Fact(DisplayName = "WikiSearch renders an empty PRE element initially")]
        public void Test001()
        {
            // Arrange
            // Registered the MockJsRuntime in "Loose" mode with the service provider used when rendering components.
            // JsRuntimeMockMode.Loose is the default. It configures the mock to just return the default
            // value for whatever is requested in a InvokeAsync call if no call has explicitly been set up.
            var jsMock = Services.AddMockJsRuntime();

            // Act - render the WikiSearch component
            var cut = RenderComponent<WikiSearch>();

            // Assert
            // Check that the components initial HTML is as expected
            // and that the mock was called with the expected JS identifier and arguments.
            cut.MarkupMatches(@"<pre class:ignore></pre>");
            jsMock.VerifyInvoke("queryWiki").Arguments.Single().ShouldBe("blazor");
        }

        [Fact(DisplayName = "On first render WikiSearch uses JSInterop to query wiki and display the result")]
        public void Test002()
        {
            // Arrange
            // Registered the MockJsRuntime in "strict" mode with the service provider used when rendering components.
            // JsRuntimeMockMode.Strict mode configures the mock to throw an error if it receives an InvokeAsync call
            // it has not been set up to handle.
            var jsMock = Services.AddMockJsRuntime(JsRuntimeMockMode.Strict);

            // Set up the mock to handle the expected call
            var expectedSearchResult = "SEARCH RESULT";
            var plannedInvocation = jsMock.Setup<string>("queryWiki", "blazor");

            // Render the WikiSearch and verify that there is no content in the paragraph element
            var cut = RenderComponent<WikiSearch>();
            cut.Find("pre").InnerHtml.Trim().ShouldBeEmpty();

            // Act
            // Use the WaitForNextRender to block until the component has finished re-rendered.
            // The plannedInvocation.SetResult will return the result to the component is waiting 
            // for in its OnAfterRender from the await jsRuntime.InvokeAsync<string>("queryWiki", "blazor") call.
            WaitForNextRender(() => plannedInvocation.SetResult(expectedSearchResult));

            // Assert
            // Verify that the result was received and correct placed in the paragraph element.
            cut.Find("pre").InnerHtml.Trim().ShouldBe(expectedSearchResult);
        }
    }
}
