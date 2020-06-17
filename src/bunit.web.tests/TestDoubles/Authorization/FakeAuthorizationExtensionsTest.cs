using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class FakeAuthorizationExtensionsTest
	{
		[Fact(DisplayName = "Register Authorization services with authenticated and authorized user.")]
		public async Task Test001()
		{
			// arrange
			using var sp = new TestServiceProvider();
			var roles = new List<string> { "User" };
			var principal = new ClaimsPrincipal(
				new FakePrincipal { Identity = new FakeIdentity { Name = "DarthPedro" }, Roles = roles });

			// act
			sp.AddTestAuthorization("DarthPedro", AuthorizationState.Authorized, roles);

			var authProvider = sp.GetRequiredService<AuthenticationStateProvider>();
			var authState = await authProvider.GetAuthenticationStateAsync();

			var authService = sp.GetRequiredService<IAuthorizationService>();
			var authResult = await authService.AuthorizeAsync(principal, "testResource", "testPolicy");

			// assert
			Assert.NotNull(authState.User);
			Assert.NotNull(authState.User.Identity);
			Assert.Equal("DarthPedro", authState.User.Identity.Name);
			Assert.True(authState.User.Identity.IsAuthenticated);
			Assert.True(authResult.Succeeded);
		}

		[Fact(DisplayName = "Register Authorization services with authenticated but unauthorized user.")]
		public async Task Test002()
		{
			// arrange
			using var sp = new TestServiceProvider();
			var principal = new ClaimsPrincipal(
				new FakePrincipal { Identity = new FakeIdentity { Name = "DarthPedro" } });

			// act
			sp.AddTestAuthorization("DarthPedro", AuthorizationState.Unauthorized);

			var authProvider = sp.GetRequiredService<AuthenticationStateProvider>();
			var authState = await authProvider.GetAuthenticationStateAsync();

			var authService = sp.GetRequiredService<IAuthorizationService>();
			var authResult = await authService.AuthorizeAsync(principal, "testResource", "testPolicy");

			// assert
			Assert.NotNull(authState.User);
			Assert.NotNull(authState.User.Identity);
			Assert.Equal("DarthPedro", authState.User.Identity.Name);
			Assert.True(authState.User.Identity.IsAuthenticated);
			Assert.False(authResult.Succeeded);
		}

		[Fact(DisplayName = "Register Authorization services with unauthenticated user.")]
		public async Task Test003()
		{
			// arrange
			using var sp = new TestServiceProvider();
			var principal = new ClaimsPrincipal(
				new FakePrincipal { Identity = new FakeIdentity { Name = "DarthPedro" } });

			// act
			sp.AddTestAuthorization();

			var authProvider = sp.GetRequiredService<AuthenticationStateProvider>();
			var authState = await authProvider.GetAuthenticationStateAsync();

			var authService = sp.GetRequiredService<IAuthorizationService>();
			var authResult = await authService.AuthorizeAsync(principal, "testResource", "testPolicy");

			// assert
			Assert.NotNull(authState.User);
			Assert.NotNull(authState.User.Identity);
			Assert.Null(authState.User.Identity.Name);
			Assert.False(authState.User.Identity.IsAuthenticated);
			Assert.False(authResult.Succeeded);
		}
	}
}
