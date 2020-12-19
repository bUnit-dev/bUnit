#if NET5_0
using System;
using Microsoft.JSInterop;

namespace Bunit.JSInterop
{
	internal sealed partial class BunitJSRuntime : IJSUnmarshalledRuntime
	{
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
	}
}
#endif
