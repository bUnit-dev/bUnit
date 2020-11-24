#if NET5_0
using Bunit.JSInterop.InvocationHandlers;
#endif

namespace Bunit.JSInterop
{
	/// <summary>
	/// Helper methods for registering handlers on the <see cref="BunitJSInterop"/>.
	/// </summary>
	public static class BunitJSInteropExtensions
	{
		/// <summary>
		/// Adds the built-in JSRuntime invocation handlers to the <paramref name="jsInterop"/>.
		/// </summary>
		public static BunitJSInterop AddBuiltInJSRuntimeInvocationHandlers(this BunitJSInterop jsInterop)
		{
#if NET5_0
			jsInterop.AddInvocationHandler(new FocusAsyncInvocationHandler());
			jsInterop.AddInvocationHandler(new VirtualizeJSRuntimeInvocationHandler());
#endif
			return jsInterop;
		}
	}
}
