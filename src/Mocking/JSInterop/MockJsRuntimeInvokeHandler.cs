using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Threading;
using System.Collections.Generic;
using Xunit;
using Egil.RazorComponents.Testing.Mocking.JsInterop;

namespace Egil.RazorComponents.Testing.Mocking.JSInterop
{
    public class MockJsRuntimeInvokeHandler
    {
        private Dictionary<string, List<JsRuntimeInvocation>> _invocations = new Dictionary<string, List<JsRuntimeInvocation>>();

        public IReadOnlyDictionary<string, List<JsRuntimeInvocation>> Invocations => _invocations;

        public JsRuntimeMockMode Mode { get; }

        public MockJsRuntimeInvokeHandler(JsRuntimeMockMode mode = JsRuntimeMockMode.Loose)
        {
            Mode = mode;
        }

        public IJSRuntime ToJsRuntime()
        {
            return new MockJsRuntime(this);
        }

        private void AddInvocation(JsRuntimeInvocation invocation)
        {
            if (!_invocations.ContainsKey(invocation.Identifier))
            {
                _invocations.Add(invocation.Identifier, new List<JsRuntimeInvocation>());
            }
            _invocations[invocation.Identifier].Add(invocation);
        }

        private class MockJsRuntime : IJSRuntime
        {
            private readonly MockJsRuntimeInvokeHandler _handlers;

            public MockJsRuntime(MockJsRuntimeInvokeHandler mockJsRuntimeInvokeHandler)
            {
                _handlers = mockJsRuntimeInvokeHandler;
            }

            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
            {
                _handlers.AddInvocation(new JsRuntimeInvocation(identifier, null, args));
                return new ValueTask<TValue>(default(TValue)!);
            }

            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
            {
                _handlers.AddInvocation(new JsRuntimeInvocation(identifier, null, args));
                return new ValueTask<TValue>(default(TValue)!);
            }
        }
    }
}
