using Bunit.JSInterop.InvocationHandlers.Implementation;

namespace Bunit;

/// <summary>
/// Helper methods for creating invocation handlers and adding the to a <see cref="BunitJSInterop"/>.
/// </summary>
public static partial class BunitJSInteropSetupExtensions
{
	private const string DefaultImportIdentifier = "import";

	/// <summary>
	/// Setup a handler for a <c>IJSRuntime.InvokeAsync&lt;IJSObjectReference&gt;()</c> call whose input parameters is matched by the provided
	/// <paramref name="invocationMatcher"/>.
	/// </summary>
	/// <remarks>
	/// The returned <see cref="BunitJSInterop"/> can be used to setup handlers for
	/// <c>InvokeAsync&lt;TValue&gt;(string, object?[]?)"</c> calls to the module, using either
	/// <see cref="SetupModule(BunitJSInterop, string)"/> or Setup calls.
	/// </remarks>
	/// <param name="jsInterop">The JSInterop to setup the handler for.</param>
	/// <param name="invocationMatcher">The matcher to use to match <see cref="JSRuntimeInvocation"/>'s with.</param>
	/// <param name="isCatchAllHandler">Set to true if the created handler is a catch all handler, that should only be used if there are no other non-catch all handlers available.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="jsInterop"/> is null.</exception>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="invocationMatcher"/> is null.</exception>
	/// <returns>A <see cref="BunitJSModuleInterop"/>.</returns>
	public static BunitJSModuleInterop SetupModule(this BunitJSInterop jsInterop, InvocationMatcher invocationMatcher, bool isCatchAllHandler = false)
	{
		ArgumentNullException.ThrowIfNull(jsInterop);
		ArgumentNullException.ThrowIfNull(invocationMatcher);

		var result = new JSObjectReferenceInvocationHandler(jsInterop, invocationMatcher, isCatchAllHandler);
		jsInterop.AddInvocationHandler(result);
		return result.JSInterop;
	}

	/// <summary>
	/// Setup a handler for a <c>IJSRuntime.InvokeAsync&lt;IJSObjectReference&gt;()</c> call whose input parameters is matched by the provided
	/// <paramref name="invocationMatcher"/> and the <paramref name="identifier"/>.
	/// </summary>
	/// <remarks>
	/// The returned <see cref="BunitJSInterop"/> can be used to setup handlers for
	/// <c>InvokeAsync&lt;TValue&gt;(string, object?[]?)"</c> calls to the module, using either
	/// <see cref="SetupModule(BunitJSInterop, string)"/> or Setup calls.
	/// </remarks>
	/// <param name="jsInterop">The JSInterop to setup the handler for.</param>
	/// <param name="identifier">The identifier to setup a response for.</param>
	/// <param name="invocationMatcher">The matcher to use to match <see cref="JSRuntimeInvocation"/>'s with.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="jsInterop"/> is null.</exception>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="invocationMatcher"/> is null.</exception>
	/// <returns>A <see cref="BunitJSModuleInterop"/>.</returns>
	public static BunitJSModuleInterop SetupModule(this BunitJSInterop jsInterop, string identifier, InvocationMatcher invocationMatcher)
		=> SetupModule(jsInterop, inv => identifier.Equals(inv.Identifier, StringComparison.Ordinal) && invocationMatcher(inv));

	/// <summary>
	/// Setup a handler for a <c>IJSRuntime.InvokeAsync&lt;IJSObjectReference&gt;("import", <paramref name="moduleName"/>)</c>
	/// call.
	/// </summary>
	/// <remarks>
	/// The returned <see cref="BunitJSInterop"/> can be used to setup handlers for
	/// <c>InvokeAsync&lt;TValue&gt;(string, object?[]?)"</c> calls to the module, using either
	/// <see cref="SetupModule(BunitJSInterop, string)"/> or Setup calls.
	/// </remarks>
	/// <param name="jsInterop">The JSInterop to setup the handler for.</param>
	/// <param name="moduleName">The name of the JavaScript module to handle invocations for.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="jsInterop"/> is null.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="moduleName"/> is null or whitespace.</exception>
	/// <returns>A <see cref="BunitJSModuleInterop"/>.</returns>
	public static BunitJSModuleInterop SetupModule(this BunitJSInterop jsInterop, string moduleName)
	{
		if (string.IsNullOrWhiteSpace(moduleName))
			throw new ArgumentException($"'{nameof(moduleName)}' cannot be null or whitespace.", nameof(moduleName));

		return SetupModule(
			jsInterop,
			DefaultImportIdentifier,
			invocation => invocation.Arguments?[0] is string requestedModuleName
					   && requestedModuleName.Equals(moduleName, StringComparison.Ordinal));
	}

	/// <summary>
	/// Setup a handler for a <c>IJSRuntime.InvokeAsync&lt;IJSObjectReference&gt;(<paramref name="identifier"/>, <paramref name="arguments"/>)</c>
	/// call.
	/// </summary>
	/// <remarks>
	/// The returned <see cref="BunitJSInterop"/> can be used to setup handlers for
	/// <c>InvokeAsync&lt;TValue&gt;(string, object?[]?)"</c> calls to the module, using either
	/// <see cref="SetupModule(BunitJSInterop, string)"/> or Setup calls.
	/// </remarks>
	/// <param name="jsInterop">The JSInterop to setup the handler for.</param>
	/// <param name="identifier">The identifier to setup a response for.</param>
	/// <param name="arguments">The arguments that an invocation to <paramref name="identifier"/> should match. Use <c>Array.Empty&lt;object?&gt;()</c> for none.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="jsInterop"/> is null.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="identifier"/> is null or whitespace.</exception>
	/// <returns>A <see cref="BunitJSModuleInterop"/>.</returns>
	public static BunitJSModuleInterop SetupModule(this BunitJSInterop jsInterop, string identifier, object?[] arguments)
		=> SetupModule(jsInterop, identifier, invocation => invocation.Arguments.SequenceEqual(arguments ?? Array.Empty<object?>()));

	/// <summary>
	/// Configure a catch all JSObjectReferenceInvocationHandler invocation handler for any module load and invocations
	/// on those modules.
	/// </summary>
	/// <remarks>
	/// The returned <see cref="BunitJSInterop"/> can be used to setup handlers for
	/// <c>InvokeAsync&lt;TValue&gt;(string, object?[]?)"</c> calls to the module, using either
	/// <see cref="SetupModule(BunitJSInterop, string)"/> or Setup calls.
	/// </remarks>
	/// <param name="jsInterop">The JSInterop to setup the handler for.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="jsInterop"/> is null.</exception>
	/// <returns>A <see cref="BunitJSModuleInterop"/>.</returns>
	public static BunitJSModuleInterop SetupModule(this BunitJSInterop jsInterop)
		=> SetupModule(jsInterop, _ => true, isCatchAllHandler: true);

	/// <summary>
	/// Looks through the registered handlers and returns the latest registered that can handle
	/// the provided <paramref name="identifier"/> and <paramref name="arguments"/>, and that
	/// will return <see cref="IJSObjectReference"/>.
	/// </summary>
	/// <param name="jsInterop">The JSInterop to setup the handler for.</param>
	/// <param name="identifier">The identifier the handler should match with.</param>
	/// <param name="arguments">The arguments that an invocation to <paramref name="identifier"/> should match.</param>
	/// <returns>A <see cref="BunitJSModuleInterop"/> or null if no one is found.</returns>
	public static BunitJSModuleInterop? TryGetModuleJSInterop(this BunitJSInterop jsInterop, string identifier, params object?[]? arguments)
	{
		ArgumentNullException.ThrowIfNull(jsInterop);

		var invocation = new JSRuntimeInvocation(identifier, default, arguments, typeof(IJSObjectReference), "InvokeAsync");

		var handler = jsInterop.TryGetHandlerFor<IJSObjectReference>(invocation) as JSObjectReferenceInvocationHandler;

		return handler?.JSInterop;
	}
}
