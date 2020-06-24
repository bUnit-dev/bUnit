using System.Collections.Generic;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class TestAuthorizationContextTest
	{
		[Fact(DisplayName = "Register Authorization services with unauthenticated user.")]
		public void Test003()
		{
			// arrange
			using var sp = new TestServiceProvider();

			// act
			var authContext = sp.AddTestAuthorization();

			// assert
			Assert.False(authContext.IsAuthenticated);
			Assert.Equal(AuthorizationState.Unauthorized, authContext.Authorization);
			Assert.Empty(authContext.Roles);
			Assert.Empty(authContext.Policies);
		}

		[Fact(DisplayName = "Register Authorization services with authenticated but unauthorized user.")]
		public void Test002()
		{
			// arrange
			using var sp = new TestServiceProvider();

			// act
			var authContext = sp.AddTestAuthorization();
			authContext.SetAuthorized("DarthPedro", AuthorizationState.Unauthorized);

			// assert
			Assert.True(authContext.IsAuthenticated);
			Assert.Equal(AuthorizationState.Unauthorized, authContext.Authorization);
			Assert.Empty(authContext.Roles);
			Assert.Empty(authContext.Policies);
		}

		[Fact(DisplayName = "Register Authorization services with authenticated and authorized user")]
		public void Test0010()
		{
			// arrange
			using var sp = new TestServiceProvider();

			// act
			var authContext = sp.AddTestAuthorization();
			authContext.SetAuthorized("DarthPedro");

			// assert
			Assert.True(authContext.IsAuthenticated);
			Assert.Equal(AuthorizationState.Authorized, authContext.Authorization);
			Assert.Empty(authContext.Roles);
			Assert.Empty(authContext.Policies);
		}

		[Fact(DisplayName = "Register Authorization services with authenticated and authorized user and role.")]
		public void Test001()
		{
			// arrange
			using var sp = new TestServiceProvider();

			// act
			var authContext = sp.AddTestAuthorization();
			authContext.SetAuthorized("DarthPedro");
			authContext.SetRoles("some-role");

			// assert
			Assert.True(authContext.IsAuthenticated);
			Assert.Equal(AuthorizationState.Authorized, authContext.Authorization);
			Assert.Equal(new[] { "some-role" }, authContext.Roles);
			Assert.Empty(authContext.Policies);
		}

		[Fact(DisplayName = "Register Authorization services with authenticated and authorized user and policy.")]
		public void Test0011()
		{
			// arrange
			using var sp = new TestServiceProvider();

			// act
			var authContext = sp.AddTestAuthorization();
			authContext.SetAuthorized("DarthPedro");
			authContext.SetPolicies("TestPolicy", "Other");

			// assert
			Assert.True(authContext.IsAuthenticated);
			Assert.Equal(AuthorizationState.Authorized, authContext.Authorization);
			Assert.Equal(new[] { "TestPolicy", "Other" }, authContext.Policies);
			Assert.Empty(authContext.Roles);
		}
	}
}
