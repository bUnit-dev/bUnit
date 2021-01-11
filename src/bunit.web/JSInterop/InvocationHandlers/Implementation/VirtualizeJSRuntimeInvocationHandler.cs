#if NET5_0
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;

namespace Bunit.JSInterop.InvocationHandlers.Implementation
{
	/// <summary>
	/// Represents an JSInterop handler for the <see cref="Virtualize{TItem}"/> component.
	/// </summary>
	internal sealed class VirtualizeJSRuntimeInvocationHandler : JSRuntimeInvocationHandler
	{
		private const string JsFunctionsPrefix = "Blazor._internal.Virtualize.";
		private static readonly Lazy<(PropertyInfo, MethodInfo)> VirtualizeReflection = new Lazy<(PropertyInfo, MethodInfo)>(() =>
		{
			var virtualizeJsInteropType = typeof(Virtualize<>)
				.Assembly
				.GetType("Microsoft.AspNetCore.Components.Web.Virtualization.VirtualizeJsInterop")
					?? throw new InvalidOperationException("Did not find the VirtualizeJsInterop in the expected namespace/assembly.");

			var dotNetObjectReferenceVirtualizeJsInteropType = typeof(DotNetObjectReference<>).MakeGenericType(virtualizeJsInteropType);

			var dotNetObjectReferenceValuePropertyInfo = dotNetObjectReferenceVirtualizeJsInteropType
				.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance)
				?? throw new InvalidOperationException("Did not find the Value property on the DotNetObjectReference<VirtualizeJsInterop> type.");

			var onSpacerBeforeVisibleMethodInfo = virtualizeJsInteropType?.GetMethod("OnSpacerBeforeVisible")
				?? throw new InvalidOperationException("Did not find the OnSpacerBeforeVisible method on the VirtualizeJsInterop type.");

			return (dotNetObjectReferenceValuePropertyInfo, onSpacerBeforeVisibleMethodInfo);
		});

		internal VirtualizeJSRuntimeInvocationHandler()
			: base(CatchAllIdentifier, i => i.Identifier.StartsWith(JsFunctionsPrefix, StringComparison.Ordinal))
		{ }

		/// <inheritdoc/>
		protected internal override Task<object> HandleAsync(JSRuntimeInvocation invocation)
		{
			if (!invocation.Identifier.Equals(JsFunctionsPrefix + "dispose", StringComparison.Ordinal))
			{
				// Assert expectations about the internals of the <Virtualize> component
				Debug.Assert(invocation.Identifier.Equals(JsFunctionsPrefix + "init", StringComparison.Ordinal), "Received an unexpected invocation identifier from the <Virtualize> component.");
				Debug.Assert(invocation.Arguments.Count == 3, "Received an unexpected amount of arguments from the <Virtualize> component.");
				Debug.Assert(invocation.Arguments[0] is not null, "Received an unexpected null argument, expected an DotNetObjectReference<VirtualizeJsInterop> instance.");

				InvokeOnSpacerBeforeVisible(invocation.Arguments[0]!);

				SetVoidResult();
			}

			return base.HandleAsync(invocation);
		}

		private static void InvokeOnSpacerBeforeVisible(object dotNetObjectReference)
		{
			var (dotNetObjectReferenceValuePropertyInfo, onSpacerBeforeVisibleMethodInfo) = VirtualizeReflection.Value;
			var virtualizeJsInterop = dotNetObjectReferenceValuePropertyInfo.GetValue(dotNetObjectReference);
			var parameters = new object[]
			{
				0f, /* spacerSize */
				0f, /* spacerSeparation */
				1_000_000_000f, /* containerSize - very large number to ensure all items are loaded at once */
			};
			onSpacerBeforeVisibleMethodInfo.Invoke(virtualizeJsInterop, parameters);
		}
	}
}
#endif
