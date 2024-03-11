namespace Bunit;

/// <summary>
/// Helper methods for creating invocation handlers and adding the to a <see cref="BunitJSInterop"/>.
/// </summary>
public static partial class BunitJSInteropSetupExtensions
{
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
		ArgumentNullException.ThrowIfNull(jsInterop);

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
		ArgumentNullException.ThrowIfNull(jsInterop);

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
		ArgumentNullException.ThrowIfNull(jsInterop);

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
		ArgumentNullException.ThrowIfNull(jsInterop);

		var invocation = new JSRuntimeInvocation(identifier, default, arguments, typeof(object), string.Empty);

		return jsInterop.TryGetHandlerFor<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(invocation, x => x.IsVoidResultHandler) as JSRuntimeInvocationHandler;
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
}
