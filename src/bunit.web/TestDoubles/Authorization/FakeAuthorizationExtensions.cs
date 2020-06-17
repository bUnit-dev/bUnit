using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// Helper methods for registering the Authentication/Authorization services with a <see cref="TestServiceProvider"/>.
	/// </summary>
	public static class FakeAuthorizationExtensions
	{
		/// <summary>
		/// Adds the appropriate auth services to the <see cref="TestServiceProvider"/> to enable an authenticated user.
		/// </summary>
		public static void AddTestAuthorization(
			this TestServiceProvider serviceProvider,
			string userName,
			AuthorizationState authorizationState = AuthorizationState.Authorized,
			IList<string>? roles = null)
		{
			var authService = new FakeAuthorizationService()
			{
				CurrentState = authorizationState
			};

			serviceProvider.AddSingleton<IAuthorizationService>(authService);
			serviceProvider.AddSingleton<IAuthorizationPolicyProvider>(new FakeAuthorizationPolicyProvider());
			serviceProvider.AddSingleton<AuthenticationStateProvider>(new FakeAuthenticationStateProvider(userName, roles));
		}

		/// <summary>
		/// Adds the appropriate auth services to the <see cref="TestServiceProvider"/> to enable an authenticated user.
		/// </summary>
		public static void AddTestAuthorization(this TestServiceProvider serviceProvider)
		{
			var authService = new FakeAuthorizationService()
			{
				CurrentState = AuthorizationState.Unauthorized
			};

			serviceProvider.AddSingleton<IAuthorizationService>(authService);
			serviceProvider.AddSingleton<IAuthorizationPolicyProvider>(new FakeAuthorizationPolicyProvider());
			serviceProvider.AddSingleton<AuthenticationStateProvider>(new FakeAuthenticationStateProvider());
		}

		/// <summary>
		/// Sets an authenticated user in the <see cref="AuthenticationStateProvider"/>.
		/// </summary>
		public static void SetAuthenticated(this AuthenticationStateProvider authProvider, string userName, IList<string>? roles = null)
		{
			_ = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
			var testProvider = (FakeAuthenticationStateProvider)authProvider;

			testProvider.TriggerAuthenticationStateChanged(userName, roles);
		}

		/// <summary>
		/// Sets the authorization state for the user in the <see cref="IAuthorizationService"/>.
		/// </summary>
		public static void SetAuthorizationState(this IAuthorizationService authService, AuthorizationState state)
		{
			_ = authService ?? throw new ArgumentNullException(nameof(authService));
			var testService = (FakeAuthorizationService)authService;

			testService.CurrentState = state;
		}
	}
}
