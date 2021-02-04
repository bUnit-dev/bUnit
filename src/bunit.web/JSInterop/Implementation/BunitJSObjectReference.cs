#if NET5_0
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Bunit
{
	[SuppressMessage("Minor Code Smell", "S1939:Inheritance list should not be redundant", Justification = "By design. To make it obvious that both is implemented.")]
	[SuppressMessage("Design", "CA2012:ValueTask instances should not have their result directly accessed unless the instance has already completed.", Justification = "The ValueTask always wraps a Task object.")]
	internal sealed class BunitJSObjectReference : IJSObjectReference, IJSInProcessObjectReference, IJSUnmarshalledObjectReference
	{
		private readonly IJSRuntime jsRuntime;

		public BunitJSObjectReference(IJSRuntime jsRuntime)
		{
			this.jsRuntime = jsRuntime;
		}

		/// <inheritdoc/>
		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
			=> jsRuntime.InvokeAsync<TValue>(identifier, CancellationToken.None, args);

		/// <inheritdoc/>
		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
			=> jsRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args);

		/// <inheritdoc/>
		public TValue Invoke<TValue>(string identifier, params object?[]? args)
			=> InvokeAsync<TValue>(identifier, args).GetAwaiter().GetResult();

		/// <inheritdoc/>
		public TResult InvokeUnmarshalled<TResult>(string identifier) =>
			InvokeAsync<TResult>(identifier, Array.Empty<object?>()).GetAwaiter().GetResult();

		/// <inheritdoc/>
		public TResult InvokeUnmarshalled<T0, TResult>(string identifier, T0 arg0) =>
			InvokeAsync<TResult>(identifier, new object?[] { arg0 }).GetAwaiter().GetResult();

		/// <inheritdoc/>
		public TResult InvokeUnmarshalled<T0, T1, TResult>(string identifier, T0 arg0, T1 arg1) =>
			InvokeAsync<TResult>(identifier, new object?[] { arg0, arg1 }).GetAwaiter().GetResult();

		/// <inheritdoc/>
		public TResult InvokeUnmarshalled<T0, T1, T2, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2) =>
			InvokeAsync<TResult>(identifier, new object?[] { arg0, arg1, arg2 }).GetAwaiter().GetResult();

		/// <inheritdoc/>
		public void Dispose()
		{
			// Just here to meet the interface requirements. Nothing to dispose.
		}

		/// <inheritdoc/>
		public ValueTask DisposeAsync() => ValueTask.CompletedTask;
	}
}
#endif
