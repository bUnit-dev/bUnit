#if NET5_0
using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;

namespace Bunit.JSInterop.InvocationHandlers
{
	/// <summary>
	/// Represents an JSInterop handler for the <see cref="Virtualize{TItem}"/> component.
	/// </summary>
	public class VirtualizeJSRuntimeInvocationHandler : JSRuntimeInvocationHandler
	{
		private const string JsFunctionsPrefix = "Blazor._internal.Virtualize.";
		private static readonly Lazy<(PropertyInfo, MethodInfo)> VirtualizeReflection = new Lazy<(PropertyInfo, MethodInfo)>(() =>
		{
			var VirtualizeJsInteropType = typeof(Virtualize<>)
				.Assembly
				.GetType("Microsoft.AspNetCore.Components.Web.Virtualization.VirtualizeJsInterop")
					?? throw new InvalidOperationException("Did not find the VirtualizeJsInterop in the expected namespace/assembly.");

			var DotNetObjectReferenceVirtualizeJsInteropType = typeof(DotNetObjectReference<>).MakeGenericType(VirtualizeJsInteropType);

			var dotNetObjectReferenceValuePropertyInfo = DotNetObjectReferenceVirtualizeJsInteropType
				.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance)
				?? throw new InvalidOperationException("Did not find the Value property on the DotNetObjectReference<VirtualizeJsInterop> type.");

			var onSpacerBeforeVisibleMethodInfo = VirtualizeJsInteropType?.GetMethod("OnSpacerBeforeVisible")
				?? throw new InvalidOperationException("Did not find the OnSpacerBeforeVisible method on the VirtualizeJsInterop type.");

			return (dotNetObjectReferenceValuePropertyInfo, onSpacerBeforeVisibleMethodInfo);
		});

		internal VirtualizeJSRuntimeInvocationHandler()
			: base(CatchAllIdentifier, i => i.Identifier.StartsWith(JsFunctionsPrefix, StringComparison.Ordinal))
		{ }

		/// <inheritdoc/>
		protected override void OnInvocation(JSRuntimeInvocation invocation)
		{
			if (invocation.Identifier.Equals(JsFunctionsPrefix + "dispose", StringComparison.Ordinal))
				return;

			// Assert expectations about the internals of the <Virtualize> component
			Debug.Assert(invocation.Identifier.Equals(JsFunctionsPrefix + "init", StringComparison.Ordinal), "Received an unexpected invocation identifier from the <Virtualize> component.");
			Debug.Assert(invocation.Arguments.Count == 3, "Received an unexpected amount of arguments from the <Virtualize> component.");
			Debug.Assert(invocation.Arguments[0] is not null, "Received an unexpected null argument, expected an DotNetObjectReference<VirtualizeJsInterop> instance.");

			InvokeOnSpacerBeforeVisible(invocation.Arguments[0]!);

			SetVoidResult();
		}

		private static void InvokeOnSpacerBeforeVisible(object dotNetObjectReference)
		{
			var (dotNetObjectReferenceValuePropertyInfo, onSpacerBeforeVisibleMethodInfo) = VirtualizeReflection.Value;
			var virtualizeJsInterop = dotNetObjectReferenceValuePropertyInfo.GetValue(dotNetObjectReference);			
			onSpacerBeforeVisibleMethodInfo.Invoke(
				virtualizeJsInterop,
				new object[] {
					0f /* spacerSize */,
					0f /* spacerSeparation */,
					1_000_000_000f /* containerSize - very large number to ensure all items are loaded at once */
				});
		}
	}
}
#endif
