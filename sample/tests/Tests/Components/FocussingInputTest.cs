using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.SampleApp.Components;
using Xunit;

namespace Egil.RazorComponents.Testing.SampleApp.CodeOnlyTests.Components
{
    public class FocussingInputTest : ComponentTestFixture
    {
        [Fact(DisplayName = "After first render, the new input field has focus")]
        public void Test001()
        {
            // Arrange - add the IJsRuntime mock
            var jsRtMock = Services.AddMockJsRuntime();

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
