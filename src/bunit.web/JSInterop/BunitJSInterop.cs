using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit.JSInterop;
using Bunit.JSInterop.InvocationHandlers;
#if NET5_0
using Bunit.JSInterop.InvocationHandlers.Implementation;
#endif
using Microsoft.JSInterop;

namespace Bunit
{
	/// <summary>
	/// Represents an bUnit's implementation of Blazor's JSInterop.
	/// </summary>
	public class BunitJSInterop
	{
		private readonly Dictionary<string, List<object>> handlers = new(StringComparer.Ordinal);
		private JSRuntimeMode mode;

		/// <summary>
		/// Gets a dictionary of all <see cref="List{JSRuntimeInvocation}"/> this mock has observed.
		/// </summary>
		public JSRuntimeInvocationDictionary Invocations { get; } = new();

		/// <summary>
		/// Gets or sets whether the <see cref="BunitJSInterop"/> is running in <see cref="JSRuntimeMode.Loose"/> or
		/// <see cref="JSRuntimeMode.Strict"/>.
		/// </summary>
		public virtual JSRuntimeMode Mode { get => mode; set => mode = value; }

		/// <summary>
		/// Gets the mocked <see cref="IJSRuntime"/> instance.
		/// </summary>
		public IJSRuntime JSRuntime { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BunitJSInterop"/> class.
		/// </summary>
		public BunitJSInterop()
		{
			mode = JSRuntimeMode.Strict;
			JSRuntime = new BunitJSRuntime(this);
#if NET5_0
			AddCustomHandlers();
#endif
		}

		/// <summary>
		/// Adds an invocation handler to bUnit's JSInterop. Can be used to register
		/// custom invocation handlers.
		/// </summary>
		public void AddInvocationHandler<TResult>(JSRuntimeInvocationHandlerBase<TResult> handler)
		{
			if (handler is null)
				throw new ArgumentNullException(nameof(handler));

			if (!handlers.ContainsKey(handler.Identifier))
				handlers.Add(handler.Identifier, new List<object>());

			handlers[handler.Identifier].Add(handler);
		}

		internal ValueTask<TValue> HandleInvocation<TValue>(JSRuntimeInvocation invocation)
		{
			RegisterInvocation(invocation);
			return TryHandlePlannedInvocation<TValue>(invocation)
				?? new ValueTask<TValue>(default(TValue)!);
		}

		private ValueTask<TValue>? TryHandlePlannedInvocation<TValue>(JSRuntimeInvocation invocation)
		{
			ValueTask<TValue>? result = default;

			if (TryGetHandlerFor<TValue>(invocation) is JSRuntimeInvocationHandlerBase<TValue> handler)
			{
				var task = handler.HandleAsync(invocation);
				result = new ValueTask<TValue>(task);
			}
			else if (Mode == JSRuntimeMode.Strict)
			{
				throw new JSRuntimeUnhandledInvocationException(invocation);
			}

			return result;
		}

		internal virtual void RegisterInvocation(JSRuntimeInvocation invocation)
		{
			Invocations.RegisterInvocation(invocation);
		}

		internal JSRuntimeInvocationHandlerBase<TResult>? TryGetHandlerFor<TResult>(JSRuntimeInvocation invocation, Predicate<JSRuntimeInvocationHandlerBase<TResult>>? handlerPredicate = null)
		{
			handlerPredicate ??= _ => true;
			JSRuntimeInvocationHandlerBase<TResult>? result = default;

			if (handlers.TryGetValue(invocation.Identifier, out var plannedInvocations))
			{
				result = plannedInvocations.OfType<JSRuntimeInvocationHandlerBase<TResult>>()
					.LastOrDefault(x => handlerPredicate(x) && x.CanHandle(invocation));
			}

			if (result is null && handlers.TryGetValue(JSRuntimeInvocationHandler.CatchAllIdentifier, out var catchAllHandlers))
			{
				result = catchAllHandlers.OfType<JSRuntimeInvocationHandlerBase<TResult>>()
					.LastOrDefault(x => handlerPredicate(x) && x.CanHandle(invocation));
			}

			return result;
		}

#if NET5_0
		private void AddCustomHandlers()
		{
			AddInvocationHandler(new FocusAsyncInvocationHandler());
			AddInvocationHandler(new VirtualizeJSRuntimeInvocationHandler());
			AddInvocationHandler(new LooseModeJSObjectReferenceInvocationHandler(this));
		}
#endif
	}
}
