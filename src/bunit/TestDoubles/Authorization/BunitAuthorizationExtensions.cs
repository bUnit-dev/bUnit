using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Bunit;

public abstract partial class TestContextBase
{
	/// <summary>
	/// Adds the appropriate Blazor authentication and authorization services to the <see cref="TestServiceProvider"/> to enable
	/// an authenticated user, as well as adding the <see cref="CascadingAuthenticationState"/> component to the
	/// test contexts render tree.
	/// </summary>
	public BunitAuthorizationContext AddAuthorization()
	{
		RenderTree.TryAdd<CascadingAuthenticationState>();
		Services.AddSingleton<BunitSignOutSessionStateManager>();
#pragma warning disable CS0618
		Services.AddSingleton<SignOutSessionStateManager>(s => s.GetRequiredService<BunitSignOutSessionStateManager>());
#pragma warning restore CS0618
		var authCtx = new BunitAuthorizationContext();
		authCtx.SetNotAuthorized();
		authCtx.RegisterAuthorizationServices(Services);
		return authCtx;
	}
}
