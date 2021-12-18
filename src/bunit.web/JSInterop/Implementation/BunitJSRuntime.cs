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

	/// <inheritdoc/>
	public TResult Invoke<TResult>(string identifier, params object?[]? args)
		=> JSInterop.HandleInvoke<TResult>(identifier, args);
}
