using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace Bunit.JSInterop.InvocationHandlers.Implementation;

/// <summary>
/// Special <see cref="JSRuntimeMode.Loose"/> mode invocation handler for <see cref="IJSObjectReference"/>.
/// Will match all loose mode calls of the parent <see cref="BunitJSInterop"/>.
/// </summary>
internal sealed class LooseModeJSObjectReferenceInvocationHandler : JSRuntimeInvocationHandler<IJSObjectReference>
{
	internal LooseModeJSObjectReferenceInvocationHandler(BunitJSInterop parent)
		: base(_ => parent.Mode == JSRuntimeMode.Loose, isCatchAllHandler: true)
	{
#pragma warning disable CA2000 // BunitJSObjectReference has an empty Dispose method
		SetResult(new BunitJSObjectReference(parent));
#pragma warning restore CA2000
	}
}
