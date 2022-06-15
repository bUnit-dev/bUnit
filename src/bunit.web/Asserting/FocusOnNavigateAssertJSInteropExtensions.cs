using System.Collections.Generic;
using Bunit.JSInterop.InvocationHandlers.Implementation;
using Microsoft.AspNetCore.Components.Routing;

namespace Bunit;

/// <summary>
/// Extensions methods for verifying <see cref="FocusOnNavigate"/> focus calls.
/// </summary>
public static class FocusOnNavigateAssertJSInteropExtensions
{
	/// <summary>
	/// Verifies that the <see cref="FocusOnNavigate"/> component has set focus one time.
	/// </summary>
	/// <param name="handler">Handler to verify against.</param>
	/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
	/// <returns>The <see cref="JSRuntimeInvocation"/>.</returns>
	public static JSRuntimeInvocation VerifyFocusOnNavigateInvoke(this BunitJSInterop handler, string? userMessage = null)
		=> handler.VerifyInvoke(FocusOnNavigateHandler.Identifier, userMessage);

	/// <summary>
	/// Verifies that the <see cref="FocusOnNavigate"/> component has set focus <paramref name="calledTimes"/> times.
	/// </summary>
	/// <param name="handler">Handler to verify against.</param>
	/// <param name="calledTimes">The number of times the invocation is expected to have been called.</param>
	/// <param name="userMessage">A custom user message to display if the assertion fails.</param>
	/// <returns>The <see cref="JSRuntimeInvocation"/>.</returns>
	public static IReadOnlyList<JSRuntimeInvocation> VerifyFocusOnNavigateInvoke(this BunitJSInterop handler, int calledTimes, string? userMessage = null)
		=> handler.VerifyInvoke(FocusOnNavigateHandler.Identifier, calledTimes, userMessage);
}
