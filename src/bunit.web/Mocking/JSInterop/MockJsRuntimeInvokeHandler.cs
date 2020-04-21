using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Bunit.Mocking.JSInterop
{
	/// <summary>
	/// Represents an invoke handler for a mock of a <see cref="IJSRuntime"/>.
	/// </summary>
	public class MockJsRuntimeInvokeHandler
    {
        private readonly Dictionary<string, List<JsRuntimeInvocation>> _invocations = new Dictionary<string, List<JsRuntimeInvocation>>();
        private readonly Dictionary<string, List<object>> _plannedInvocations = new Dictionary<string, List<object>>();

        /// <summary>
        /// Gets a dictionary of all <see cref="List{JsRuntimeInvocation}"/> this mock has observed.
        /// </summary>
        public IReadOnlyDictionary<string, List<JsRuntimeInvocation>> Invocations => _invocations;

        /// <summary>
        /// Gets whether the mock is running in <see cref="JsRuntimeMockMode.Loose"/> or 
        /// <see cref="JsRuntimeMockMode.Strict"/>.
        /// </summary>
        public JsRuntimeMockMode Mode { get; }

        /// <summary>
        /// Creates a <see cref="MockJsRuntimeInvokeHandler"/>.
        /// </summary>
        /// <param name="mode">The <see cref="JsRuntimeMockMode"/> the handler should use.</param>
        public MockJsRuntimeInvokeHandler(JsRuntimeMockMode mode = JsRuntimeMockMode.Loose)
        {
            Mode = mode;
        }

        /// <summary>
        /// Gets the mocked <see cref="IJSRuntime"/> instance.
        /// </summary>
        /// <returns></returns>
        public IJSRuntime ToJsRuntime()
        {
            return new MockJsRuntime(this);
        }

        /// <summary>
        /// Configure a planned JSInterop invocation with the <paramref name="identifier"/> and arguments
        /// passing the <paramref name="argumentsMatcher"/> test.
        /// </summary>
        /// <typeparam name="TResult">The result type of the invocation</typeparam>
        /// <param name="identifier">The identifier to setup a response for</param>
        /// <param name="argumentsMatcher">A matcher that is passed arguments received in invocations to <paramref name="identifier"/>. If it returns true the invocation is matched.</param>
        /// <returns>A <see cref="JsRuntimePlannedInvocation{TResult}"/>.</returns>
        public JsRuntimePlannedInvocation<TResult> Setup<TResult>(string identifier, Func<IReadOnlyList<object>, bool> argumentsMatcher)
        {
            var result = new JsRuntimePlannedInvocation<TResult>(identifier, argumentsMatcher);

            AddPlannedInvocation(result);

            return result;
        }

        /// <summary>
        /// Configure a planned JSInterop invocation with the <paramref name="identifier"/> and <paramref name="arguments"/>.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="identifier">The identifier to setup a response for</param>
        /// <param name="arguments">The arguments that an invocation to <paramref name="identifier"/> should match.</param>
        /// <returns>A <see cref="JsRuntimePlannedInvocation{TResult}"/>.</returns>
        public JsRuntimePlannedInvocation<TResult> Setup<TResult>(string identifier, params object[] arguments)
        {
            return Setup<TResult>(identifier, args => Enumerable.SequenceEqual(args, arguments));
        }

        /// <summary>
        /// Configure a planned JSInterop invocation with the <paramref name="identifier"/> and arguments
        /// passing the <paramref name="argumentsMatcher"/> test, that should not receive any result.
        /// </summary>
        /// <param name="identifier">The identifier to setup a response for</param>
        /// <param name="argumentsMatcher">A matcher that is passed arguments received in invocations to <paramref name="identifier"/>. If it returns true the invocation is matched.</param>
        /// <returns>A <see cref="JsRuntimePlannedInvocation"/>.</returns>
        public JsRuntimePlannedInvocation SetupVoid(string identifier, Func<IReadOnlyList<object>, bool> argumentsMatcher)
        {
            var result = new JsRuntimePlannedInvocation(identifier, argumentsMatcher);

            AddPlannedInvocation(result);

            return result;
        }

        /// <summary>
        /// Configure a planned JSInterop invocation with the <paramref name="identifier"/> 
        /// and <paramref name="arguments"/>, that should not receive any result.
        /// </summary>
        /// <param name="identifier">The identifier to setup a response for</param>
        /// <param name="arguments">The arguments that an invocation to <paramref name="identifier"/> should match.</param>
        /// <returns>A <see cref="JsRuntimePlannedInvocation"/>.</returns>
        public JsRuntimePlannedInvocation SetupVoid(string identifier, params object[] arguments)
        {
            return SetupVoid(identifier, args => Enumerable.SequenceEqual(args, arguments));
        }

        private void AddPlannedInvocation<TResult>(JsRuntimePlannedInvocationBase<TResult> planned)
        {
            if (!_plannedInvocations.ContainsKey(planned.Identifier))
            {
                _plannedInvocations.Add(planned.Identifier, new List<object>());
            }
            _plannedInvocations[planned.Identifier].Add(planned);
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
                => InvokeAsync<TValue>(identifier, default, args);

            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
            {
                var invocation = new JsRuntimeInvocation(identifier, cancellationToken, args);
                _handlers.AddInvocation(invocation);

                return TryHandlePlannedInvocation<TValue>(identifier, invocation)
                    ?? new ValueTask<TValue>(default(TValue)!);
            }

            private ValueTask<TValue>? TryHandlePlannedInvocation<TValue>(string identifier, JsRuntimeInvocation invocation)
            {
                ValueTask<TValue>? result = default;

                if (_handlers._plannedInvocations.TryGetValue(identifier, out var plannedInvocations))
                {
                    var planned = plannedInvocations.OfType<JsRuntimePlannedInvocationBase<TValue>>()
                        .SingleOrDefault(x => x.Matches(invocation));

                    if (planned is { })
                    {
                        var task = planned.RegisterInvocation(invocation);
                        result = new ValueTask<TValue>(task);
                    }
                }

                if (result is null && _handlers.Mode == JsRuntimeMockMode.Strict)
                {
                    throw new UnplannedJsInvocationException(invocation);
                }

                return result;
            }
        }
    }
}
