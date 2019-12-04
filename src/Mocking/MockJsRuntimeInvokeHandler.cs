using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Threading;

namespace Egil.RazorComponents.Testing
{
    public class MockJsRuntimeInvokeHandler
    {
        //private Dictionary<string, TaskCompletionSource>

        public bool StrictMode { get; set; }

        public IJSRuntime ToJsRuntime()
        {
            return new MockJsRuntime(this);
        }

        private class MockJsRuntime : IJSRuntime
        {
            private MockJsRuntimeInvokeHandler _handlers;

            public MockJsRuntime(MockJsRuntimeInvokeHandler mockJsRuntimeInvokeHandler)
            {
                _handlers = mockJsRuntimeInvokeHandler;
            }

            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
            {
                return new ValueTask<TValue>(default(TValue)!);
            }

            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
            {
                return new ValueTask<TValue>(default(TValue)!);
            }
        }
    }
}
