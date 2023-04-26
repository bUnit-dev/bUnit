using Bunit.JSInterop.InvocationHandlers.Implementation;

namespace Bunit;

/// <summary>
/// Helper methods for creating invocation handlers and adding the to a <see cref="BunitJSInterop"/>.
/// </summary>
public static class BunitJSInteropSetupExtensions
{
	private const string DefaultImportIdentifier = "import";

	/// <summary>
	/// Configure a JSInterop invocation handler for an <c>InvokeAsync&lt;TResult&gt;</c> call with arguments
	/// passing the <paramref name="invocationMatcher"/> test.
	/// </summary>
	/// <typeparam name="TResult">The result type of the invocation.</typeparam>
	/// <param name="jsInterop">The bUnit JSInterop to setup the invocation handling with.</param>
	/// <param name="invocationMatcher">A matcher that is passed an <see cref="JSRuntimeInvocation"/>. If it returns true the invocation is matched.</param>
	/// <param name="isCatchAllHandler">Set to true if the created handler is a catch all handler, that should only be used if there are no other non-catch all handlers available.</param>
	/// <returns>A <see cref="JSRuntimeInvocationHandler{TResult}"/>.</returns>
	public static JSRuntimeInvocationHandler<TResult> Setup<TResult>(this BunitJSInterop jsInterop, InvocationMatcher invocationMatcher, bool isCatchAllHandler = false)
	{
		if (jsInterop is null)
			throw new ArgumentNullException(nameof(jsInterop));

		EnsureResultNotIJSObjectReference<TResult>();

		var result = new JSRuntimeInvocationHandler<TResult>(invocationMatcher, isCatchAllHandler);
		jsInterop.AddInvocationHandler(result);
		return result;
	}

	/// <summary>
	/// Configure a JSInterop invocation handler with the <paramref name="identifier"/> and arguments
	/// passing the <paramref name="invocationMatcher"/> test.
	/// </summary>
	/// <typeparam name="TResult">The result type of the invocation.</typeparam>
	/// <param name="jsInterop">The bUnit JSInterop to setup the invocation handling with.</param>
	/// <param name="identifier">The identifier to setup a response for.</param>
	/// <param name="invocationMatcher">A matcher that is passed an <see cref="JSRuntimeInvocation"/> associated with  the<paramref name="identifier"/>. If it returns true the invocation is matched.</param>
	/// <returns>A <see cref="JSRuntimeInvocationHandler{TResult}"/>.</returns>
	public static JSRuntimeInvocationHandler<TResult> Setup<TResult>(this BunitJSInterop jsInterop, string identifier, InvocationMatcher invocationMatcher)
		=> Setup<TResult>(jsInterop, inv => identifier.Equals(inv.Identifier, StringComparison.Ordinal) && invocationMatcher(inv));

	/// <summary>
	/// Configure a JSInterop invocation handler with the <paramref name="identifier"/> and <paramref name="arguments"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of value to match with in a InvokeAsync&lt;TResult&gt; call.</typeparam>
	/// <param name="jsInterop">The bUnit JSInterop to setup the invocation handling with.</param>
	/// <param name="identifier">The identifier to setup a response for.</param>
	/// <param name="arguments">The arguments that an invocation to <paramref name="identifier"/> should match.</param>
	/// <returns>A <see cref="JSRuntimeInvocationHandler{TResult}"/>.</returns>
	public static JSRuntimeInvocationHandler<TResult> Setup<TResult>(this BunitJSInterop jsInterop, string identifier, params object?[]? arguments)
		=> Setup<TResult>(jsInterop, identifier, invocation => invocation.Arguments.SequenceEqual(arguments ?? Array.Empty<object?>()));

	/// <summary>
	/// Configure a catch all JSInterop invocation handler for a specific return type.
	/// This will match only on the <typeparamref name="TResult"/>, and any arguments passed to
	/// <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/>.
	/// </summary>
	/// <typeparam name="TResult">The result type of the invocation.</typeparam>
	/// <param name="jsInterop">The bUnit JSInterop to setup the invocation handling with.</param>
	/// <returns>A <see cref="JSRuntimeInvocationHandler{TResult}"/>.</returns>
	public static JSRuntimeInvocationHandler<TResult> Setup<TResult>(this BunitJSInterop jsInterop)
		=> Setup<TResult>(jsInterop, _ => true, isCatchAllHandler: true);

	/// <summary>
	/// Configure a JSInterop invocation handler for an <c>InvokeVoidAsync</c> call with arguments
	/// passing the <paramref name="invocationMatcher"/> test, that should not receive any result.
	/// </summary>
	/// <param name="jsInterop">The bUnit JSInterop to setup the invocation handling with.</param>
	/// <param name="invocationMatcher">A matcher that is passed an <see cref="JSRuntimeInvocation"/>. If it returns true the invocation is matched.</param>
	/// <param name="isCatchAllHandler">Set to true if the created handler is a catch all handler, that should only be used if there are no other non-catch all handlers available.</param>
	/// <returns>A <see cref="JSRuntimeInvocationHandler"/>.</returns>
	public static JSRuntimeInvocationHandler SetupVoid(this BunitJSInterop jsInterop, InvocationMatcher invocationMatcher, bool isCatchAllHandler = false)
	{
		if (jsInterop is null)
			throw new ArgumentNullException(nameof(jsInterop));

		var result = new JSRuntimeInvocationHandler(invocationMatcher, isCatchAllHandler);
		jsInterop.AddInvocationHandler(result);
		return result;
	}

	/// <summary>
	/// Configure a JSInterop invocation handler with the <paramref name="identifier"/> and arguments
	/// passing the <paramref name="invocationMatcher"/> test, that should not receive any result.
	/// </summary>
	/// <param name="jsInterop">The bUnit JSInterop to setup the invocation handling with.</param>
	/// <param name="identifier">The identifier to setup a response for.</param>
	/// <param name="invocationMatcher">A matcher that is passed an <see cref="JSRuntimeInvocation"/> associated with  the<paramref name="identifier"/>. If it returns true the invocation is matched.</param>
	/// <returns>A <see cref="JSRuntimeInvocationHandler"/>.</returns>
	public static JSRuntimeInvocationHandler SetupVoid(this BunitJSInterop jsInterop, string identifier, InvocationMatcher invocationMatcher)
		=> SetupVoid(jsInterop, inv => identifier.Equals(inv.Identifier, StringComparison.Ordinal) && invocationMatcher(inv));

	/// <summary>
	/// Configure a JSInterop invocation handler with the <paramref name="identifier"/>
	/// and <paramref name="arguments"/>, that should not receive any result.
	/// </summary>
	/// <param name="jsInterop">The bUnit JSInterop to setup the invocation handling with.</param>
	/// <param name="identifier">The identifier to setup a response for.</param>
	/// <param name="arguments">The arguments that an invocation to <paramref name="identifier"/> should match.</param>
	/// <returns>A <see cref="JSRuntimeInvocationHandler"/>.</returns>
	public static JSRuntimeInvocationHandler SetupVoid(this BunitJSInterop jsInterop, string identifier, params object?[]? arguments)
		=> SetupVoid(jsInterop, identifier, invocation => invocation.Arguments.SequenceEqual(arguments ?? Array.Empty<object?>()));

	/// <summary>
	/// Configure a catch all JSInterop invocation handler, that should not receive any result.
	/// </summary>
	/// <param name="jsInterop">The bUnit JSInterop to setup the invocation handling with.</param>
	/// <returns>A <see cref="JSRuntimeInvocationHandler"/>.</returns>
	public static JSRuntimeInvocationHandler SetupVoid(this BunitJSInterop jsInterop)
		=> SetupVoid(jsInterop, _ => true, isCatchAllHandler: true);

	/// <summary>
	/// Looks through the registered handlers and returns the latest registered that can handle
	/// the provided <paramref name="identifier"/> and <paramref name="arguments"/>, and that
	/// will return <typeparamref name="TResult"/>.
	/// </summary>
	/// <param name="jsInterop">The bUnit JSInterop to setup the invocation handling with.</param>
	/// <param name="identifier">The identifier the handler should match with.</param>
	/// <param name="arguments">The arguments that an invocation to <paramref name="identifier"/> should match.</param>
	/// <typeparam name="TResult">The result type of the invocation.</typeparam>
	/// <returns>Returns the <see cref="JSRuntimeInvocationHandler{TResult}"/> or null if no one is found.</returns>
	public static JSRuntimeInvocationHandler<TResult>? TryGetInvokeHandler<TResult>(this BunitJSInterop jsInterop, string identifier, params object?[]? arguments)
	{
		if (jsInterop is null)
			throw new ArgumentNullException(nameof(jsInterop));

		return jsInterop.TryGetHandlerFor<TResult>(
			new JSRuntimeInvocation(
				identifier,
				default,
				arguments,
				typeof(TResult),
				string.Empty)) as JSRuntimeInvocationHandler<TResult>;
	}

	/// <summary>
	/// Looks through the registered handlers and returns the latest registered that can handle
	/// the provided <paramref name="identifier"/> and <paramref name="arguments"/>, and that returns a "void" result.
	/// </summary>
	/// <param name="jsInterop">The bUnit JSInterop to setup the invocation handling with.</param>
	/// <param name="identifier">The identifier the handler should match with.</param>
	/// <param name="arguments">The arguments that an invocation to <paramref name="identifier"/> should match.</param>
	/// <returns>Returns the <see cref="JSRuntimeInvocationHandler"/> or null if no one is found.</returns>
	public static JSRuntimeInvocationHandler? TryGetInvokeVoidHandler(this BunitJSInterop jsInterop, string identifier, params object?[]? arguments)
	{
		if (jsInterop is null)
			throw new ArgumentNullException(nameof(jsInterop));

		var invocation = new JSRuntimeInvocation(identifier, default, arguments, typeof(object), string.Empty);

		return jsInterop.TryGetHandlerFor<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(invocation, x => x.IsVoidResultHandler) as JSRuntimeInvocationHandler;
	}

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
		if (jsInterop is null)
			throw new ArgumentNullException(nameof(jsInterop));
		if (invocationMatcher is null)
			throw new ArgumentNullException(nameof(invocationMatcher));

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
		if (jsInterop is null)
			throw new ArgumentNullException(nameof(jsInterop));

		var invocation = new JSRuntimeInvocation(identifier, default, arguments, typeof(IJSObjectReference), "InvokeAsync");

		var handler = jsInterop.TryGetHandlerFor<IJSObjectReference>(invocation) as JSObjectReferenceInvocationHandler;

		return handler?.JSInterop;
	}

	private static void EnsureResultNotIJSObjectReference<TResult>()
	{
		const string UseSetupModuleErrorMessage = "Use one of the SetupModule() methods instead to set up an invocation handler that returns an IJSObjectReference.";

		var resultType = typeof(TResult);

		if (resultType == typeof(IJSObjectReference))
			throw new ArgumentException(UseSetupModuleErrorMessage);

		if (resultType == typeof(Microsoft.JSInterop.Implementation.JSObjectReference))
			throw new ArgumentException(UseSetupModuleErrorMessage);
	}

#if NET5_0_OR_GREATER
	private static void EnsureResultNotIJSObjectReference<TResult>()
	{
		const string UseSetupModuleErrorMessage = "Use one of the SetupModule() methods instead to set up an invocation handler that returns an IJSObjectReference.";

		var resultType = typeof(TResult);

		if (resultType == typeof(IJSObjectReference))
			throw new ArgumentException(UseSetupModuleErrorMessage);

		if (resultType == typeof(Microsoft.JSInterop.Implementation.JSObjectReference))
			throw new ArgumentException(UseSetupModuleErrorMessage);
	}
#endif
}
