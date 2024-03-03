using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bunit.TestDoubles;

/// <summary>
/// Root authorization service that manages different authentication/authorization state
/// in the system.
/// </summary>
public class BunitAuthorizationContext
{
	private readonly BunitAuthorizationService authService = new();
	private readonly BunitAuthorizationPolicyProvider policyProvider = new();
	private readonly BunitAuthenticationStateProvider authProvider = new();

	/// <summary>
	/// Gets a value indicating whether user is authenticated.
	/// </summary>
	public bool IsAuthenticated { get; private set; }

	/// <summary>
	/// Gets the authorization context user name.
	/// </summary>
	public string UserName { get; private set; } = string.Empty;

	/// <summary>
	/// Gets the authorization state for the user.
	/// </summary>
	public AuthorizationState State { get; private set; } = AuthorizationState.Unauthorized;

	/// <summary>
	/// Gets the set of roles for the current user.
	/// </summary>
	public IEnumerable<string> Roles { get; private set; } = Array.Empty<string>();

	/// <summary>
	/// Gets the set of authentication policies for the current user.
	/// </summary>
	public IEnumerable<string> Policies { get; private set; } = Array.Empty<string>();

	/// <summary>
	/// Gets the set of principal claims for the current user.
	/// </summary>
	public IEnumerable<Claim> Claims { get; private set; } = Array.Empty<Claim>();

	/// <summary>
	/// Gets or sets the policy schemeName. Defaults to TestScheme and usually doesn't
	/// need to be changed by user.
	/// </summary>
	public string PolicySchemeName { get; set; } = "TestScheme";

	/// <summary>
	/// Creates a new instance of <see cref="BunitAuthorizationContext"/>.
	/// </summary>
	internal BunitAuthorizationContext(TestServiceProvider services)
	{
		services.AddSingleton<IAuthorizationService>(authService);
		services.AddSingleton<IAuthorizationPolicyProvider>(policyProvider);
		services.AddSingleton<AuthenticationStateProvider>(authProvider);

		authService.SetAuthorizationState(AuthorizationState.Unauthorized);
	}

	/// <summary>
	/// Authenticates the user with specified name and authorization state.
	/// </summary>
	/// <param name="userName">User name for the principal identity.</param>
	/// <param name="state">Authorization state.</param>
	public BunitAuthorizationContext SetAuthorized(string userName, AuthorizationState state = AuthorizationState.Authorized)
	{
		IsAuthenticated = true;
		UserName = userName;

		authProvider.TriggerAuthenticationStateChanged(userName, Roles, Claims);

		State = state;
		authService.SetAuthorizationState(state);
		authService.SetRoles(Roles);

		return this;
	}

	/// <summary>
	/// Puts the authorization services into the authorizing state.
	/// </summary>
	public BunitAuthorizationContext SetAuthorizing()
	{
		IsAuthenticated = false;
		Roles = Array.Empty<string>();

		authProvider.TriggerAuthorizingStateChanged();

		State = AuthorizationState.Authorizing;
		authService.SetAuthorizationState(AuthorizationState.Authorizing);

		return this;
	}

	/// <summary>
	/// Puts the authorization services into an unauthenticated and unauthorized state.
	/// </summary>
	public BunitAuthorizationContext SetNotAuthorized()
	{
		IsAuthenticated = false;
		Roles = Array.Empty<string>();

		authProvider.TriggerUnauthenticationStateChanged();

		State = AuthorizationState.Unauthorized;
		authService.SetAuthorizationState(AuthorizationState.Unauthorized);

		return this;
	}

	/// <summary>
	/// Sets the user roles in this context..
	/// </summary>
	/// <param name="roles">Roles for the claims principal.</param>
	public BunitAuthorizationContext SetRoles(params string[] roles)
	{
		Roles = roles;
		authService.SetRoles(Roles);
		authProvider.TriggerAuthenticationStateChanged(UserName, Roles);

		return this;
	}

	/// <summary>
	/// Sets the authorization policies supported for the current user.
	/// </summary>
	/// <param name="policies">Supported authorization policies.</param>
	public BunitAuthorizationContext SetPolicies(params string[] policies)
	{
		Policies = policies;
		policyProvider.SetPolicyScheme(PolicySchemeName);
		authService.SetPolicies(Policies);

		return this;
	}

	/// <summary>
	/// Sets the claims on the current user/principal.
	/// </summary>
	/// <param name="claims">Claims to set.</param>
	public BunitAuthorizationContext SetClaims(params Claim[] claims)
	{
		Claims = claims;
		authProvider.TriggerAuthenticationStateChanged(UserName, Roles, Claims);

		return this;
	}

	/// <summary>
	/// Sets the Identity.AuthenticationType for the current user/principa;.
	/// </summary>
	/// <param name="authenticationType">The authentication type to set.</param>
	public BunitAuthorizationContext SetAuthenticationType(string authenticationType)
	{
		this.authProvider.TriggerAuthenticationStateChanged(this.UserName, this.Roles, this.Claims, authenticationType);
		return this;
	}
}
