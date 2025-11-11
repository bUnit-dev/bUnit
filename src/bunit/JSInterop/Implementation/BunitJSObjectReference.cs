using Bunit.JSInterop.Implementation;

namespace Bunit;

[SuppressMessage("Minor Code Smell", "S1939:Inheritance list should not be redundant", Justification = "By design. To make it obvious that both is implemented.")]
internal sealed class BunitJSObjectReference : IJSObjectReference, IJSInProcessObjectReference
#if !NET9_0_OR_GREATER
	, IJSUnmarshalledObjectReference
#endif
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
		=> JSInterop.HandleInvokeAsync<TValue>(identifier, cancellationToken, args);

#if NET10_0_OR_GREATER
	/// <inheritdoc/>

	/// <inheritdoc/>
	public ValueTask<TValue> GetValueAsync<TValue>(string identifier) => throw new NotImplementedException();

	/// <inheritdoc/>
	public ValueTask<TValue> GetValueAsync<TValue>(string identifier, CancellationToken cancellationToken) => throw new NotImplementedException();

	/// <inheritdoc/>
	public ValueTask SetValueAsync<TValue>(string identifier, TValue value) => throw new NotImplementedException();

	/// <inheritdoc/>
	public ValueTask SetValueAsync<TValue>(string identifier, TValue value, CancellationToken cancellationToken) => throw new NotImplementedException();

	/// <inheritdoc/>
	public TValue GetValue<TValue>(string identifier) => throw new NotImplementedException();

	/// <inheritdoc/>
	public void SetValue<TValue>(string identifier, TValue value) => throw new NotImplementedException();
#endif

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
