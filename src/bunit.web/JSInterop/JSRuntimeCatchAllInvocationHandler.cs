namespace Bunit
{
	/// <summary>
	/// Represents a handler for an invocation of a JavaScript function, which matches all types of arguments,
	/// which returns nothing.
	/// </summary>
	public class JSRuntimeCatchAllInvocationHandler : JSRuntimeInvocationHandlerBase<object>
	{
		internal JSRuntimeCatchAllInvocationHandler() { }

		internal override bool Matches(JSRuntimeInvocation invocation)
		{
			return true;
		}

		/// <summary>
		/// Completes the current awaiting void invocation request.
		/// </summary>
		public void SetVoid() => SetResultBase(default!);
	}

	/// <summary>
	/// Represents a handler for an invocation of a JavaScript function, which matches all types of arguments,
	/// that returns a <typeparamref name="TResult"/>.
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public class JSRuntimeCatchAllInvocationHandler<TResult> : JSRuntimeInvocationHandlerBase<TResult>
	{
		internal JSRuntimeCatchAllInvocationHandler()
		{ }

		internal override bool Matches(JSRuntimeInvocation invocation) => true;

		/// <summary>
		/// Sets the <typeparamref name="TResult"/> result that invocations will receive.
		/// </summary>
		/// <param name="result"></param>
		public void SetResult(TResult result) => SetResultBase(result);
	}
}
