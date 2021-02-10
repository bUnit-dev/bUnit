#if NET5_0
using System;
using Microsoft.JSInterop;

namespace Bunit.JSInterop
{
	/// <summary>
	/// bUnit's implementation of <see cref="IJSUnmarshalledRuntime"/>.
	/// </summary>
	internal sealed partial class BunitJSRuntime : IJSUnmarshalledRuntime
	{
		/// <inheritdoc/>
		public TResult InvokeUnmarshalled<TResult>(string identifier)
		{
			var resultTask = InvokeAsync<TResult>(identifier, Array.Empty<object?>()).AsTask();
			return resultTask.GetAwaiter().GetResult();
		}

		/// <inheritdoc/>
		public TResult InvokeUnmarshalled<T0, TResult>(string identifier, T0 arg0)
		{
			var resultTask = InvokeAsync<TResult>(identifier, new object?[] { arg0 }).AsTask();
			return resultTask.GetAwaiter().GetResult();
		}

		/// <inheritdoc/>
		public TResult InvokeUnmarshalled<T0, T1, TResult>(string identifier, T0 arg0, T1 arg1)
		{
			var resultTask = InvokeAsync<TResult>(identifier, new object?[] { arg0, arg1 }).AsTask();
			return resultTask.GetAwaiter().GetResult();
		}

		/// <inheritdoc/>
		public TResult InvokeUnmarshalled<T0, T1, T2, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2)
		{
			var resultTask = InvokeAsync<TResult>(identifier, new object?[] { arg0, arg1, arg2 }).AsTask();
			return resultTask.GetAwaiter().GetResult();
		}
	}
}
#endif
