using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bunit;

public partial class BunitContext
{
	/// <summary>
	/// Adds the appropriate Blazor authentication and authorization services to the <see cref="BunitServiceProvider"/> to enable
	/// an authenticated user, as well as adding the <see cref="CascadingAuthenticationState"/> component to the
	/// test contexts render tree.
	/// </summary>
	public BunitAuthorizationContext AddAuthorization()
	{
		Services.AddCascadingAuthenticationState();
		var authCtx = new BunitAuthorizationContext(Services);
		return authCtx;
	}
}
