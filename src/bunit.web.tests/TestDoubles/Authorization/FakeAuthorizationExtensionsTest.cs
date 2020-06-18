using System.Collections.Generic;
using System.Threading.Tasks;
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
			var authAdaptor = sp.AddTestAuthorization();
			authAdaptor.Authenticate("DarthPedro", AuthorizationState.Authorized, roles);

			// assert
			Assert.True(authAdaptor.IsAuthenticated);
			Assert.Equal(AuthorizationState.Authorized, authAdaptor.Authorization);
			Assert.Equal(roles, authAdaptor.Roles);
		}

		[Fact(DisplayName = "Register Authorization services with authenticated but unauthorized user.")]
		public void Test002()
		{
			// arrange
			using var sp = new TestServiceProvider();
			var principal = FakePrincipal.CreatePrincipal("DarthPedro");

			// act
			var authAdaptor = sp.AddTestAuthorization();
			authAdaptor.Authenticate("DarthPedro", AuthorizationState.Unauthorized);

			// assert
			Assert.True(authAdaptor.IsAuthenticated);
			Assert.Equal(AuthorizationState.Unauthorized, authAdaptor.Authorization);
			Assert.Null(authAdaptor.Roles);
		}

		[Fact(DisplayName = "Register Authorization services with unauthenticated user.")]
		public void Test003()
		{
			// arrange
			using var sp = new TestServiceProvider();
			var principal = FakePrincipal.CreatePrincipal("DarthPedro");

			// act
			var authAdaptor = sp.AddTestAuthorization();

			// assert
			Assert.False(authAdaptor.IsAuthenticated);
			Assert.Equal(AuthorizationState.Unauthorized, authAdaptor.Authorization);
			Assert.Null(authAdaptor.Roles);
		}
	}
}
