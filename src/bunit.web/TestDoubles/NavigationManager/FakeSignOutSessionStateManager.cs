using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Bunit.TestDoubles;

/// <summary>
/// Represents a fake <see cref="SignOutSessionStateManager"/> that captures calls to
/// </summary>
public class FakeSignOutSessionStateManager : SignOutSessionStateManager
{
	private readonly BunitJSInterop jsInterop;

	/// <summary>
	/// Initializes a new instance of <see cref="FakeSignOutSessionStateManager"/>
	/// </summary>
	public FakeSignOutSessionStateManager(IJSRuntime jsRuntime, BunitJSInterop jsInterop) : base(jsRuntime)
	{
		_ = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
		this.jsInterop = jsInterop ?? throw new ArgumentNullException(nameof(jsInterop));
		InitializeInvocations();
	}

	/// <summary>
	/// Returns true when the user was signed out otherwise false
	/// </summary>
	public bool SignOutStateWasCalled =>
		jsInterop.Invocations.Any(i =>
			WasSessionStorageCalled(i) &&
			WasSignOutStateSet(i));

	private static bool WasSignOutStateSet(JSRuntimeInvocation i) => i.Arguments.Any(a => string.Equals(a?.ToString(), "Microsoft.AspNetCore.Components.WebAssembly.Authentication.SignOutState", StringComparison.Ordinal));

	private static bool WasSessionStorageCalled(JSRuntimeInvocation invocation) => string.Equals(invocation.Identifier, "sessionStorage.setItem", StringComparison.Ordinal);

	private void InitializeInvocations()
	{
		jsInterop.SetupVoid(
			"sessionStorage.setItem",
			inv => Equals(inv.Arguments.FirstOrDefault(),
				"Microsoft.AspNetCore.Components.WebAssembly.Authentication.SignOutState")
		).SetVoidResult();
	}
}
