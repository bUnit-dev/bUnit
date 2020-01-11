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
        [Fact(DisplayName = "The test service provider should register a default IJSRuntime " +
             "which throws exceptions")]
        public void Test001()
        {
            var ex = Assert.Throws<AggregateException>(() => RenderComponent<SimpleWithJsRuntimeDep>());
            Assert.Equal(DefaultJsRuntime.MissingJsRuntimeMessage, ex.InnerException?.Message);
            Assert.Equal(DefaultJsRuntime.MissingJsRuntimeHelpLink, ex.InnerException?.HelpLink);
        }
    }
}
