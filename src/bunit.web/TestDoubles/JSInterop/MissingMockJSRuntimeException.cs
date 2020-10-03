using System;
using System.Collections.Generic;

namespace Bunit.TestDoubles.JSInterop
{
	/// <summary>
	/// Exception use to indicate that a MockJSRuntime is required by a test
	/// but was not provided.
	/// </summary>
	public class MissingMockJSRuntimeException : Exception
	{
		/// <summary>
		/// Identifier string used in the JSInvoke method.
		/// </summary>
		public string Identifier { get; }

		/// <summary>
		/// Arguments passed to the JSInvoke method.
		/// </summary>
		public IReadOnlyList<object?> Arguments { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="MissingMockJSRuntimeException"/>
		/// with the arguments used in the invocation.
		/// </summary>
		/// <param name="identifier">The identifer used in the invocation.</param>
		/// <param name="arguments">The args used in the invocation, if any</param>
		public MissingMockJSRuntimeException(string identifier, object?[]? arguments)
			: base($"This test requires a IJSRuntime to be supplied, because the component under test invokes the IJSRuntime during the test. The invoked method is '{identifier}' and the invocation arguments are stored in the {nameof(Arguments)} property of this exception. Guidance on mocking the IJSRuntime is available in the testing library's Wiki.")
		{
			Identifier = identifier;
			Arguments = arguments ?? Array.Empty<object?>();
			HelpLink = "https://github.com/egil/razor-components-testing-library/wiki/Mocking-JsRuntime";
		}
	}
}
