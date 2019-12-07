using System.Threading;

namespace Egil.RazorComponents.Testing.Mocking.JSInterop
{
    public class JsRuntimeInvocation
    {
        public string Identifier { get; }
        public CancellationToken? CancellationToken { get; }
#pragma warning disable CA1819 // Properties should not return arrays
        public object[] Arguments { get; }
#pragma warning restore CA1819 // Properties should not return arrays

        public JsRuntimeInvocation(string identifier, CancellationToken? cancellationToken, object[] args)
        {
            Identifier = identifier;
            CancellationToken = cancellationToken;
            Arguments = args;
        }
    }
}
