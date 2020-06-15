using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// Test class that provides an implementation of AuthenticationStateProvider that allows
	/// user to test components that use authentication and authorization.
	/// </summary>
	public class FakeAuthenticationStateProvider : AuthenticationStateProvider
	{
		/// <summary>
		/// Constructor to initialize this state provider with an initial AuthenticationState.
		/// </summary>
		/// <param name="username">Identity's user name.</param>
		/// <param name="roles">Roles that this user principal has.</param>
		public FakeAuthenticationStateProvider(string username, IList<string>? roles = null)
		{
			this.CurrentAuthStateTask = FakeAuthenticationStateProvider.CreateAuthenticationState(username, roles);
		}

		/// <summary>
		/// Default constructor that creates an unauthenticated state.
		/// </summary>
		public FakeAuthenticationStateProvider()
		{
			this.CurrentAuthStateTask = FakeAuthenticationStateProvider.CreateUnauthenticationState();
		}

		/// <summary>
		/// Gets or sets the current authentication state.
		/// </summary>
		protected Task<AuthenticationState> CurrentAuthStateTask { get; set; }

		/// <summary>
		/// Overidden method to get the current authentication state.
		/// </summary>
		/// <returns>Current authentication state.</returns>
		public override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			return this.CurrentAuthStateTask;
		}

		/// <summary>
		/// Overridden method to change the authenticaiton state.
		/// </summary>
		/// <param name="authState">New authentication state.</param>
		public void TriggerAuthenticationStateChanged(Task<AuthenticationState> authState)
		{
			this.CurrentAuthStateTask = authState;
			this.NotifyAuthenticationStateChanged(authState);
		}

		/// <summary>
		/// Factory method to create an authenticated state for a username and set of authorization roles.
		/// </summary>
		/// <param name="username">Identity's user name.</param>
		/// <param name="roles">Roles that this user principal has.</param>
		/// <returns>Instance of AuthenticationState with user principal.</returns>
		public static Task<AuthenticationState> CreateAuthenticationState(string username, IList<string>? roles = null)
		{
			var identity = new FakeIdentity { Name = username };
			var testPrincipal = new FakePrincipal { Identity = identity, Roles = roles };
			var principal = new ClaimsPrincipal(testPrincipal);

			return Task.FromResult(new AuthenticationState(principal));
		}

		/// <summary>
		/// Factory method to create an unauthenticated state.
		/// </summary>
		/// <returns>Instance of AuthenticationState for an unauthenticated user.</returns>
		public static Task<AuthenticationState> CreateUnauthenticationState()
		{
			var principal = new ClaimsPrincipal(new FakePrincipal());
			return Task.FromResult(new AuthenticationState(principal));
		}
	}
}
