#if NET5_0
using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace Bunit.JSInterop.InvocationHandlers.Implementation
{
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
		[SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "BunitJSObjectReference doesn't have any disposable ressources, it just implements the methods to be compatible with the interfaces it implements.")]
		public JSObjectReferenceInvocationHandler(BunitJSInterop parent, string identifier, InvocationMatcher invocationMatcher)
			: base(identifier, invocationMatcher)
		{
			JSInterop = new BunitJSModuleInterop(parent);
			SetResult(new BunitJSObjectReference(JSInterop.JSRuntime));
		}
	}
}
#endif
