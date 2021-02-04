using Microsoft.JSInterop;

namespace Bunit
{
	/// <summary>
	/// The execution mode of the <see cref="BunitJSInterop"/>.
	/// </summary>
	public enum JSRuntimeMode
	{
		/// <summary>
		/// <see cref="Loose"/> configures the <see cref="BunitJSInterop"/> to return default TValue
		/// for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> calls.
		/// </summary>
		Loose = 0,

		/// <summary>
		/// <see cref="Strict"/> configures the <see cref="BunitJSInterop"/> to throw an
		/// <see cref="JSRuntimeUnhandledInvocationException"/> exception when a call to
		/// for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> has not been
		/// setup.
		/// </summary>
		Strict,
	}
}
