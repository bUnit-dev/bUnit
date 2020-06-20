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

			// act
			var authContext = sp.AddTestAuthorization();
			authContext.SetAuthorized("DarthPedro", AuthorizationState.Authorized, "User");

			// assert
			Assert.True(authContext.IsAuthenticated);
			Assert.Equal(AuthorizationState.Authorized, authContext.Authorization);
			Assert.Equal(new List<string> { "User" }, authContext.Roles);
		}

		[Fact(DisplayName = "Register Authorization services with authenticated but unauthorized user.")]
		public void Test002()
		{
			// arrange
			using var sp = new TestServiceProvider();
			var principal = TestAuthorizationContext.CreatePrincipal("DarthPedro");

			// act
			var authContext = sp.AddTestAuthorization();
			authContext.SetAuthorized("DarthPedro", AuthorizationState.Unauthorized);

			// assert
			Assert.True(authContext.IsAuthenticated);
			Assert.Equal(AuthorizationState.Unauthorized, authContext.Authorization);
			Assert.Empty(authContext.Roles);
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
			Assert.Empty(authContext.Roles);
		}
	}
}
