using Bunit.TestDoubles;
using System;
using Xunit;

namespace Bunit.Docs.Samples
{
    public class InjectAuthServiceTest
    {
		[Fact(DisplayName = "Use AuthenticationStateProvider service with authenticated and authorized user")]
		public void Test001()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUserName", AuthorizationState.Authorized);

			// act
			var cut = ctx.RenderComponent<InjectAuthService>();

			// assert
			Assert.Contains("<p>User: TestUserName</p>", cut.Markup, StringComparison.InvariantCulture);
		}

		[Fact(DisplayName = "Use AuthenticationStateProvider service with unauthenticated and unauthorized user")]
		public void Test002()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();

			// act
			var cut = ctx.RenderComponent<InjectAuthService>();

			// assert
			Assert.DoesNotContain("User:", cut.Markup, StringComparison.InvariantCulture);
		}
	}
}
