#if NET5_0
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Bunit
{
	internal sealed class BunitJSObjectReference : IJSObjectReference
	{
		private readonly IJSRuntime _jsRuntime;

		public BunitJSObjectReference(IJSRuntime jsRuntime)
		{
			_jsRuntime = jsRuntime;
		}

		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
			=> _jsRuntime.InvokeAsync<TValue>(identifier, CancellationToken.None, args);

		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
			=> _jsRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args);

		/// <inheritdoc/>
		public ValueTask DisposeAsync() => ValueTask.CompletedTask;
	}
}
#endif
