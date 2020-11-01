using System;
using System.Collections.Generic;
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
		private readonly Dictionary<string, List<JSRuntimeInvocation>> _invocations = new Dictionary<string, List<JSRuntimeInvocation>>();
		private readonly Dictionary<string, List<object>> _handlers = new Dictionary<string, List<object>>();
		private readonly Dictionary<Type, object> _catchAllHandlers = new Dictionary<Type, object>();

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
		/// <returns>A <see cref="JSRuntimeCatchAllInvocationHandler{TResult}"/>.</returns>
		public JSRuntimeCatchAllInvocationHandler<TResult> Setup<TResult>()
		{
			var result = new JSRuntimeCatchAllInvocationHandler<TResult>();

			_catchAllHandlers[typeof(TResult)] = result;

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
		/// <returns>A <see cref="JSRuntimeCatchAllInvocationHandler"/>.</returns>
		public JSRuntimeCatchAllInvocationHandler SetupVoid()
		{
			var result = new JSRuntimeCatchAllInvocationHandler();
			_catchAllHandlers[typeof(object)] = result;
			return result;
		}

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

				return TryHandlePlannedInvocation<TValue>(identifier, invocation)
					?? new ValueTask<TValue>(default(TValue)!);
			}

			private ValueTask<TValue>? TryHandlePlannedInvocation<TValue>(string identifier, JSRuntimeInvocation invocation)
			{
				ValueTask<TValue>? result = default;
				if (_jsInterop._handlers.TryGetValue(identifier, out var plannedInvocations))
				{
					var planned = plannedInvocations.OfType<JSRuntimeInvocationHandlerBase<TValue>>()
						.SingleOrDefault(x => x.Matches(invocation));

					if (planned is not null)
					{
						var task = planned.RegisterInvocation(invocation);
						return new ValueTask<TValue>(task);
					}
				}

				if (_jsInterop._catchAllHandlers.TryGetValue(typeof(TValue), out var catchAllInvocation))
				{
					var planned = catchAllInvocation as JSRuntimeInvocationHandlerBase<TValue>;

					if (planned is not null)
					{
						var task = ((JSRuntimeInvocationHandlerBase<TValue>)catchAllInvocation).RegisterInvocation(invocation);
						return new ValueTask<TValue>(task);
					}
				}

				if (result is null && _jsInterop.Mode == JSRuntimeMode.Strict)
				{
					throw new JSRuntimeUnhandledInvocationException(invocation);
				}

				return result;
			}
		}
	}
}
