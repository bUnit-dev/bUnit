using Bunit.JSInterop.Implementation;

namespace Bunit.JSInterop;

/// <summary>
/// bUnit's implementation of the <see cref="IJSRuntime"/>
/// and <see cref="IJSInProcessRuntime"/> types.
/// </summary>
internal sealed class BunitJSRuntime : IJSInProcessRuntime, IJSUnmarshalledRuntime
{
	private BunitJSInterop JSInterop { get; }

	public BunitJSRuntime(BunitJSInterop jsInterop)
	{
		JSInterop = jsInterop;
	}

	/// <inheritdoc/>
	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
		=> JSInterop.HandleInvokeAsync<TValue>(identifier, args);

	/// <inheritdoc/>
	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
		=> JSInterop.HandleInvokeAsync<TValue>(identifier, cancellationToken, args);

	/// <inheritdoc/>
	public TResult Invoke<TResult>(string identifier, params object?[]? args)
		=> JSInterop.HandleInvoke<TResult>(identifier, args);

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
