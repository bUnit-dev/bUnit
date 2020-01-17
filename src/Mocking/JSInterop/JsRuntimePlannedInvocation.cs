using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Egil.RazorComponents.Testing.Mocking.JSInterop
{
    /// <summary>
    /// Represents a planned invocation of a JavaScript function which returns nothing, with specific arguments.
    /// </summary>
    public class JsRuntimePlannedInvocation : JsRuntimePlannedInvocationBase<object>
    {
        internal JsRuntimePlannedInvocation(string identifier, Func<IReadOnlyList<object>, bool> matcher) : base(identifier, matcher)
        {
        }

        /// <summary>
        /// Completes the current awaiting void invocation requests.
        /// </summary>
        public void SetVoidResult()
        {
            base.SetResultBase(default!);
        }
    }

    /// <summary>
    /// Represents a planned invocation of a JavaScript function with specific arguments.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class JsRuntimePlannedInvocation<TResult> : JsRuntimePlannedInvocationBase<TResult>
    {
        internal JsRuntimePlannedInvocation(string identifier, Func<IReadOnlyList<object>, bool> matcher) : base(identifier, matcher)
        {
        }

        /// <summary>
        /// Sets the <typeparamref name="TResult"/> result that invocations will receive.
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(TResult result)
        {
            base.SetResultBase(result);
        }
    }

    /// <summary>
    /// Represents a planned invocation of a JavaScript function with specific arguments.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public abstract class JsRuntimePlannedInvocationBase<TResult>
    {
        private readonly List<JsRuntimeInvocation> _invocations;

        private Func<IReadOnlyList<object>, bool> InvocationMatcher { get; }

        private TaskCompletionSource<TResult> _completionSource;

        /// <summary>
        /// The expected identifier for the function to invoke.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Gets the invocations that this <see cref="JsRuntimePlannedInvocation{TResult}"/> has matched with.
        /// </summary>
        public IReadOnlyList<JsRuntimeInvocation> Invocations => _invocations.AsReadOnly();

        /// <summary>
        /// Creates an instance of a <see cref="JsRuntimePlannedInvocationBase{TResult}"/>.
        /// </summary>
        protected JsRuntimePlannedInvocationBase(string identifier, Func<IReadOnlyList<object>, bool> matcher)
        {
            Identifier = identifier;
            _invocations = new List<JsRuntimeInvocation>();
            InvocationMatcher = matcher;
            _completionSource = new TaskCompletionSource<TResult>();
        }

        /// <summary>
        /// Sets the <typeparamref name="TResult"/> result that invocations will receive.
        /// </summary>
        /// <param name="result"></param>
        protected void SetResultBase(TResult result)
        {
            _completionSource.SetResult(result);
        }

        /// <summary>
        /// Sets the <typeparamref name="TException"/> exception that invocations will receive.
        /// </summary>
        /// <param name="exception"></param>
        public void SetException<TException>(TException exception)
            where TException : Exception
        {
            _completionSource.SetException(exception);
        }

        /// <summary>
        /// Marks the <see cref="Task{TResult}"/> that invocations will receive as canceled.
        /// </summary>
        public void SetCanceled()
        {
            _completionSource.SetCanceled();
        }

        internal Task<TResult> RegisterInvocation(JsRuntimeInvocation invocation)
        {
            if (_completionSource.Task.IsCompleted)
                _completionSource = new TaskCompletionSource<TResult>();

            _invocations.Add(invocation);

            return _completionSource.Task;
        }

        internal bool Matches(JsRuntimeInvocation invocation)
        {
            return Identifier.Equals(invocation.Identifier, StringComparison.Ordinal)
                && InvocationMatcher(invocation.Arguments);
        }
    }
}
