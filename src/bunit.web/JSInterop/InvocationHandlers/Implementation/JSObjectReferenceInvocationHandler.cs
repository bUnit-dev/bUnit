using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace Bunit.JSInterop.InvocationHandlers.Implementation;

/// <summary>
/// Represents a JavaScript module handler for requests for <see cref="IJSObjectReference"/> from bUnit's
/// <see cref="IJSRuntime"/>. This handler allows the user to setup invocation handlers
/// for the modules it is configured to handle.
/// </summary>
internal sealed class JSObjectReferenceInvocationHandler : JSRuntimeInvocationHandler<IJSObjectReference>
{
	/// <summary>
	/// Gets the <see cref="BunitJSInterop"/> for modules matching with this invocation handler.
	/// </summary>
	public BunitJSModuleInterop JSInterop { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="JSObjectReferenceInvocationHandler"/> class.
	/// </summary>
	public JSObjectReferenceInvocationHandler(BunitJSInterop parent, InvocationMatcher invocationMatcher, bool isCatchAllHandler)
		: base(invocationMatcher, isCatchAllHandler)
	{
		JSInterop = new BunitJSModuleInterop(parent);
#pragma warning disable CA2000 // BunitJSObjectReference has an empty Dispose method
		SetResult(new BunitJSObjectReference(JSInterop));
#pragma warning restore CA2000
	}
}
