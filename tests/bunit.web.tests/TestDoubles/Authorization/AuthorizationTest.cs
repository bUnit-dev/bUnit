using System;
using System.Security.Claims;
using Bunit.TestAssets.SampleComponents;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class AuthorizationTest
	{
		[Fact(DisplayName = "AuthorizeView with unauthenticated user")]
		public void Test001()
		{
			// Arrange
			using var ctx = new TestContext();
			ctx.Services.AddTestAuthorization();

			// Act
			var cut = ctx.RenderComponent<SimpleAuthView>();

			// Assert
			cut.MarkupMatches("Not authorized?");
		}

		[Fact(DisplayName = "AuthorizeView with authenticated and authorized user")]
		public void Test002()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser", AuthorizationState.Authorized);

			// act
			var cut = ctx.RenderComponent<SimpleAuthView>();

			// assert
			cut.MarkupMatches("Authorized!");
		}

		[Fact(DisplayName = "AuthorizeView with authenticated but unauthorized user")]
		public void Test003()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser", AuthorizationState.Unauthorized);

			// act
			var cut = ctx.RenderComponent<SimpleAuthView>();

			// assert
			cut.MarkupMatches("Not authorized?");
		}

		[Fact(DisplayName = "AuthorizeView switch from unauthorized to authorized.")]
		public void Test004()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();

			// start off unauthenticated.
			var cut = ctx.RenderComponent<SimpleAuthView>();
			cut.MarkupMatches("Not authorized?");

			// act
			authContext.SetAuthorized("TestUser004", AuthorizationState.Authorized);

			cut.Render();

			// assert
			cut.WaitForAssertion(() => cut.MarkupMatches("Authorized!"));
		}

		[Fact(DisplayName = "AuthorizeView switch from authorized to unauthorized.")]
		public void Test005()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser005", AuthorizationState.Authorized);

			// start off unauthenticated.
			var cut = ctx.RenderComponent<SimpleAuthView>();
			cut.MarkupMatches("Authorized!");

			// act
			authContext.SetNotAuthorized();

			cut.Render();

			// assert
			cut.MarkupMatches("Not authorized?");
		}

		[Fact(DisplayName = "AuthorizeView rendering without authorization services registered")]
		public void Test006()
		{
			// arrange
			using var ctx = new TestContext();

			// act
			var ex = Assert.Throws<MissingFakeAuthorizationException>(() => ctx.RenderComponent<SimpleAuthView>());

			// assert
			Assert.Equal("AuthenticationStateProvider", ex.ServiceName);
			Assert.Equal("https://bunit.egilhansen.com/docs/test-doubles/faking-auth", ex.HelpLink);
		}

		[Fact(DisplayName = "AuthorizeView with set policy with authenticated and authorized user")]
		public void Test007()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser").SetPolicies("ContentViewer");

			// act
			var cut = ctx.RenderComponent<SimpleAuthViewWithPolicy>();

			// assert
			cut.MarkupMatches("Authorized for content viewers.");
		}

		[Fact(DisplayName = "AuthorizeView without policy set")]
		public void Test008()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser");
			// act
			var cut = ctx.RenderComponent<SimpleAuthViewWithPolicy>();

			// assert
			cut.MarkupMatches("");
		}

		[Fact(DisplayName = "AuthorizeView with wrong policy set")]
		public void Test0081()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser").SetPolicies("OtherPolicy");

			// act
			var cut = ctx.RenderComponent<SimpleAuthViewWithPolicy>();

			// assert
			cut.MarkupMatches("");
		}

		[Fact(DisplayName = "SimpleAuthViewWithRole with set role")]
		public void Test009()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser").SetRoles("Admin");

			// act
			var cut = ctx.RenderComponent<SimpleAuthViewWithRole>();

			// assert
			cut.MarkupMatches("Authorized content for admins.");
		}

		[Fact(DisplayName = "AuthorizeView without set role")]
		public void Test010()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser");

			// act
			var cut = ctx.RenderComponent<SimpleAuthViewWithRole>();

			// assert
			cut.MarkupMatches("");
		}

		[Fact(DisplayName = "AuthorizeView with wrong role set")]
		public void Test011()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser").SetRoles("NotAdmin");

			// act
			var cut = ctx.RenderComponent<SimpleAuthViewWithRole>();

			// assert
			cut.MarkupMatches("");
		}

		[Fact(DisplayName = "AuthorizeView in authorizing state")]
		public void Test012()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorizing();

			// act
			var cut = ctx.RenderComponent<SimpleAuthView>();

			// assert
			cut.MarkupMatches("Authorizing...");
		}

		[Fact(DisplayName = "AuthorizeView with claims")]
		public void Test013()
		{
			// arrange
			var userId = new Guid("{5d5fa9c1-abf9-4ed6-8fb0-3365382b629c}");
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			var emailClaim = new Claim(ClaimTypes.Email, "user@test.com");
			var uuidClaim = new Claim(ClaimTypes.Sid, userId.ToString());
			authContext.SetAuthorized("TestUser").SetClaims(uuidClaim, emailClaim);

			// act
			var cut = ctx.RenderComponent<SimpleAuthViewWithClaims>();

			// assert
			cut.MarkupMatches(@$"<div>Authorized!</div>
								<div>Name: TestUser</div>
								<div>Email: user@test.com</div>
								<div>Id: {userId}</div>");
		}


		[Fact(DisplayName = "AuthorizeView without defined claims")]
		public void Test014()
		{
			// arrange
			using var ctx = new TestContext();
			var authContext = ctx.Services.AddTestAuthorization();
			authContext.SetAuthorized("TestUser");

			// act
			var cut = ctx.RenderComponent<SimpleAuthViewWithClaims>();

			// assert
			cut.MarkupMatches(@$"<div>Authorized!</div>
								<div>Name: TestUser</div>");
		}
	}
}
