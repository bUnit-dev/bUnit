using System;
using Microsoft.JSInterop;

namespace Egil.RazorComponents.Testing
{
    public static class MockJsRuntimeExtensions
    {
        public static MockJsRuntimeInvokeHandler AddMockJsRuntime(this TestHost host, bool strictMode = false)
        {
            if (host is null) throw new ArgumentNullException(nameof(host));

            var result = new MockJsRuntimeInvokeHandler() { StrictMode = strictMode };

            host.AddService(result.ToJsRuntime());

            return result;
        }
    }
}
