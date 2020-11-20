namespace Bunit.JSInterop.ComponentSupport
{
	/// <summary>
	/// Helper methods for registering handlers on the <see cref="BunitJSInterop"/>.
	/// </summary>
	public static class BunitJSInteropExtensions
	{
		/// <summary>
		/// Adds the built-in JSRuntime invocation handlers to the <paramref name="jSInterop"/>.
		/// </summary>
		public static BunitJSInterop AddBuiltInJSRuntimeInvocationHandlers(this BunitJSInterop jSInterop)
		{
#if NET5_0
			//jSInterop.AddInvocationHandler(new Virtualize.VirtualizeJSRuntimeInvocationHandler());
#endif
			return jSInterop;
		}

	}
}
