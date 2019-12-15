using System.Collections.Generic;
using System;
using System.Threading;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Represents an invocation of JavaScript via the JsRuntime Mock
    /// </summary>
    public readonly struct JsRuntimeInvocation : IEquatable<JsRuntimeInvocation>
    {
        /// <summary>
        /// Gets the identifier used in the invocation.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Gets the cancellation token used in the invocation.
        /// </summary>
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// Gets the arguments used in the invocation.
        /// </summary>
        public IReadOnlyList<object> Arguments { get; }

        /// <summary>
        /// Creates an instance of the <see cref="JsRuntimeInvocation"/>.
        /// </summary>
        public JsRuntimeInvocation(string identifier, CancellationToken? cancellationToken, object[] args)
        {
            Identifier = identifier;
            CancellationToken = cancellationToken ?? CancellationToken.None;
            Arguments = args;
        }

        /// <inheritdoc/>
        public bool Equals(JsRuntimeInvocation other) => Identifier.Equals(other.Identifier, StringComparison.Ordinal) && CancellationToken == other.CancellationToken && Arguments == other.Arguments;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is JsRuntimeInvocation other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => (Identifier, CancellationToken, Arguments).GetHashCode();

        /// <inheritdoc/>
        public static bool operator ==(JsRuntimeInvocation left, JsRuntimeInvocation right) => left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(JsRuntimeInvocation left, JsRuntimeInvocation right) => !(left == right);
    }
}
