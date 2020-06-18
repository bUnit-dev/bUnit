using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class FakeAuthorizationServiceTest
	{
		[Fact(DisplayName = "Get default authorization result from AuthorizationService.")]
		public void Test001()
		{
			// arrange
			var service = new FakeAuthorizationService();

			// act
			var result = service.CurrentState;

			// assert
			Assert.Equal(AuthorizationState.Authorized, result);
		}

		[Fact(DisplayName = "Get AuthorizeAsync with an authorized result.")]
		public async Task Test002()
		{
			// arrange
			var service = new FakeAuthorizationService(AuthorizationState.Unauthorized);
			var user = new ClaimsPrincipal(new FakeIdentity { Name = "DarthPedro" });
			var requirements = new List<IAuthorizationRequirement>();

			// act
			var result = await service.AuthorizeAsync(user, "testResource", requirements).ConfigureAwait(false);

			// assert
			Assert.NotNull(result);
			Assert.False(result.Succeeded);
		}

		[Fact(DisplayName = "Get AuthorizeAsync with an authorized result.")]
		public async Task Test003()
		{
			// arrange
			var service = new FakeAuthorizationService();
			var user = new ClaimsPrincipal(new FakeIdentity { Name = "DarthPedro" });
			var requirements = new List<IAuthorizationRequirement>();

			// act
			var result = await service.AuthorizeAsync(user, "testResource", requirements).ConfigureAwait(false);

			// assert
			Assert.NotNull(result);
			Assert.True(result.Succeeded);
		}

		[Fact(DisplayName = "Get AuthorizeAsync with policy name.")]
		public async Task Test004()
		{
			// arrange
			var service = new FakeAuthorizationService(AuthorizationState.Unauthorized);
			var user = new ClaimsPrincipal(new FakeIdentity { Name = "DarthPedro" });

			// act
			var result = await service.AuthorizeAsync(user, "testResource", "testPolicy").ConfigureAwait(false);

			// assert
			Assert.NotNull(result);
			Assert.False(result.Succeeded);
		}

		[Fact(DisplayName = "Get exception from invoking placeholder AuthorizationService methods.")]
		public async Task Test005()
		{
			// arrange
			using var ctx = new TestContext();
			var authService = ctx.Services.GetRequiredService<IAuthorizationService>();
			IEnumerable<IAuthorizationRequirement>? requirements = null;

			// act
			var ex = await Assert.ThrowsAsync<MissingFakeAuthorizationException>(() =>
				authService.AuthorizeAsync(null, string.Empty, requirements));

			// assert
			Assert.Equal("IAuthorizationService", ex.ServiceName);
		}

		[Fact(DisplayName = "Get exception from invoking placeholder AuthorizationService methods.")]
		public async Task Test006()
		{
			// arrange
			using var ctx = new TestContext();
			var authService = ctx.Services.GetRequiredService<IAuthorizationService>();

			// act
			var ex = await Assert.ThrowsAsync<MissingFakeAuthorizationException>(() =>
				authService.AuthorizeAsync(null, string.Empty, "testPolicy"));

			// assert
			Assert.Equal("IAuthorizationService", ex.ServiceName);
		}
	}
}
