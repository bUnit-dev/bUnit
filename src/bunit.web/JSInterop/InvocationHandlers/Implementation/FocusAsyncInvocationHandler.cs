#if NET5_0
using Microsoft.AspNetCore.Components;

namespace Bunit.JSInterop.InvocationHandlers.Implementation
{
	/// <summary>
	/// Represents a handler for Blazor's
	/// <see cref="ElementReferenceExtensions.FocusAsync(ElementReference)"/> feature.
	/// </summary>
	internal sealed class FocusAsyncInvocationHandler : JSRuntimeInvocationHandler
	{
		/// <summary>
		/// The internal identifier used by <see cref="ElementReferenceExtensions.FocusAsync(ElementReference)"/>
		/// to call it JavaScript.
		/// </summary>
		public const string FocusIdentifier = "Blazor._internal.domWrapper.focus";

		/// <summary>
		/// Initializes a new instance of the <see cref="FocusAsyncInvocationHandler"/> class.
		/// </summary>
		internal FocusAsyncInvocationHandler()
			: base(FocusIdentifier, _ => true)
		{ }
	}
}
#endif
