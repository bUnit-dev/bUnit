#if NET6_0_OR_GREATER
using System;
using Microsoft.AspNetCore.Components;
namespace Bunit.JSInterop.InvocationHandlers.Implementation
{
	/// <summary>
	/// Represents a handler for Blazor's
	/// <see cref="ElementReferenceExtensions.FocusAsync(ElementReference)"/> feature.
	/// </summary>
	internal sealed class FocusOnNavigateHandler : JSRuntimeInvocationHandler
	{
		/// <summary>
		/// The internal identifier used by <see cref="ElementReferenceExtensions.FocusAsync(ElementReference)"/>
		/// to call it JavaScript.
		/// </summary>
		public const string Identifier = "Blazor._internal.domWrapper.focusBySelector";

		/// <summary>
		/// Initializes a new instance of the <see cref="FocusAsyncInvocationHandler"/> class.
		/// </summary>
		internal FocusOnNavigateHandler()
			: base(inv => inv.Identifier.Equals(Identifier, StringComparison.Ordinal), isCatchAllHandler: true)
		{ }
	}
}
#endif
