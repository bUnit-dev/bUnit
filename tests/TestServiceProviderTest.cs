using Egil.RazorComponents.Testing.EventDispatchExtensions;
using Egil.RazorComponents.Testing.Extensions;
using Egil.RazorComponents.Testing.Mocking.JSInterop;
using Egil.RazorComponents.Testing.SampleComponents;
using Egil.RazorComponents.Testing.SampleComponents.Data;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    public class TestServiceProviderTest : ComponentTestFixture
    {
        [Fact(DisplayName = "The test service provider should register a placeholder IJSRuntime " +
             "which throws exceptions")]
        public void Test001()
        {
            var ex = Assert.Throws<AggregateException>(() => RenderComponent<SimpleWithJsRuntimeDep>());
            Assert.True(ex?.InnerException is MissingMockJsRuntimeException);
        }

        [Fact(DisplayName = "The placeholder IJSRuntime is overriden by a supplied mock and does not throw")]
        public void Test002()
        {
            Services.AddMockJsRuntime();

            RenderComponent<SimpleWithJsRuntimeDep>();
        }
    }
}
