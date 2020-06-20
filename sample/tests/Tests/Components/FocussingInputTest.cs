﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.TestDoubles.JSInterop;
using SampleApp.Components;
using Xunit;
using Bunit;
using static Bunit.ComponentParameterFactory;

namespace SampleApp.CodeOnlyTests.Components
{
    public class FocussingInputTest : TestContext
    {
        [Fact(DisplayName = "After first render, the new input field has focus")]
        public void Test001()
        {
            // Arrange - add the IJSRuntime mock
            var jsRtMock = Services.AddMockJSRuntime();

            // Act - render the FocussingInput component, causing
            // the OnAfterRender(firstRender: true) to be called
            var cut = RenderComponent<FocussingInput>();

            // Assert
            // that there is a single call to document.body.focus.call
            var invocation = jsRtMock.VerifyInvoke("document.body.focus.call");

            // Assert that the invocation received a single argument
            // and that it was a reference to the input element.
            var expectedReferencedElement = cut.Find("input");
            invocation.Arguments.Single().ShouldBeElementReferenceTo(expectedReferencedElement);
        }
    }
}
