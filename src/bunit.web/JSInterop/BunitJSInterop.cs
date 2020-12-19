using System;
using System.Collections.Generic;
using System.Linq;
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
		private readonly Dictionary<string, List<object>> _handlers = new();

		/// <summary>
		/// Gets a dictionary of all <see cref="List{JSRuntimeInvocation}"/> this mock has observed.
		/// </summary>
		public JSRuntimeInvocationDictionary Invocations { get; } = new();

		/// <summary>
		/// Gets or sets whether the mock is running in <see cref="JSRuntimeMode.Loose"/> or
		/// <see cref="JSRuntimeMode.Strict"/>.
		/// </summary>
		public virtual JSRuntimeMode Mode { get; set; }

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
			JSRuntime = new BunitJSRuntime(this);
			AddCustomHandlers();
		}

		/// <summary>
		/// Adds an invocation handler to bUnit's JSInterop. Can be used to register
		/// custom invocation handlers.
		/// </summary>
		public void AddInvocationHandler<TResult>(JSRuntimeInvocationHandlerBase<TResult> handler)
		{
			if (!_handlers.ContainsKey(handler.Identifier))
			{
				_handlers.Add(handler.Identifier, new List<object>());
			}
			_handlers[handler.Identifier].Add(handler);
		}


		internal virtual void RegisterInvocation(JSRuntimeInvocation invocation)
		{
			Invocations.RegisterInvocation(invocation);
		}

		internal JSRuntimeInvocationHandlerBase<TResult>? TryGetHandlerFor<TResult>(JSRuntimeInvocation invocation, Predicate<JSRuntimeInvocationHandlerBase<TResult>>? handlerPredicate = null)
		{
			handlerPredicate ??= _ => true;
			JSRuntimeInvocationHandlerBase<TResult>? result = default;

			if (_handlers.TryGetValue(invocation.Identifier, out var plannedInvocations))
			{
				result = plannedInvocations.OfType<JSRuntimeInvocationHandlerBase<TResult>>()
					.LastOrDefault(x => handlerPredicate(x) && x.CanHandle(invocation));
			}

			if (result is null && _handlers.TryGetValue(JSRuntimeInvocationHandler.CatchAllIdentifier, out var catchAllHandlers))
			{
				result = catchAllHandlers.OfType<JSRuntimeInvocationHandlerBase<TResult>>()
					.LastOrDefault(x => handlerPredicate(x) && x.CanHandle(invocation));
			}

			return result;
		}

		private void AddCustomHandlers()
		{
#if NET5_0
			AddInvocationHandler(new FocusAsyncInvocationHandler());
			AddInvocationHandler(new VirtualizeJSRuntimeInvocationHandler());
			AddInvocationHandler(new LooseModeJSObjectReferenceInvocationHandler(this));
#endif
		}
	}
}
