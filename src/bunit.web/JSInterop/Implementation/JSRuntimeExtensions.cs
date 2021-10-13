using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Bunit.JSInterop.Implementation
{
	internal static class JSRuntimeExtensions
	{
		internal static ValueTask<TValue> HandleInvokeAsync<TValue>(this BunitJSInterop jSInterop, string identifier, object?[]? args)
		{
			var invocationMethodName = GetInvokeAsyncMethodName<TValue>();
			var invocation = new JSRuntimeInvocation(identifier, null, args, typeof(TValue), invocationMethodName);
			return jSInterop.HandleInvocation<TValue>(invocation);
		}

		[SuppressMessage("Design", "CA1068:CancellationToken parameters must come last", Justification = "Matching Blazor's JSRuntime design.")]
		internal static ValueTask<TValue> HandleInvokeAsync<TValue>(this BunitJSInterop jSInterop, string identifier, CancellationToken cancellationToken, object?[]? args)
		{
			var invocationMethodName = GetInvokeAsyncMethodName<TValue>();
			var invocation = new JSRuntimeInvocation(identifier, cancellationToken, args, typeof(TValue), invocationMethodName);
			return jSInterop.HandleInvocation<TValue>(invocation);
		}

		internal static TValue HandleInvoke<TValue>(this BunitJSInterop jSInterop, string identifier, params object?[]? args)
		{
			var invocationMethodName = GetInvokeMethodName<TValue>();
			var invocation = new JSRuntimeInvocation(identifier, null, args, typeof(TValue), invocationMethodName);
			var resultTask = jSInterop.HandleInvocation<TValue>(invocation).AsTask();
			return resultTask.GetAwaiter().GetResult();
		}

		internal static TResult HandleInvokeUnmarshalled<TResult>(this BunitJSInterop jSInterop, string identifier)
		{
			var invocation = new JSRuntimeInvocation(
				identifier,
				null,
				Array.Empty<object?>(),
				typeof(TResult),
				"InvokeUnmarshalled");

			return jSInterop.HandleInvocation<TResult>(invocation)
				.AsTask()
				.GetAwaiter()
				.GetResult();
		}

		internal static TResult HandleInvokeUnmarshalled<T0, TResult>(this BunitJSInterop jSInterop, string identifier, T0 arg0)
		{
			var invocation = new JSRuntimeInvocation(
				identifier,
				null,
				new object?[] { arg0 },
				typeof(TResult),
				"InvokeUnmarshalled");

			return jSInterop.HandleInvocation<TResult>(invocation)
				.AsTask()
				.GetAwaiter()
				.GetResult();
		}

		internal static TResult HandleInvokeUnmarshalled<T0, T1, TResult>(this BunitJSInterop jSInterop, string identifier, T0 arg0, T1 arg1)
		{
			var invocation = new JSRuntimeInvocation(
				identifier,
				null,
				new object?[] { arg0, arg1 },
				typeof(TResult),
				"InvokeUnmarshalled");

			return jSInterop.HandleInvocation<TResult>(invocation)
				.AsTask()
				.GetAwaiter()
				.GetResult();
		}

		internal static TResult HandleInvokeUnmarshalled<T0, T1, T2, TResult>(this BunitJSInterop jSInterop, string identifier, T0 arg0, T1 arg1, T2 arg2)
		{
			var invocation = new JSRuntimeInvocation(
				identifier,
				null,
				new object?[] { arg0, arg1, arg2 },
				typeof(TResult),
				"InvokeUnmarshalled");

			return jSInterop.HandleInvocation<TResult>(invocation)
				.AsTask()
				.GetAwaiter()
				.GetResult();
		}

#if !NET6_0_OR_GREATER
		private static string GetInvokeAsyncMethodName<TValue>()
			=> typeof(TValue) == typeof(object)
				? "InvokeVoidAsync"
				: "InvokeAsync";
#else
		private static string GetInvokeAsyncMethodName<TValue>()
			=> typeof(TValue) == typeof(Microsoft.JSInterop.Infrastructure.IJSVoidResult)
				? "InvokeVoidAsync"
				: "InvokeAsync";
#endif

#if !NET6_0_OR_GREATER
		private static string GetInvokeMethodName<TValue>()
			=> typeof(TValue) == typeof(object)
				? "InvokeVoid"
				: "Invoke";
#else
		private static string GetInvokeMethodName<TValue>()
			=> typeof(TValue) == typeof(Microsoft.JSInterop.Infrastructure.IJSVoidResult)
				? "InvokeVoid"
				: "Invoke";
#endif
	}
}
