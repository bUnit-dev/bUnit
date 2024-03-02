#if NET5_0_OR_GREATER && !NET9_0_OR_GREATER
using System;
using Bunit.JSInterop.Implementation;
using Microsoft.JSInterop;

namespace Bunit.JSInterop;

/// <summary>
/// bUnit's implementation of <see cref="IJSUnmarshalledRuntime"/>.
/// </summary>
internal sealed partial class BunitJSRuntime : IJSUnmarshalledRuntime
{
	/// <inheritdoc/>
	public TResult InvokeUnmarshalled<TResult>(string identifier)
		=> JSInterop.HandleInvokeUnmarshalled<TResult>(identifier);

	/// <inheritdoc/>
	public TResult InvokeUnmarshalled<T0, TResult>(string identifier, T0 arg0)
		=> JSInterop.HandleInvokeUnmarshalled<T0, TResult>(identifier, arg0);

	/// <inheritdoc/>
	public TResult InvokeUnmarshalled<T0, T1, TResult>(string identifier, T0 arg0, T1 arg1)
		=> JSInterop.HandleInvokeUnmarshalled<T0, T1, TResult>(identifier, arg0, arg1);

	/// <inheritdoc/>
	public TResult InvokeUnmarshalled<T0, T1, T2, TResult>(string identifier, T0 arg0, T1 arg1, T2 arg2)
		=> JSInterop.HandleInvokeUnmarshalled<T0, T1, T2, TResult>(identifier, arg0, arg1, arg2);
}
#endif
