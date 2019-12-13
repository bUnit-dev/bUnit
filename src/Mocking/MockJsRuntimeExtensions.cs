using System;

namespace Egil.RazorComponents.Testing
{
    public static class MockJsRuntimeExtensions
    {
        public static MockJsRuntimeInvokeHandler AddMockJsRuntime(this TestServiceProvider serviceProvider, JsRuntimeMockMode mode = JsRuntimeMockMode.Loose)
        {
            if (serviceProvider is null) throw new ArgumentNullException(nameof(serviceProvider));

            var result = new MockJsRuntimeInvokeHandler(mode);

            serviceProvider.AddService(result.ToJsRuntime());

            return result;
        }
    }
}
