using System;
using System.Threading.Tasks;

namespace Bunit.JSInterop.InvocationHandlers
{
	/// <summary>
	/// Represents a handler for an invocation of a JavaScript function which returns nothing, with specific arguments.
	/// </summary>
	public class JSRuntimeInvocationHandler : JSRuntimeInvocationHandlerBase<object>
	{
		/// <inheritdoc/>
		public override sealed bool IsVoidResultHandler { get; } = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="JSRuntimeInvocationHandler"/> class.
		/// </summary>
		/// <param name="matcher">An invocation matcher used to determine if the handler should handle an invocation.</param>
		/// <param name="isCatchAllHandler">Set to true if this handler is a catch all handler, that should only be used if there are no other non-catch all handlers available.</param>
		protected internal JSRuntimeInvocationHandler(InvocationMatcher matcher, bool isCatchAllHandler)
			: base(matcher, isCatchAllHandler) { }

		/// <summary>
		/// Completes the current awaiting void invocation requests.
		/// </summary>
		/// <returns>This handler to allow calls to be chained.</returns>
		public JSRuntimeInvocationHandler SetVoidResult()
		{
			SetResultBase(default!);
			return this;
		}

		/// <summary>
		/// Marks the <see cref="Task"/> that invocations will receive as canceled.
		/// </summary>
		/// <returns>This handler to allow calls to be chained.</returns>
		public JSRuntimeInvocationHandler SetCanceled()
		{
			SetCanceledBase();
			return this;
		}

		/// <summary>
		/// Sets the <typeparamref name="TException"/> exception that invocations will receive.
		/// </summary>
		/// <param name="exception">The exception to set.</param>
		/// <returns>This handler to allow calls to be chained.</returns>
		public JSRuntimeInvocationHandler SetException<TException>(TException exception)
			where TException : Exception
		{
			SetExceptionBase(exception);
			return this;
		}
	}
}
