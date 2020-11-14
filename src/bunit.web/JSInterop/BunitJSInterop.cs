using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Bunit
{
	/// <summary>
	/// Represents an bUnit's implementation of Blazor's JSInterop.
	/// </summary>
	public class BunitJSInterop
	{
		private readonly Dictionary<string, List<JSRuntimeInvocation>> _invocations = new();
		private readonly Dictionary<string, List<object>> _handlers = new();

		/// <summary>
		/// Gets a dictionary of all <see cref="List{JSRuntimeInvocation}"/> this mock has observed.
		/// </summary>
		public IReadOnlyDictionary<string, List<JSRuntimeInvocation>> Invocations => _invocations;

		/// <summary>
		/// Gets or sets whether the mock is running in <see cref="JSRuntimeMode.Loose"/> or
		/// <see cref="JSRuntimeMode.Strict"/>.
		/// </summary>
		public JSRuntimeMode Mode { get; set; }

		/// <summary>
		/// Gets the mocked <see cref="IJSRuntime"/> instance.
		/// </summary>
		/// <returns></returns>
		public IJSRuntime JSRuntime { get; }

		/// <summary>
		/// Creates a <see cref="BunitJSInterop"/>.
		/// </summary>
		public BunitJSInterop()
		{
			Mode = JSRuntimeMode.Strict;
			JSRuntime = new BUnitJSRuntime(this);
		}

		/// <summary>
		/// Configure a catch all JSInterop invocation handler for a specific return type.
		/// This will match only on the <typeparamref name="TResult"/>, and any arguments passed to
		/// <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/>.
		/// </summary>
		/// <typeparam name="TResult">The result type of the invocation.</typeparam>
		/// <returns>A <see cref="JSRuntimeInvocationHandler{TResult}"/>.</returns>
		public JSRuntimeInvocationHandler<TResult> Setup<TResult>()
		{
			var result = new JSRuntimeInvocationHandler<TResult>(JSRuntimeInvocationHandler<object>.CatchAllIdentifier, _ => true);

			AddHandler(result);

			return result;
		}

		/// <summary>
		/// Configure a JSInterop invocation handler with the <paramref name="identifier"/> and arguments
		/// passing the <paramref name="argumentsMatcher"/> test.
		/// </summary>
		/// <typeparam name="TResult">The result type of the invocation.</typeparam>
		/// <param name="identifier">The identifier to setup a response for.</param>
		/// <param name="argumentsMatcher">A matcher that is passed arguments received in invocations to <paramref name="identifier"/>. If it returns true the invocation is matched.</param>
		/// <returns>A <see cref="JSRuntimeInvocationHandler{TResult}"/>.</returns>
		public JSRuntimeInvocationHandler<TResult> Setup<TResult>(string identifier, Func<IReadOnlyList<object?>, bool> argumentsMatcher)
		{
			var result = new JSRuntimeInvocationHandler<TResult>(identifier, argumentsMatcher);

			AddHandler(result);

			return result;
		}

		/// <summary>
		/// Configure a JSInterop invocation handler with the <paramref name="identifier"/> and <paramref name="arguments"/>.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="identifier">The identifier to setup a response for.</param>
		/// <param name="arguments">The arguments that an invocation to <paramref name="identifier"/> should match.</param>
		/// <returns>A <see cref="JSRuntimeInvocationHandler{TResult}"/>.</returns>
		public JSRuntimeInvocationHandler<TResult> Setup<TResult>(string identifier, params object[] arguments)
		{
			return Setup<TResult>(identifier, args => args.SequenceEqual(arguments));
		}

		/// <summary>
		/// Configure a JSInterop invocation handler with the <paramref name="identifier"/> and arguments
		/// passing the <paramref name="argumentsMatcher"/> test, that should not receive any result.
		/// </summary>
		/// <param name="identifier">The identifier to setup a response for.</param>
		/// <param name="argumentsMatcher">A matcher that is passed arguments received in invocations to <paramref name="identifier"/>. If it returns true the invocation is matched.</param>
		/// <returns>A <see cref="JSRuntimeInvocationHandler"/>.</returns>
		public JSRuntimeInvocationHandler SetupVoid(string identifier, Func<IReadOnlyList<object?>, bool> argumentsMatcher)
		{
			var result = new JSRuntimeInvocationHandler(identifier, argumentsMatcher);

			AddHandler(result);

			return result;
		}

		/// <summary>
		/// Configure a JSInterop invocation handler with the <paramref name="identifier"/>
		/// and <paramref name="arguments"/>, that should not receive any result.
		/// </summary>
		/// <param name="identifier">The identifier to setup a response for.</param>
		/// <param name="arguments">The arguments that an invocation to <paramref name="identifier"/> should match.</param>
		/// <returns>A <see cref="JSRuntimeInvocationHandler"/>.</returns>
		public JSRuntimeInvocationHandler SetupVoid(string identifier, params object[] arguments)
		{
			return SetupVoid(identifier, args => args.SequenceEqual(arguments));
		}

		/// <summary>
		/// Configure a catch all JSInterop invocation handler, that should not receive any result.
		/// </summary>
		/// <returns>A <see cref="JSRuntimeInvocationHandler"/>.</returns>
		public JSRuntimeInvocationHandler SetupVoid()
		{
			var result = new JSRuntimeInvocationHandler(JSRuntimeInvocationHandler<object>.CatchAllIdentifier, _ => true);

			AddHandler(result);

			return result;
		}

		/// <summary>
		/// Looks through the registered handlers and returns the latest registered that can handle
		/// the provided <paramref name="identifier"/> and <paramref name="args"/>, and that
		/// will return <typeparamref name="TResult"/>.
		/// </summary>
		/// <returns>Returns the <see cref="JSRuntimeInvocationHandler{TResult}"/> or null if no one is found.</returns>
		public JSRuntimeInvocationHandler<TResult>? TryGetInvokeHandler<TResult>(string identifier, object?[]? args = null)
			=> TryGetHandlerFor<TResult>(new JSRuntimeInvocation(identifier, default, args));

		/// <summary>
		/// Looks through the registered handlers and returns the latest registered that can handle
		/// the provided <paramref name="identifier"/> and <paramref name="args"/>, and that returns a "void" result.
		/// </summary>
		/// <returns>Returns the <see cref="JSRuntimeInvocationHandler"/> or null if no one is found.</returns>
		public JSRuntimeInvocationHandler? TryGetInvokeVoidHandler(string identifier, object?[]? args = null)
			=> TryGetHandlerFor<object>(new JSRuntimeInvocation(identifier, default, args), x => x.IsVoidResultHandler) as JSRuntimeInvocationHandler;

		private void AddHandler<TResult>(JSRuntimeInvocationHandler<TResult> handler)
		{
			if (!_handlers.ContainsKey(handler.Identifier))
			{
				_handlers.Add(handler.Identifier, new List<object>());
			}
			_handlers[handler.Identifier].Add(handler);
		}

		private void RegisterInvocation(JSRuntimeInvocation invocation)
		{
			if (!_invocations.ContainsKey(invocation.Identifier))
			{
				_invocations.Add(invocation.Identifier, new List<JSRuntimeInvocation>());
			}
			_invocations[invocation.Identifier].Add(invocation);
		}

		private JSRuntimeInvocationHandler<TResult>? TryGetHandlerFor<TResult>(JSRuntimeInvocation invocation, Predicate<JSRuntimeInvocationHandler<TResult>>? handlerPredicate = null)
		{
			handlerPredicate ??= _ => true;
			JSRuntimeInvocationHandler<TResult>? result = default;
			if (_handlers.TryGetValue(invocation.Identifier, out var plannedInvocations))
			{
				result = plannedInvocations.OfType<JSRuntimeInvocationHandler<TResult>>()
					.Where(x => handlerPredicate(x) && x.Matches(invocation)).LastOrDefault();
			}

			if (result is null && _handlers.TryGetValue(JSRuntimeInvocationHandler<TResult>.CatchAllIdentifier, out var catchAllHandlers))
			{
				result = catchAllHandlers.OfType<JSRuntimeInvocationHandler<TResult>>().Where(x => handlerPredicate(x)).LastOrDefault();
			}
			return result;
		}

		private class BUnitJSRuntime : IJSRuntime
		{
			private readonly BunitJSInterop _jsInterop;

			public BUnitJSRuntime(BunitJSInterop bunitJsInterop)
			{
				_jsInterop = bunitJsInterop;
			}

			public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
				=> InvokeAsync<TValue>(identifier, default, args);

			public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
			{
				var invocation = new JSRuntimeInvocation(identifier, cancellationToken, args);
				_jsInterop.RegisterInvocation(invocation);

				return TryHandlePlannedInvocation<TValue>(invocation) ?? new ValueTask<TValue>(default(TValue)!);
			}

			private ValueTask<TValue>? TryHandlePlannedInvocation<TValue>(JSRuntimeInvocation invocation)
			{
				ValueTask<TValue>? result = default;

				if (_jsInterop.TryGetHandlerFor<TValue>(invocation) is JSRuntimeInvocationHandler<TValue> handler)
				{
					var task = handler.RegisterInvocation(invocation);
					result = new ValueTask<TValue>(task);
				}
				else if (_jsInterop.Mode == JSRuntimeMode.Strict)
				{
					throw new JSRuntimeUnhandledInvocationException(invocation);
				}

				return result;
			}
		}
	}
}
