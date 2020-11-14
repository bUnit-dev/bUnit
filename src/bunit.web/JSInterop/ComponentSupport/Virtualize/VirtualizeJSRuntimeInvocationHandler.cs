#if NET5_0
using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;

namespace Bunit.JSInterop.ComponentSupport.Virtualize
{
	/// <summary>
	/// Represents an JSInterop handler for the <see cref="Virtualize{TItem}"/> component.
	/// </summary>
	public class VirtualizeJSRuntimeInvocationHandler : JSRuntimeInvocationHandler
	{
		private const string JsFunctionsPrefix = "Blazor._internal.Virtualize.";
		private static readonly Type VirtualizeJsInteropType;
		private static readonly Type DotNetObjectReferenceVirtualizeJsInteropType;
		private static readonly PropertyInfo DotNetObjectReferenceValuePropertyInfo;
		private static readonly MethodInfo OnSpacerBeforeVisibleMethodInfo;

		static VirtualizeJSRuntimeInvocationHandler()
		{
			// Get <Virtualize> types needed to emulate the <Virtualize>'s JavaScript
			VirtualizeJsInteropType = typeof(Virtualize<>)
				.Assembly
				.GetType("Microsoft.AspNetCore.Components.Web.Virtualization.VirtualizeJsInterop")
				?? throw new InvalidOperationException("Did not find the VirtualizeJsInterop in the expected namespace/assembly.");
			DotNetObjectReferenceVirtualizeJsInteropType = typeof(DotNetObjectReference<>).MakeGenericType(VirtualizeJsInteropType);
			DotNetObjectReferenceValuePropertyInfo = DotNetObjectReferenceVirtualizeJsInteropType
				.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance)
				?? throw new InvalidOperationException("Did not find the Value property on the DotNetObjectReference<VirtualizeJsInterop> type.");
			OnSpacerBeforeVisibleMethodInfo = VirtualizeJsInteropType?.GetMethod("OnSpacerBeforeVisible")
				?? throw new InvalidOperationException("Did not find the OnSpacerBeforeVisible method on the VirtualizeJsInterop type.");
		}

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

			var onSpacerBeforeVisible = GetOnSpacerBeforeVisibleCallback(invocation.Arguments[0]!);

			onSpacerBeforeVisible(
				0f /* spacerSize */,
				0f /* spacerSeparation */,
				10_000_000f /* containerSize - very large number to ensure all items are loaded at once */
			);

			SetVoidResult();
		}

		private static Action<float, float, float> GetOnSpacerBeforeVisibleCallback(object dotNetObjectReference)
		{
			var virtualizeJsInterop = DotNetObjectReferenceValuePropertyInfo?.GetValue(dotNetObjectReference);

			return (spacerSize, spacerSeparation, containerSize)
				=> OnSpacerBeforeVisibleMethodInfo.Invoke(virtualizeJsInterop, new object[] { spacerSize, spacerSeparation, containerSize });
		}
	}
}
#endif
