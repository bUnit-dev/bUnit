#if NET5_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit.JSInterop.InvocationHandlers
{
	/// <summary>
	/// Represents a handler for Blazor's
	/// <see cref="ElementReferenceExtensions.FocusAsync(ElementReference)"/> feature.
	/// </summary>
	public class FocusAsyncInvocationHandler : JSRuntimeInvocationHandler
	{
		/// <summary>
		/// The internal identifier used by <see cref="ElementReferenceExtensions.FocusAsync(ElementReference)"/>
		/// to call it JavaScript.
		/// </summary>
		public const string FocusIdentifier = "Blazor._internal.domWrapper.focus";

		/// <summary>
		/// Creates an instance of the <see cref="FocusEventDispatchExtensions"/>.
		/// </summary>
		protected internal FocusAsyncInvocationHandler() : base(FocusIdentifier, _ => true)
		{
		}
	}
}
#endif
