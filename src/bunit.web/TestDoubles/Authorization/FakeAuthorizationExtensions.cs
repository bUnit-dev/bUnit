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
			var authService = new FakeAuthorizationService(authorizationState);

			serviceProvider.AddSingleton<IAuthorizationService>(authService);
			serviceProvider.AddSingleton<IAuthorizationPolicyProvider>(new FakeAuthorizationPolicyProvider());
			serviceProvider.AddSingleton<AuthenticationStateProvider>(new FakeAuthenticationStateProvider(userName, roles));
		}

		/// <summary>
		/// Adds the appropriate auth services to the <see cref="TestServiceProvider"/> to enable an authenticated user.
		/// </summary>
		public static void AddTestAuthorization(this TestServiceProvider serviceProvider)
		{
			var authService = new FakeAuthorizationService(AuthorizationState.Unauthorized);

			serviceProvider.AddSingleton<IAuthorizationService>(authService);
			serviceProvider.AddSingleton<IAuthorizationPolicyProvider>(new FakeAuthorizationPolicyProvider());
			serviceProvider.AddSingleton<AuthenticationStateProvider>(new FakeAuthenticationStateProvider());
		}

		/// <summary>
		/// Updates the auth services to change from an unauthenticated to authenticated user.
		/// </summary>
		public static void UpdateTestAuthorizationState(
			this TestServiceProvider serviceProvider,
			string userName,
			AuthorizationState authorizationState = AuthorizationState.Authorized,
			IList<string>? roles = null)
		{
			var authProvider = serviceProvider.GetRequiredService<AuthenticationStateProvider>();
			var testProvider = (FakeAuthenticationStateProvider)authProvider;

			testProvider.TriggerAuthenticationStateChanged(userName, roles);


			var authService = serviceProvider.GetRequiredService<IAuthorizationService>();
			var testService = (FakeAuthorizationService)authService;

			testService.SetAuthorizationState(authorizationState);
		}

		/// <summary>
		/// Updates the auth services to change from an authenticated to unauthenticated user.
		/// </summary>
		public static void UpdateTestAuthorizationState(this TestServiceProvider serviceProvider)
		{
			var authProvider = serviceProvider.GetRequiredService<AuthenticationStateProvider>();
			var testProvider = (FakeAuthenticationStateProvider)authProvider;

			testProvider.TriggerAuthenticationStateChanged();

			var authService = serviceProvider.GetRequiredService<IAuthorizationService>();
			var testService = (FakeAuthorizationService)authService;

			testService.SetAuthorizationState(AuthorizationState.Unauthorized);
		}
	}
}
