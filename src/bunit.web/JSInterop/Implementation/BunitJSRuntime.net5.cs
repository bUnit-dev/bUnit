#if NET5_0
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace Bunit.JSInterop
{
	/// <summary>
	/// bUnit's implementation of <see cref="IJSUnmarshalledRuntime"/>.
	/// </summary>
	[SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits", Justification = "By design. Test and renderer run in separate threads, making this safe.")]
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
