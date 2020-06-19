﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApp.Components;
using Bunit.TestDoubles.JSInterop;
using Shouldly;
using Xunit;
using Bunit;
using static Bunit.ComponentParameterFactory;

namespace SampleApp.CodeOnlyTests.Components
{
    public class WikiSearchTest : TestContext
    {
        [Fact(DisplayName = "WikiSearch renders an empty PRE element initially")]
        public void Test001()
        {
            // Arrange
            // Registered the MockJSRuntime in "Loose" mode with the service provider used when rendering components.
            // JSRuntimeMockMode.Loose is the default. It configures the mock to just return the default
            // value for whatever is requested in a InvokeAsync call if no call has explicitly been set up.
            var jsMock = Services.AddMockJSRuntime();

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
            // Registered the MockJSRuntime in "strict" mode with the service provider used when rendering components.
            // JSRuntimeMockMode.Strict mode configures the mock to throw an error if it receives an InvokeAsync call
            // it has not been set up to handle.
            var jsMock = Services.AddMockJSRuntime(JSRuntimeMockMode.Strict);

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
            plannedInvocation.SetResult(expectedSearchResult);

            // Assert
            // Verify that the result was received and correct placed in the paragraph element.
            cut.WaitForAssertion(() => cut.Find("pre").InnerHtml.Trim().ShouldBe(expectedSearchResult));
        }
    }
}
