using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Bunit.TestDoubles;

/// <summary>
/// Helper methods for registering the Authentication/Authorization services with
/// a <see cref="TestServiceProvider"/>.
/// </summary>
public static class FakeAuthorizationExtensions
{
	/// <summary>
	/// Adds the appropriate Blazor authentication and authorization services to the <see cref="TestServiceProvider"/> to enable
	/// an authenticated user, as well as adding the <see cref="CascadingAuthenticationState"/> component to the
	/// test contexts render tree.
	/// </summary>
	public static TestAuthorizationContext AddTestAuthorization(this TestContextBase context)
	{
		if (context is null)
			throw new ArgumentNullException(nameof(context));

		context.RenderTree.TryAdd<CascadingAuthenticationState>();
		context.Services.AddSingleton<FakeSignOutSessionStateManager>();
		context.Services.AddSingleton<SignOutSessionStateManager>(s => s.GetRequiredService<FakeSignOutSessionStateManager>());
		var authCtx = new TestAuthorizationContext();
		authCtx.SetNotAuthorized();
		authCtx.RegisterAuthorizationServices(context.Services);
		return authCtx;
	}
}
