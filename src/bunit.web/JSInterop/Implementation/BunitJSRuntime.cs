using System.Threading;
using System.Threading.Tasks;
using Bunit.JSInterop.InvocationHandlers;
using Microsoft.JSInterop;

namespace Bunit.JSInterop
{
	/// <summary>
	/// bUnit's implementation of the <see cref="IJSRuntime"/>
	/// and <see cref="IJSInProcessRuntime"/> types.
	/// </summary>
	internal sealed partial class BunitJSRuntime : IJSInProcessRuntime
	{
		private BunitJSInterop JSInterop { get; }

		public BunitJSRuntime(BunitJSInterop jsInterop)
		{
			JSInterop = jsInterop;
		}

		/// <inheritdoc/>
		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
			=> InvokeAsync<TValue>(identifier, default, args);

		/// <inheritdoc/>
		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
		{
			var invocation = new JSRuntimeInvocation(identifier, cancellationToken, args);

			JSInterop.RegisterInvocation(invocation);

			return TryHandlePlannedInvocation<TValue>(invocation)
				?? new ValueTask<TValue>(default(TValue)!);
		}

		/// <inheritdoc/>
		public TResult Invoke<TResult>(string identifier, params object?[]? args)
		{
			var resultTask = InvokeAsync<TResult>(identifier, args).AsTask();
			return resultTask.GetAwaiter().GetResult();
		}

		private ValueTask<TValue>? TryHandlePlannedInvocation<TValue>(JSRuntimeInvocation invocation)
		{
			ValueTask<TValue>? result = default;

			if (JSInterop.TryGetHandlerFor<TValue>(invocation) is JSRuntimeInvocationHandlerBase<TValue> handler)
			{
				var task = handler.HandleAsync(invocation);
				result = new ValueTask<TValue>(task);
			}
			else if (JSInterop.Mode == JSRuntimeMode.Strict)
			{
				throw new JSRuntimeUnhandledInvocationException(invocation);
			}

			return result;
		}
	}
}
