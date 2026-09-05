using Bunit.JSInterop.Implementation;

namespace Bunit.JSInterop;

/// <summary>
/// bUnit's implementation of the <c>InvokeConstructorAsync</c> methods on <see cref="IJSRuntime"/>.
/// </summary>
internal sealed partial class BunitJSRuntime
{
	/// <inheritdoc/>
	ValueTask<IJSObjectReference> IJSRuntime.InvokeConstructorAsync(string identifier, object?[]? args)
		=> JSInterop.HandleInvokeConstructorAsync(identifier, args);

	/// <inheritdoc/>
	ValueTask<IJSObjectReference> IJSRuntime.InvokeConstructorAsync(string identifier, CancellationToken cancellationToken, object?[]? args)
		=> JSInterop.HandleInvokeConstructorAsync(identifier, cancellationToken, args);
}
