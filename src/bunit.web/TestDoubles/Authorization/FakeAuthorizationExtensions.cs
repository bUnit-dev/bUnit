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
		public static void AddAuthorization(
			this TestServiceProvider serviceProvider,
			string userName,
			AuthorizationState authorizationState = AuthorizationState.Authorized,
			IList<string>? roles = null)
		{
			var authService = new FakeAuthorizationService()
			{
				NextResult = (authorizationState == AuthorizationState.Authorized) ? AuthorizationResult.Success() : AuthorizationResult.Failed(),
			};

			serviceProvider.AddSingleton<IAuthorizationService>(authService);
			serviceProvider.AddSingleton<IAuthorizationPolicyProvider>(new FakeAuthorizationPolicyProvider());
			serviceProvider.AddSingleton<AuthenticationStateProvider>(new FakeAuthenticationStateProvider(userName, roles));
		}

		/// <summary>
		/// Adds the appropriate auth services to the <see cref="TestServiceProvider"/> to enable an authenticated user.
		/// </summary>
		public static void AddAuthorization(this TestServiceProvider serviceProvider)
		{
			var authService = new FakeAuthorizationService()
			{
				NextResult = AuthorizationResult.Failed()
			};

			serviceProvider.AddSingleton<IAuthorizationService>(authService);
			serviceProvider.AddSingleton<IAuthorizationPolicyProvider>(new FakeAuthorizationPolicyProvider());
			serviceProvider.AddSingleton<AuthenticationStateProvider>(new FakeAuthenticationStateProvider());
		}
	}
}
