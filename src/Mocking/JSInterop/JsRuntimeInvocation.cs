﻿using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Bunit.Mocking.JSInterop
{
    /// <summary>
    /// Represents an invocation of JavaScript via the JsRuntime Mock
    /// </summary>
    [SuppressMessage("Design", "CA1068:CancellationToken parameters must come last", Justification = "<Pending>")]
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
        public JsRuntimeInvocation(string identifier, CancellationToken cancellationToken, object[] args)
        {
            Identifier = identifier;
            CancellationToken = cancellationToken;
            Arguments = args;
        }

        /// <inheritdoc/>
        public bool Equals(JsRuntimeInvocation other)
            => Identifier.Equals(other.Identifier, StringComparison.Ordinal)
            && CancellationToken == other.CancellationToken
            && ArgumentsEqual(Arguments, other.Arguments);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is JsRuntimeInvocation other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Identifier);
            hash.Add(CancellationToken);

            for (int i = 0; i < Arguments.Count; i++)
            {
                hash.Add(Arguments[i]);
            }

            return hash.ToHashCode();
        }

        /// <inheritdoc/>
        public static bool operator ==(JsRuntimeInvocation left, JsRuntimeInvocation right) => left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(JsRuntimeInvocation left, JsRuntimeInvocation right) => !(left == right);

        private static bool ArgumentsEqual(IReadOnlyList<object> left, IReadOnlyList<object> right)
        {
            if (left.Count != right.Count) return false;

            for (int i = 0; i < left.Count; i++)
            {
                if (!left[i].Equals(right[i])) return false;
            }

            return true;
        }
    }
}
