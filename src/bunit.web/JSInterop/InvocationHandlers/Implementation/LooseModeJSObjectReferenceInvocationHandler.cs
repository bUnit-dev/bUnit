#if NET5_0
using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace Bunit.JSInterop.InvocationHandlers.Implementation
{
	/// <summary>
	/// Special <see cref="JSRuntimeMode.Loose"/> mode invocation handler for <see cref="IJSObjectReference"/>.
	/// Will match all loose mode calls of the parent <see cref="BunitJSInterop"/>.
	/// </summary>
	internal sealed class LooseModeJSObjectReferenceInvocationHandler : JSRuntimeInvocationHandler<IJSObjectReference>
	{
		[SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "BunitJSObjectReference doesn't have any disposable ressources, it just implements the methods to be compatible with the interfaces it implements.")]
		internal LooseModeJSObjectReferenceInvocationHandler(BunitJSInterop parent)
			: base(CatchAllIdentifier, _ => parent.Mode == JSRuntimeMode.Loose)
		{
			SetResult(new BunitJSObjectReference(parent.JSRuntime));
		}
	}
}
#endif
