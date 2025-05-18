using Bunit.JSInterop.Implementation;

namespace Bunit.JSInterop;

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
		=> JSInterop.HandleInvokeAsync<TValue>(identifier, args);

	/// <inheritdoc/>
	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
		=> JSInterop.HandleInvokeAsync<TValue>(identifier, cancellationToken, args);

#if NET10_0_OR_GREATER
	/// <inheritdoc/>
	public ValueTask<IJSObjectReference> InvokeNewAsync(string identifier, object?[]? args) => throw new NotImplementedException();

	/// <inheritdoc/>
	public ValueTask<IJSObjectReference> InvokeNewAsync(string identifier, CancellationToken cancellationToken, object?[]? args) => throw new NotImplementedException();

	/// <inheritdoc/>
	public ValueTask<TValue> GetValueAsync<TValue>(string identifier) => throw new NotImplementedException();

	/// <inheritdoc/>
	public ValueTask<TValue> GetValueAsync<TValue>(string identifier, CancellationToken cancellationToken) => throw new NotImplementedException();

	/// <inheritdoc/>
	public ValueTask SetValueAsync<TValue>(string identifier, TValue value) => throw new NotImplementedException();

	/// <inheritdoc/>
	public ValueTask SetValueAsync<TValue>(string identifier, TValue value, CancellationToken cancellationToken) => throw new NotImplementedException();

	/// <inheritdoc/>
	public IJSInProcessObjectReference InvokeNew(string identifier, params object?[]? args) => throw new NotImplementedException();

	/// <inheritdoc/>
	public TValue GetValue<TValue>(string identifier) => throw new NotImplementedException();

	/// <inheritdoc/>
	public void SetValue<TValue>(string identifier, TValue value) => throw new NotImplementedException();
#endif

	/// <inheritdoc/>
	public TResult Invoke<TResult>(string identifier, params object?[]? args)
		=> JSInterop.HandleInvoke<TResult>(identifier, args);
}
