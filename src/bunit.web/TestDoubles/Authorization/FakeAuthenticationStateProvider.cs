using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a fake implementation of AuthenticationStateProvider for testing purposes that allows
	/// user to test components that use authentication and authorization.
	/// </summary>
	public class FakeAuthenticationStateProvider : AuthenticationStateProvider
	{
		private TaskCompletionSource<AuthenticationState> _authState = new();

		/// <summary>
		/// Constructor to initialize this state provider with an initial AuthenticationState.
		/// </summary>
		/// <param name="userName">Identity's user name.</param>
		/// <param name="roles">Roles that this user principal has.</param>
		/// <param name="claims">Claims to add to user principal.</param>
		public FakeAuthenticationStateProvider(string userName, IEnumerable<string>? roles = null, IEnumerable<Claim>? claims = null)
			=> SetAuthenticatedState(userName, roles, claims);

		/// <summary>
		/// Default constructor that creates an unauthenticated state.
		/// </summary>
		public FakeAuthenticationStateProvider() => SetUnauthenticatedState();

		/// <summary>
		/// Overridden method to get the current authentication state.
		/// </summary>
		/// <returns>Current authentication state.</returns>
		public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authState.Task;

		/// <summary>
		/// Method to sets the authentication state and authenticated user.
		/// </summary>
		/// <param name="userName">Identity's user name.</param>
		/// <param name="roles">Roles that this user principal has.</param>
		/// <param name="claims">Claims to add to user principal.</param>
		public void TriggerAuthenticationStateChanged(string userName, IEnumerable<string>? roles = null, IEnumerable<Claim>? claims = null)
		{
			SetAuthenticatedState(userName, roles, claims);
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		/// <summary>
		/// Method to sets the authentication state to authorizing state.
		/// </summary>
		public void TriggerAuthorizingStateChanged()
		{
			SetAuthorizingState();
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		/// <summary>
		/// Method to sets the authentication state to unauthenticated.
		/// </summary>
		public void TriggerUnauthenticationStateChanged()
		{
			SetUnauthenticatedState();
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		private void SetUnauthenticatedState()
		{
			if (_authState.Task.IsCompleted)
				_authState = new TaskCompletionSource<AuthenticationState>();

			_authState.SetResult(CreateUnauthenticationState());
		}

		private void SetAuthorizingState()
		{
			if (_authState.Task.IsCompleted)
				_authState = new TaskCompletionSource<AuthenticationState>();
		}

		private void SetAuthenticatedState(string userName, IEnumerable<string>? roles, IEnumerable<Claim>? claims)
		{
			if (_authState.Task.IsCompleted)
				_authState = new TaskCompletionSource<AuthenticationState>();

			_authState.SetResult(CreateAuthenticationState(userName, roles, claims));
		}

		/// <summary>
		/// Factory method to create an authenticated state for a <paramref name="username"/> and set of authorization roles.
		/// </summary>
		/// <param name="username">Identity's user name.</param>
		/// <param name="roles">Roles that this user principal has.</param>
		/// <param name="claims">Claims to add to user principal.</param>
		/// <returns>Instance of AuthenticationState with user principal.</returns>
		private static AuthenticationState CreateAuthenticationState(
			string username,
			IEnumerable<string>? roles = null,
			IEnumerable<Claim>? claims = null)
		{
			var identity = new FakeIdentity { Name = username };
			var testPrincipal = new FakePrincipal { Identity = identity, Roles = roles ?? Array.Empty<string>() };
			var principal = new ClaimsPrincipal(testPrincipal);

			if (claims is not null && claims.Any())
			{
				principal.AddIdentity(new ClaimsIdentity(claims));
			}

			return new AuthenticationState(principal);
		}

		/// <summary>
		/// Factory method to create an unauthenticated state.
		/// </summary>
		/// <returns>Instance of AuthenticationState for an unauthenticated user.</returns>
		private static AuthenticationState CreateUnauthenticationState()
		{
			var principal = new ClaimsPrincipal(new FakePrincipal());
			return new AuthenticationState(principal);
		}
	}
}
