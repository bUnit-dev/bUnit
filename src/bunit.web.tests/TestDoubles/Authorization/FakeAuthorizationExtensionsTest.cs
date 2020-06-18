using System.Collections.Generic;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class FakeAuthorizationExtensionsTest
	{
		[Fact(DisplayName = "Register Authorization services with authenticated and authorized user.")]
		public void Test001()
		{
			// arrange
			using var sp = new TestServiceProvider();
			var roles = new List<string> { "User" };

			// act
			var authContext = sp.AddTestAuthorization();
			authContext.Authenticate("DarthPedro", AuthorizationState.Authorized, roles);

			// assert
			Assert.True(authContext.IsAuthenticated);
			Assert.Equal(AuthorizationState.Authorized, authContext.Authorization);
			Assert.Equal(roles, authContext.Roles);
		}

		[Fact(DisplayName = "Register Authorization services with authenticated but unauthorized user.")]
		public void Test002()
		{
			// arrange
			using var sp = new TestServiceProvider();
			var principal = TestAuthorizationContext.CreatePrincipal("DarthPedro");

			// act
			var authContext = sp.AddTestAuthorization();
			authContext.Authenticate("DarthPedro", AuthorizationState.Unauthorized);

			// assert
			Assert.True(authContext.IsAuthenticated);
			Assert.Equal(AuthorizationState.Unauthorized, authContext.Authorization);
			Assert.Null(authContext.Roles);
		}

		[Fact(DisplayName = "Register Authorization services with unauthenticated user.")]
		public void Test003()
		{
			// arrange
			using var sp = new TestServiceProvider();
			var principal = TestAuthorizationContext.CreatePrincipal("DarthPedro");

			// act
			var authContext = sp.AddTestAuthorization();

			// assert
			Assert.False(authContext.IsAuthenticated);
			Assert.Equal(AuthorizationState.Unauthorized, authContext.Authorization);
			Assert.Null(authContext.Roles);
		}
	}
}
