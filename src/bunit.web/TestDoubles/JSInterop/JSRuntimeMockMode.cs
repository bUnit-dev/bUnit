using Microsoft.JSInterop;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// The execution mode of the <see cref="MockJSRuntimeExtensions"/>.
	/// </summary>
	public enum JSRuntimeMockMode
	{
		/// <summary>
		/// <see cref="Loose"/> configures the <see cref="MockJSRuntimeExtensions"/> to return default TValue 
		/// for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> calls to the mock.
		/// </summary>
		Loose = 0,
		/// <summary>
		/// <see cref="Strict"/> configures the <see cref="MockJSRuntimeExtensions"/> to throw an
		/// <see cref="UnplannedJSInvocationException"/> exception when a call to 
		/// for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> has not been 
		/// setup in the mock.
		/// </summary>
		Strict
	}
}
