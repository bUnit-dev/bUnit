using System.Collections.Generic;
using System.Threading;

namespace Egil.RazorComponents.Testing
{
    public readonly struct JsRuntimeInvocation
    {
        public string Identifier { get; }
        
        public CancellationToken? CancellationToken { get; }

        public IReadOnlyList<object> Arguments { get; }

        public JsRuntimeInvocation(string identifier, CancellationToken? cancellationToken, object[] args)
        {
            Identifier = identifier;
            CancellationToken = cancellationToken;
            Arguments = args;
        }
    }
}
