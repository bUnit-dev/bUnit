using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Bunit.JSInterop.Implementation;
using Microsoft.JSInterop;

namespace Bunit;

internal sealed class BunitJSObjectReference : IJSUnmarshalledObjectReference
{
	private BunitJSInterop JSInterop { get; }

	public BunitJSObjectReference(BunitJSInterop jsInterop)
	{
		JSInterop = jsInterop;
	}

	/// <inheritdoc/>
	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
		=> JSInterop.HandleInvokeAsync<TValue>(identifier, args);

	/// <inheritdoc/>
	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
		=> JSInterop.HandleInvokeAsync<TValue>(identifier, args, cancellationToken);

	/// <inheritdoc/>
	public TValue Invoke<TValue>(string identifier, params object?[]? args)
		=> JSInterop.HandleInvoke<TValue>(identifier, args);

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

	/// <inheritdoc/>
	public void Dispose()
	{
		// Just here to meet the interface requirements. Nothing to dispose.
	}

	/// <inheritdoc/>
	public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
