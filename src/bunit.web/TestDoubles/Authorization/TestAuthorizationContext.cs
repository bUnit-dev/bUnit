using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Root authorization service that manages different authentication/authorization state
	/// in the system.
	/// </summary>
	public class TestAuthorizationContext
	{
		private readonly FakeAuthorizationService _authService = new();
		private readonly FakeAuthorizationPolicyProvider _policyProvider = new();
		private readonly FakeAuthenticationStateProvider _authProvider = new();

		/// <summary>
		/// Gets whether user is authenticated.
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
		/// Registers authorization services with the specified service provider.
		/// </summary>
		/// <param name="services">Service provider to use.</param>
		public void RegisterAuthorizationServices(IServiceCollection services)
		{
			services.AddSingleton<IAuthorizationService>(_authService);
			services.AddSingleton<IAuthorizationPolicyProvider>(_policyProvider);
			services.AddSingleton<AuthenticationStateProvider>(_authProvider);
		}

		/// <summary>
		/// Authenticates the user with specified name and authorization state.
		/// </summary>
		/// <param name="userName">User name for the principal identity.</param>
		/// <param name="state">Authorization state.</param>
		public TestAuthorizationContext SetAuthorized(string userName, AuthorizationState state = AuthorizationState.Authorized)
		{
			IsAuthenticated = true;
			UserName = userName;

			_authProvider.TriggerAuthenticationStateChanged(userName, Roles, Claims);

			State = state;
			_authService.SetAuthorizationState(state);
			_authService.SetRoles(Roles);

			return this;
		}

		/// <summary>
		/// Puts the authorization services into the authorizing state.
		/// </summary>
		public TestAuthorizationContext SetAuthorizing()
		{
			IsAuthenticated = false;
			Roles = Array.Empty<string>();

			_authProvider.TriggerAuthorizingStateChanged();

			State = AuthorizationState.Authorizing;
			_authService.SetAuthorizationState(AuthorizationState.Authorizing);

			return this;
		}

		/// <summary>
		/// Puts the authorization services into an unauthenticated and unauthorized state.
		/// </summary>
		public TestAuthorizationContext SetNotAuthorized()
		{
			IsAuthenticated = false;
			Roles = Array.Empty<string>();

			_authProvider.TriggerUnauthenticationStateChanged();

			State = AuthorizationState.Unauthorized;
			_authService.SetAuthorizationState(AuthorizationState.Unauthorized);

			return this;
		}

		/// <summary>
		/// Sets the user roles in this context..
		/// </summary>
		/// <param name="roles">Roles for the claims principal.</param>
		public TestAuthorizationContext SetRoles(params string[] roles)
		{
			Roles = roles ?? Array.Empty<string>();
			_authService.SetRoles(Roles);
			_authProvider.TriggerAuthenticationStateChanged(UserName, Roles);

			return this;
		}

		/// <summary>
		/// Sets the authorization policies supported for the current user.
		/// </summary>
		/// <param name="policies">Supported authorization policies.</param>
		public TestAuthorizationContext SetPolicies(params string[] policies)
		{
			Policies = policies ?? Array.Empty<string>();
			_policyProvider.SetPolicyScheme(PolicySchemeName);
			_authService.SetPolicies(Policies);

			return this;
		}

		/// <summary>
		/// Sets the claims on the curren user/principal.
		/// </summary>
		/// <param name="claims">Claims to set.</param>
		public TestAuthorizationContext SetClaims(params Claim[] claims)
		{
			Claims = claims ?? Array.Empty<Claim>();
			_authProvider.TriggerAuthenticationStateChanged(UserName, Roles, Claims);

			return this;
		}
	}
}
