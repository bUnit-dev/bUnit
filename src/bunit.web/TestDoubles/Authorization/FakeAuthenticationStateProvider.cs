using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// Represents a fake implementation of AuthenticationStateProvider for testing purposes that allows
	/// user to test components that use authentication and authorization.
	/// </summary>
	public class FakeAuthenticationStateProvider : AuthenticationStateProvider
	{
		/// <summary>
		/// Constructor to initialize this state provider with an initial AuthenticationState.
		/// </summary>
		/// <param name="userName">Identity's user name.</param>
		/// <param name="roles">Roles that this user principal has.</param>
		public FakeAuthenticationStateProvider(string userName, IEnumerable<string>? roles = null)
		{
			CurrentAuthStateTask = FakeAuthenticationStateProvider.CreateAuthenticationState(userName, roles);
		}

		/// <summary>
		/// Default constructor that creates an unauthenticated state.
		/// </summary>
		public FakeAuthenticationStateProvider()
		{
			CurrentAuthStateTask = FakeAuthenticationStateProvider.CreateUnauthenticationState();
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
			return CurrentAuthStateTask;
		}

		/// <summary>
		/// Method to change the authenticaiton state and authenticated user.
		/// </summary>
		/// <param name="userName">Identity's user name.</param>
		/// <param name="roles">Roles that this user principal has.</param>
		public void TriggerAuthenticationStateChanged(string userName, IEnumerable<string>? roles = null)
		{
			CurrentAuthStateTask = FakeAuthenticationStateProvider.CreateAuthenticationState(userName, roles);
			NotifyAuthenticationStateChanged(CurrentAuthStateTask);
		}

		/// <summary>
		/// Method to change the authenticaiton state to unauthenticated.
		/// </summary>
		public void TriggerAuthenticationStateChanged()
		{
			CurrentAuthStateTask = FakeAuthenticationStateProvider.CreateUnauthenticationState();
			NotifyAuthenticationStateChanged(CurrentAuthStateTask);
		}

		/// <summary>
		/// Factory method to create an authenticated state for a username and set of authorization roles.
		/// </summary>
		/// <param name="username">Identity's user name.</param>
		/// <param name="roles">Roles that this user principal has.</param>
		/// <returns>Instance of AuthenticationState with user principal.</returns>
		public static Task<AuthenticationState> CreateAuthenticationState(string username, IEnumerable<string>? roles = null)
		{
			var identity = new FakeIdentity { Name = username };
			var testPrincipal = new FakePrincipal { Identity = identity, Roles = roles ?? Array.Empty<string>() };
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
