using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Egil.RazorComponents.Testing.Mocking.JSInterop
{
    /// <summary>
    /// Represents a planned invocation of a JavaScript function with specific arguments.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    [SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
    public readonly struct JsRuntimePlannedInvocation<TResult>
    {
        private readonly List<JsRuntimeInvocation> _invocations;
        private Func<IReadOnlyList<object>, bool> InvocationMatcher { get; }
        internal TaskCompletionSource<TResult> CompletionSource { get; }

        /// <summary>
        /// The expected identifier for the function to invoke.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Gets the invocations that this <see cref="JsRuntimePlannedInvocation{TResult}"/> has matched with.
        /// </summary>
        public IReadOnlyList<JsRuntimeInvocation> Invocations => _invocations.AsReadOnly();

        internal JsRuntimePlannedInvocation(string identifier, Func<IReadOnlyList<object>, bool> matcher)
        {
            Identifier = identifier;
            _invocations = new List<JsRuntimeInvocation>();
            InvocationMatcher = matcher;
            CompletionSource = new TaskCompletionSource<TResult>();
        }

        /// <summary>
        /// Sets the <typeparamref name="TResult"/> result that invocations will receive.
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(TResult result) => CompletionSource.SetResult(result);

        /// <summary>
        /// Sets the <typeparamref name="TException"/> exception that invocations will receive.
        /// </summary>
        /// <param name="exception"></param>
        public void SetException<TException>(TException exception) 
            where TException : Exception
            => CompletionSource.SetException(exception);

        /// <summary>
        /// Marks the <see cref="Task{TResult}"/> that invocations will receive as canceled.
        /// </summary>
        public void SetCanceled() => CompletionSource.SetCanceled();

        internal void AddInvocation(JsRuntimeInvocation invocation)
        {
            _invocations.Add(invocation);
        }

        internal bool Matches(JsRuntimeInvocation invocation)
        {
            return Identifier.Equals(invocation.Identifier, StringComparison.Ordinal)
                && InvocationMatcher(invocation.Arguments);
        }
    }
}
